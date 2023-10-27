using BenefitsCalculator.Data.Entities;
using Microsoft.EntityFrameworkCore;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BenefitsCalculator.Data
{
    public class BenefitsContext : IdentityDbContext<AppUser>
    {
        private readonly IConfiguration _config;

        public BenefitsContext(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<BenefitsHistory> BenefitsHistories { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<Setup> Setups { get; set; }
        public DbSet<BenefitsHistGroup> BenefitsHistGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config.GetConnectionString("BenefitsConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                .HasData(
                    new AppUser()
                    {
                        FirstName = "Admin",
                        LastName = "BenefitsCalc",
                        Email = "admin@benefitscalc.com",
                        UserName = "admin@benefitscalc.com"
                    });

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Consumer>()
                .Property(p => p.BirthDate)
                .HasColumnType("Date");
        }
    }
}
