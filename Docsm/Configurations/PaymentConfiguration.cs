using Docsm.Helpers.Enums.Status;
using Docsm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Docsm.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(x => x.PaymentIntentId)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x=>x.Currency)
                .IsRequired()
                .HasMaxLength(10);
            builder.Property(x => x.Amount)
                .IsRequired();
            builder.Property(x => x.PaymentStatus)
                .IsRequired()
                .HasConversion(
                   x => x.ToString(),
                   x => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), x)
               );
            builder
                .HasOne(x => x.Appointment)
                .WithOne(x => x.Payment)
                .HasForeignKey<Payment>(x => x.AppointmentId);
                

        }
    }
}
