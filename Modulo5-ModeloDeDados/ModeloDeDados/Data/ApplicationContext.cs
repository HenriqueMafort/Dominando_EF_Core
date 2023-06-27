using System;
using System.Reflection;
using Curso;
using Curso.Configuration;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;



namespace Curso.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Conversor> Conversores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string srtConnection = "Server=localhost; Database=ConsultasDB; Trusted_Connection=True; TrustServerCertificate=True;";

            optionsBuilder
            .UseSqlServer(srtConnection)
            .LogTo(Console.WriteLine, LogLevel.Information) // Loglevel information deixara o codigo mais enxuto. trazendo somente os logs simplificadamente
            .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* modelBuilder.HasDefaultSchema("cadastros");

             modelBuilder.Entity<Estado>().ToTable("Estados", "SegundoEsquema");


             var conversao = new ValueConverter<Versao, string>(p => p.ToString(), p => (Versao)Enum.Parse(typeof(Versao), p));
             var conversao1 = new EnumToStringConverter<Versao>();//mesma coisa que o de cima porém mais curto

             modelBuilder.Entity<Conversor>()
         .Property(p => p.Versao)
         .HasConversion(conversao1);
             //.HasConversion(conversao);
             /*.HasConversion(p => p.ToString(), p => (Versao)Enum.Parse(typeof(Versao), p)); //value Convert.
             //.HasConversion<string>();


             modelBuilder.Entity<Conversor>()
         .Property(p => p.Status)
         .HasConversion(new Conversores.ConversorCustomizado());


             modelBuilder.Entity<Departamento>().Property<DateTime>("UltimaAtualizacao");*/


            //modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

           /* modelBuilder.Entity<Governador>()//Aqui estou direcionando
            .HasOne(p => p.Estado)
            .WithOne(p => p.Governador)
            .HasForeignKey<Governador>("EstadoId");*/
        }

    }

}



























/* 
                modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI"); //CI = não diferencia maiuscula de minuscula  CS = Sensitive  AI = Acentuação Ignora AS = Sensitive acentuação.
                                                                           //RAFAEL => Rafael;
                                                                           //João => Joao;

                modelBuilder.Entity<Departamento>().Property(p => p.Descricao).UseCollation("SQL_Latin1_General_CP1_CS_AS");
                //Sequencia a baixo//
                modelBuilder
                .HasSequence<int>("MinhaSequencia", "sequencias") // seguencia passando como parametro Minha sequencia e Sequencias
                .StartsAt(1) //valor que starta a sequencia
                .IncrementsBy(2) //incrementa de 2 em 2
                .HasMin(1) //valor minimo para sequencia
                .HasMax(10)// valor maximo para sequencia
                .IsCyclic(); // não estora um exeption

                modelBuilder.Entity<Departamento>().Property(p => p.Id).HasDefaultValueSql("NEXT VALUE FOR sequencias.MinhaSequencia");//define que o Id sera gerado de acordo com o codigo a cima*/
/*      
modelBuilder
.Entity<Departamento>()
.HasIndex(p => new { p.Descricao, p.Ativo })
.HasDatabaseName("idx_meu_indice_composto")
.HasFilter("Descricao IS NOT NULL")
.HasFillFactor(80)
.IsUnique();*/


// modelBuilder.Entity<Estado>()
// .HasData(new[]
// {
//     new Estado{Id = 1, Nome = "Sao Paulo"},
//     new Estado{Id = 2, Nome = "Paraná"}
// });
// /*/