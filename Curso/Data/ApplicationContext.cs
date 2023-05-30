using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Curso.Domain
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsbuilder)
        {
            const string strConnection = "Server=localhost; Database=C002; Trusted_Connection=True; TrustServerCertificate=True;";
            optionsbuilder.UseSqlServer(strConnection)
            .EnableSensitiveDataLogging()
            //.UseLazyLoadingProxies()
            .LogTo(Console.WriteLine, LogLevel.Information);
            
        }
    }
}