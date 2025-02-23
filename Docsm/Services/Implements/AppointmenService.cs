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
                throw new Exception("Profil tamamlanmayib");

            var schedule = await _context.DoctorTimeSchedules
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(s => s.Id == dto.DoctorScheduleId &&s.DoctorId ==dto.DoctorId);

            if (schedule == null || !schedule.IsAvailable)
                throw new Exception("Seçilmiş vaxt mövcud deyil ve ya hekim secimi yanlisdir");

            decimal price = schedule.Doctor.PerAppointPrice;

            string paymentIntentId = await _service.ProcessPayment(dto.CardDetails, price, "azn");
            if (string.IsNullOrEmpty(paymentIntentId))
            {
                throw new BadRequestException("Ödəniş alınmadı, zəhmət olmasa yenidən yoxlayın.");
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
                return ("Görüş yaradıldı , Həkimin təsdiqi gözlənilir.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw new BadRequestException("Görüş yaradıla bilmədi, zəhmət olmasa yenidən yoxlayın.");
            }
        }
        public async Task<string> ConfirmAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(x=>x.Payment).FirstOrDefaultAsync(x=>x.Id ==appointmentId);
            if (appointment == null || appointment.Status != AppointmentStatus.Pending)
                throw new Exception("Görüş tapılmadı və ya təsdiqlənə bilməz.");

            await _service.CapturePayment(appointment.Payment.PaymentIntentId);

            appointment.Status = AppointmentStatus.Confirmed;
            appointment.Payment.PaymentStatus = PaymentStatus.Paid;
            await _context.SaveChangesAsync();

            return ("Görüş təsdiqləndi və ödəniş tamamlandı.");
        }

        public async Task<string> CancelAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(x => x.Payment).FirstOrDefaultAsync(x => x.Id == appointmentId);
            if (appointment == null)
                throw new NotFoundException<Appointment>();
            if (appointment.Status != AppointmentStatus.Pending)
                throw new Exception("Bu gorus artiq tesdiqlenib");

            await _service.RefundPayment(appointment.Payment.PaymentIntentId);

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.Payment.PaymentStatus =PaymentStatus.Refunded;
            await _context.SaveChangesAsync();

            return ("Görüş ləğv edildi və odenis geri qaytarıldı.");
        }
        public async Task<string>CompleteAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(x => x.Payment).FirstOrDefaultAsync(x => x.Id == appointmentId);
            if (appointment == null )
                throw new NotFoundException<Appointment>();
            if (appointment.Status == AppointmentStatus.Confirmed)
            {
                appointment.Status = AppointmentStatus.Completed;
                await _context.SaveChangesAsync();
                return ("Gorus tamamlandi");

            }
            return("Yanliz qebul edilmis gorusler tamam lana biler");
          
        }
    }
}
