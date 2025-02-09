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
        }
    }
}
