using Docsm.DataAccess;
using Docsm.DTOs.AppointmentDtos;
using Docsm.Exceptions;
using Docsm.Helpers.Enums.Status;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Docsm.Services.Implements
{
    public class AppointmentService(ApoSystemDbContext _context, IPaymentService _service,
        UserManager<User> _userManager, IHttpContextAccessor _acc) : IAppointmentService
    {
        public async Task<string> CreateAppointmentAsync(AppointmentCreateDto dto)
        {
            var userId = _userManager.GetUserId(_acc.HttpContext.User);
            if (userId == null)
                throw new UnauthorizedAccessException<User>();
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null)
                throw new Exception("Profile not complete");

            var schedule = await _context.DoctorTimeSchedules
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(s => s.Id == dto.DoctorScheduleId &&s.DoctorId ==dto.DoctorId);

            if (schedule == null || !schedule.IsAvailable)
                throw new Exception("The selected time is not available or the doctor selection is incorrect");

            decimal price = schedule.Doctor.PerAppointPrice;

            string paymentIntentId = await _service.ProcessPayment(dto.CardDetails, price, "azn");
            if (string.IsNullOrEmpty(paymentIntentId))
            {
                throw new BadRequestException("Payment failed, please check again.");
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var appointment = new Appointment
                {
                    DoctorId = schedule.DoctorId,
                    PatientId = patient.Id,
                    DoctorScheduleId = dto.DoctorScheduleId,
                    Status = AppointmentStatus.Pending,
                    ReasonAppointment = dto.ReasonAppointment,                 
                };
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                var payment = new Payment 
                {
                    Amount =price ,
                    Currency ="azn",
                    PaymentStatus = PaymentStatus.Pending,
                    PaymentIntentId = paymentIntentId,
                    AppointmentId=appointment.Id
                };
                await _context.Payments.AddAsync(payment);
                schedule.IsAvailable = false;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return ("The appointment was created, the doctor's confirmation is awaited.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw new BadRequestException("The appointment could not be created, please check again.");
            }
        }
        public async Task<string> ConfirmAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(x=>x.Payment).FirstOrDefaultAsync(x=>x.Id ==appointmentId);
            if (appointment == null || appointment.Status != AppointmentStatus.Pending)
                throw new Exception("Appointment not found or cannot be confirmed.");

            await _service.CapturePayment(appointment.Payment.PaymentIntentId);

            appointment.Status = AppointmentStatus.Confirmed;
            appointment.Payment.PaymentStatus = PaymentStatus.Paid;
            await _context.SaveChangesAsync();

            return ("Appointment confirmed and payment completed.");
        }

        public async Task<string> CancelAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(x => x.Payment).FirstOrDefaultAsync(x => x.Id == appointmentId);
            if (appointment == null)
                throw new NotFoundException<Appointment>();
            if (appointment.Status != AppointmentStatus.Pending)
                throw new Exception("This appointment has already been confirmed");

            await _service.RefundPayment(appointment.Payment.PaymentIntentId);

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.Payment.PaymentStatus =PaymentStatus.Refunded;
            await _context.SaveChangesAsync();

            return ("The appointment was canceled and payment was returned.");
        }
        public async Task<string>CompleteAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(x => x.Id == appointmentId);
            if (appointment == null )
                throw new NotFoundException<Appointment>();
            if (appointment.Status == AppointmentStatus.Confirmed)
            {
                appointment.Status = AppointmentStatus.Completed;
                await _context.SaveChangesAsync();
                return ("Appointment is completed");

            }
            return("Only accepted appointment can be completed");
          
        }
    }
}
