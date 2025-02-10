using Docsm.Helpers.Enums.Status;
using Docsm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Docsm.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
           builder
                .HasOne(a=>a.Doctor)
                .WithMany(d=>d.Appointments)
                .HasForeignKey(a=>a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
          builder
                .HasOne(a=>a.Patient)
                .WithMany(p=>p.Appointments)
                .HasForeignKey(a=>a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
          builder.Property(a=>a.Status)
                .HasConversion(
                a => a.ToString(),
                a => (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), a)
                );
          
        }
    }
}
