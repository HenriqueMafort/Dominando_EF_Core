using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Curso
{
    class Program
    {
        static void Main(string[] args)
        {
            //GapDoEnsureCreated();
            //HelthCheckBancoDeDados();
            //   new Curso.Domain.ApplicationContext().Departamentos.AsNoTracking().Any();
            // GerenciarEstadoDaConexao(false);
            //GerenciarEstadoDaConexao(true);
            //SqlInjection();
            //DetectandoMigracoes();
            //ForcandoMigracoes();
            //RecuperandoMigracoesEmTempoDeExecucao();
            //RecuperarMigracoesNoBancoDeDados();
            //GerarScriptGeralDoBancoDeDados();
        }
        public static void GapDoEnsureCreated() // Cria um banco de dados forcadamente
        {
            using var db1 = new Curso.Domain.ApplicationContext();
            using var db2 = new Curso.Domain.ApplicationContextCidade();

            db1.Database.EnsureCreated();
            db2.Database.EnsureCreated();

            var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
            databaseCreator.CreateTables();
        }

        public static void HelthCheckBancoDeDados() // faz teste de conexão com o banco de dados
        {//Metodo novo
            using var db = new Curso.Domain.ApplicationContext();
            var canConnect = db.Database.CanConnect();

            if (canConnect)
            {
                var conection = db.Database.GetDbConnection();

                conection.Open();

                //  db.Departamentos.Any();

                Console.WriteLine("Posso me conectar");
            }
            else
            {
                Console.WriteLine("Não Posso me conectar");
            }

            //Metodo antigo a baixo
            /*
            try
            {
                var conection  = db.Database.GetDbConnection();
                
                conection.Open();

              //  db.Departamentos.Any();

                Console.WriteLine("Posso me conectar");
            }
            catch(Exception)
            {
            Console.WriteLine("Não Posso me conectar");
            }*/
        }

        static void GerenciarEstadoDaConexao(bool gerenciarEstadoConexao)//Gerenciar estado da conexão Deixar mais veloz


        {
            using var db = new Curso.Domain.ApplicationContext();
            var time = System.Diagnostics.Stopwatch.StartNew();

            var conexao = db.Database.GetDbConnection();
            if (gerenciarEstadoConexao)
            {
                conexao.Open();
            }
            for (var i = 0; i < 200; i++)
            {
                db.Departamentos.AsNoTracking().Any();
            }

            time.Stop();
            var mensagem = $"Tempo : {time.Elapsed.ToString()}, {gerenciarEstadoConexao}";
        }

        static void ExecuteSQL() //Fazer consultas na base de dados
        {
            //Primeira Opção
            using var db = new Curso.Domain.ApplicationContext();

            using (var cmd = db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "SELECT 1";
                cmd.ExecuteNonQuery();
            }

            //Segunda opção
            var descricao = "teste";
            db.Database.ExecuteSqlRaw("update departamentos set descricao = {0} where id = '1'", descricao);

            //Terceira opção
            db.Database.ExecuteSqlInterpolated($"update departamentos set descricao = {descricao} where id = '1'");
        }

        public static void SqlInjection() // se protejento de ataques SQL Injection
        {
            using var db = new Curso.Domain.ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Departamentos.AddRange(
                new Curso.Domain.Departamento
                {
                    Descricao = "Departamento 01"
                },
                new Curso.Domain.Departamento
                {
                    Descricao = "Departamento 02"
                });
            db.SaveChanges();

            var descricao = "Departamento 01 'or 1='1";//simulando ataque de sql
            db.Database.ExecuteSqlRaw($"Update Departamentos set Descricao = 'AtaqueSqlInjection' where descricao ='{descricao}'");

            foreach (var departamento in db.Departamentos.AsNoTracking())
            {
                Console.WriteLine($"Id: {departamento.Id}, descrição: {departamento.Descricao}");
            }
        }

        static void DetectandoMigracoes() //Detectando Migrações pendentes
        {
            using var db = new Curso.Domain.ApplicationContext(); //adicionando um novo objeto ao db.

            var migracoesPendentes = db.Database.GetPendingMigrations(); //Usando o metodo  GetPendingMigrations. Verifica migrações pendentes

            Console.WriteLine($"Total: {migracoesPendentes.Count()}"); //traz um contador das migrações

            foreach (var migracao in migracoesPendentes)//foreach para varrer as migrações pendentes
            {
                Console.WriteLine($"Migração: {migracao}"); // mostrando as migrações pendentes.
            }
        }

        static void ForcandoMigracoes() //Forcando Migrações em tempo de execução (somente alternativa porém não é uma boa pratica)
        {
            using var db = new Curso.Domain.ApplicationContext();

            db.Database.Migrate();
        }

        static void RecuperandoMigracoesEmTempoDeExecucao() //Recuperar migrações em tempo de execução(Consulta)
        {
            using var db = new Curso.Domain.ApplicationContext();

            var migracoes = db.Database.GetMigrations(); // Le o assembly e le todas as migrações que já foram feitas (le os arquivos de migrações)

            Console.WriteLine($"Total: {migracoes.Count()}");//escreve total de migrações

            foreach (var migracao in migracoes)
            {
                Console.WriteLine($"Migração: {migracao}"); // escreve o nome de cada migração
            }
        }

        static void RecuperarMigracoesNoBancoDeDados() // consultar migrações aplicadas no banco de dados.
        {
            //pelo cmd podemos usar o "dotnet ef migrations list --context ApplicationContext";

            using var db = new Curso.Domain.ApplicationContext();

            var migracoes = db.Database.GetAppliedMigrations(); // Le o assembly e le todas as migrações que já foram feitas (le os arquivos de migrações)

            Console.WriteLine($"Total: {migracoes.Count()}");//escreve total de migrações aplicadas

            foreach (var migracao in migracoes)
            {
                Console.WriteLine($"Migração: {migracao}"); // escreve o nome de cada migração
            }

        }

        static void GerarScriptGeralDoBancoDeDados() // gera um script completo do banco dedados
        {
            using var db = new Curso.Domain.ApplicationContext();

            var script = db.Database.GenerateCreateScript(); // gera o script todo do banco de dados e devolve na variavel "script"

            Console.WriteLine(script);

        }
    }


}
