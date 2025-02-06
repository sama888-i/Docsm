using Docsm.Helpers.Enums;
using Docsm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Docsm.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(32);
            builder.Property(u => u.Surname)
                .IsRequired()
                .HasMaxLength(32);
            builder.Property(u => u.ProfileImageUrl)
                .HasMaxLength(300);
            builder.Property(u => u.DateOfBirth)
                .IsRequired()
                .HasColumnType("date");
            builder.Property(u => u.Gender)
           .HasConversion(
               v => v.ToString(),  
               v => (Genders)Enum.Parse(typeof(Genders), v) 
           )
           .IsRequired();
        }
    }
}
