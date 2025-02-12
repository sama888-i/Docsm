using Docsm.Helpers.Enums;
using Docsm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Docsm.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {

            builder
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p=> p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(p => p.BloodGroup)
               .HasConversion(
                   v => v.ToString(),
                   v => (BloodGroups)Enum.Parse(typeof(BloodGroups), v)
               )
               .IsRequired();
            builder.Property(p => p.Address)
                .HasMaxLength(50);
            builder.Property(p=>p.Country)
                .HasMaxLength(50);
        }
    }
}
