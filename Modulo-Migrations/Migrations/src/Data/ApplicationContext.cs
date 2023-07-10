using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Pessoa> Pessoas {get; set;}
 
    
       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string srtConnection = "Server=localhost; Database=DevIo-03; Trusted_Connection=True; TrustServerCertificate=True;";

            optionsBuilder
            .UseSqlServer(srtConnection)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>(conf => 
            {
                conf.HasKey(p => p.Id);
                conf.Property(p => p.Nome).HasMaxLength(60).IsUnicode(false);
                
            });
        }
    }
}
