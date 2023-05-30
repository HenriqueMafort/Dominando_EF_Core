using System;
using Curso;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Curso.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos {get;set;}
        public DbSet<Funcionario> Funcionarios {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string srtConnection="Server=localhost; Database=ConsultasDB; Trusted_Connection=True; TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(srtConnection)
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }

}