using Docsm.Helpers.Enums.Status;
using Docsm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Docsm.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(d => d.Specialty)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecialtyId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Property(d => d.Adress)
             .IsRequired()  
             .HasMaxLength(256);

            builder.Property(d => d.AboutMe)
                .HasMaxLength(2000);
            builder.Property(d => d.Services)
                .HasMaxLength(300);

            builder.Property(d => d.ClinicName)
                .HasMaxLength(50);

            builder.Property(d => d.ClinicAddress)
                .HasMaxLength(50);
            builder.Property(d => d.DoctorStatus)
                .HasConversion(
                d => d.ToString(),
                d => (DoctorStatus)Enum.Parse(typeof(DoctorStatus), d)
                );
        }
    }
}
