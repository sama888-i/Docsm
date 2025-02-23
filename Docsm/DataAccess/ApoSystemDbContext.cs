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
        public DbSet<Patient> Patients  { get; set; }
        public DbSet<Appointment> Appointments  { get; set; }    
        public DbSet<DoctorTimeSchedule> DoctorTimeSchedules { get; set; }    
        public DbSet<Specialty>Specialties { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Payment> Payments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly (typeof(ApoSystemDbContext).Assembly); 
            base.OnModelCreating(builder);
        }
    }
}
