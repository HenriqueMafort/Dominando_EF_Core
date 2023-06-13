using System;
using Curso;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Curso.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string srtConnection = "Server=localhost; Database=ConsultasDB; Trusted_Connection=True; TrustServerCertificate=True;";
            //optionsBuilder.UseSqlServer(srtConnection, p => p.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
            optionsBuilder.UseSqlServer(srtConnection)
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // basicamente estou sobreescrevendo o onmodelcreateing do departamento e dizendo para ele trazer somente os empregados que forem diferentes de true
        {
            // modelBuilder.Entity<Departamento>().HasQueryFilter(p => !p.Excluido);//o hasqueryfilter faz um filtro e tras somente oque tiver dentro do parametro. no caso seria tudo oque for diferente de true Excluido 
        }
    }

}