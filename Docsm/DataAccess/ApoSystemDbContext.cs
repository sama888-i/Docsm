using Docsm.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Docsm.DataAccess
{
    public class ApoSystemDbContext : IdentityDbContext<User>
    {
        public ApoSystemDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Doctor> Doctors { get; set; }
      
        public DbSet<Specialty>Specialties { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly (typeof(ApoSystemDbContext).Assembly); 
            base.OnModelCreating(builder);
        }
    }
}
