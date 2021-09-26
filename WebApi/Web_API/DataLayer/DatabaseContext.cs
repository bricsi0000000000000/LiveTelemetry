using Microsoft.EntityFrameworkCore;
using DataLayer.Models;

namespace DataLayer
{
    public class DatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=RICSI; Initial Catalog=DataCenter; Integrated Security=SSPI;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Session> Session { get; set; }
        public DbSet<Sensor> Sensor { get; set; }
        public DbSet<SensorValue> SensorValue { get; set; }
    }
}
