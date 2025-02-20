using Docsm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Docsm.Configurations
{
    public class DocScheduleConfiguration :IEntityTypeConfiguration<DoctorTimeSchedule>
    {
        public void Configure(EntityTypeBuilder<DoctorTimeSchedule> builder)
        {
           
            builder.HasIndex(d => new { d.DoctorId, d.StartTime })
                   .IsUnique();
            builder.Property(d => d.AppointmentDate)
                   .IsRequired();
                  

            builder.Property(d => d.StartTime)
                   .IsRequired();
          
            builder.Property(d => d.EndTime)
                   .IsRequired(); 

            builder.HasOne(d => d.Doctor)
                   .WithMany(ds=>ds.TimeSchedules)
                   .HasForeignKey(d => d.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
