using System;
using System.IO;
using Curso;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Curso.Data
{
    public class ApplicationContext : DbContext
    {
        private readonly StreamWriter _writer = new StreamWriter("meu_log_do_ef_core.txt", append: true); // esse private ele pega todo o log que antes era compilado dentro do CLS do editor e transcreve para um arquivo em texto. com isso e o Dispose la em baixo podemos gravar as informações de LOG dentro de um arquivo separado
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string srtConnection = "Server=localhost; Database=ConsultasDB; Trusted_Connection=True; TrustServerCertificate=True;";

            optionsBuilder
            .UseSqlServer(srtConnection,
             o => o
             .MaxBatchSize(100)//aumenta a quantidade de registros em massa que por padrão vem de 42 inserções
             .CommandTimeout(5) // tempo de timeout aumentado
             .EnableRetryOnFailure(4, TimeSpan.FromSeconds(10), null))  //Executa 4 vezes o mesmo processo tentando executar. deixando mais resiliente 
            .LogTo(Console.WriteLine, LogLevel.Information) // Loglevel information deixara o codigo mais enxuto. trazendo somente os logs simplificadamente
            .EnableSensitiveDataLogging()
            ;

        }
        public override void Dispose() // sobreescreve o metodo dispose e faz o flush para o _writer. Com isso o Log que é criado são gravado as informações dentro dele.
        {
            base.Dispose();
            _writer.Dispose();
        }

    }

}









            /*  .LogTo(Console.WriteLine, new[] { CoreEventId.ContextInitialized, RelationalEventId.CommandExecuted }
                         , LogLevel.Information,
                         DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.SingleLine // escreve o log em uma unica linha.
                         )*/
            //.LogTo(_writer.WriteLine, LogLevel.Information);
            //.EnableDetailedErrors(); // detalha para mim onde está as exeptions causadas num dotnet run em debugging. Com isso fica mais facil encontrar os erros porém causa uma sobrecarga no sistema devido a estar monitorando todo o codigo.
            //.EnableSensitiveDataLogging() // Com isso voce exibe dentro do log de busca de dados as informações que são sensiveis. ATENCAO usar somente em ambiente de desenvolvimento.
            ;