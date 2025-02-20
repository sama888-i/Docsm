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


            var appointment = new Appointment
            {
                DoctorId = schedule.DoctorId,
                PatientId = patient.Id,
                DoctorScheduleId = dto.DoctorScheduleId,
                Status = AppointmentStatus.Pending,
                ReasonAppointment = dto.ReasonAppointment,
                PaymentIntentId = paymentIntentId
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Appointments.Add(appointment);
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
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null || appointment.Status != AppointmentStatus.Pending)
                throw new Exception("Görüş tapılmadı və ya təsdiqlənə bilməz.");

            await _service.CapturePayment(appointment.PaymentIntentId);

            appointment.Status = AppointmentStatus.Confirmed;
            await _context.SaveChangesAsync();

            return ("Görüş təsdiqləndi və ödəniş tamamlandı.");
        }

        public async Task<string> CancelAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null )
                throw new Exception("Görüş tapılmadı ");
            if (appointment.Status != AppointmentStatus.Pending)
                throw new Exception("Bu gorus artiq tesdiqlenib");

            await _service.RefundPayment(appointment.PaymentIntentId);

            appointment.Status = AppointmentStatus.Cancelled;
            await _context.SaveChangesAsync();

            return ("Görüş ləğv edildi və odenis geri qaytarıldı.");
        }
    }
}
