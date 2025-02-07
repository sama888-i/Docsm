using Docsm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Docsm.Configurations
{
    public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.HasIndex(s => s.Name)
                .IsUnique();
            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(128);
            builder.Property(s => s.ImageUrl)
                .HasMaxLength(388);
        }
    }
}
