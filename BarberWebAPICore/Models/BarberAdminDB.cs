using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BarberWebAPICore.Models
{
    public class BarberAdminDB : DbContext
    {
        public BarberAdminDB(DbContextOptions<BarberAdminDB> options) : base(options)
        { }
        public DbSet<Barber> Barbers { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("BarberAdminDB");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
