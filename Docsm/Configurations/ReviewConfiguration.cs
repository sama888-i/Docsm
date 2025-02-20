using Docsm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Docsm.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasOne(r => r.Doctor)
              .WithMany(d => d.Reviews)
              .HasForeignKey(r => r.DoctorId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Patient)
               .WithMany(p => p.Reviews)
               .HasForeignKey(r => r.PatientId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Property(r => r.Comment)
              .IsRequired()
              .HasMaxLength(1000);

            
        }
    }
}
