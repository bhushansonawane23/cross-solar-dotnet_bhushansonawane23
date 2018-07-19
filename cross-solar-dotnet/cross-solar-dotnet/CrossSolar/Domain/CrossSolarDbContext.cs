using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace CrossSolar.Domain
{
    public class CrossSolarDbContext : DbContext
    {
        public CrossSolarDbContext()
        {
        }

        public CrossSolarDbContext(DbContextOptions<CrossSolarDbContext> options) : base(options)
        {
        }

       

        public DbSet<Panel> Panels { get; set; }

        public DbSet<OneHourElectricity> OneHourElectricitys { get; set; }

        public DbSet<OneDayElectricity> OneDayElectricitys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
         // optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=CrossSolarDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}