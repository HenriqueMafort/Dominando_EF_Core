using System;
using System.Collections.Generic;
using System.Linq;
using Curso.Domain;
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
            ///CarregamentoAdiantado();
            //CarregamentoExplicito();
            CarremantoLento();
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

        static void CarregamentoAdiantado()
        {
            using var db = new Curso.Domain.ApplicationContext(); //criando objeto db usando instancia do ApplicationContext
            SetupTiposCarregamentos(db); // metodo criado a parte para ....

            var departamentos = db //o departamentos vai receber dados do banco de dados usando o Include() que é uma função do EFCore que agiliza as consultas
            .Departamentos
            .Include(p => p.Funcionarios);// aqui esta usando  lambda para puxar os dados dos Funcionarios 

            foreach (var departamento in departamentos) // foreach para varrer os dados que encontrou dentro do departamentos
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"Departamento:{departamento.Descricao}"); // aqui ele irá escrever os departamentos que encontrou descrição dos departamentos

                if (departamento.Funcionarios?.Any() ?? false) // verifica se existe algum funcionario e se for verdadeiro entra em IF
                {
                    foreach (var funcionario in departamento.Funcionarios)  //varre os funcinarios caso seja verdadeiro
                        Console.WriteLine($"\tFuncionario:{funcionario.Nome}");
                }
                else
                {
                    Console.WriteLine($"\tnenhum funcionario encontrado"); //escreve no console que não encontrou nenhum funcionario
                }
            }

        }

        static void SetupTiposCarregamentos(Curso.Domain.ApplicationContext db)
        {
            if (!db.Departamentos.Any()) //se os departamento dentro de db for true então
            {
                db.Departamentos.AddRange(//irá adicionar em memória os dados que irei escrever a baixo para depois se necesario usar o SaveChange para persistir dentro do banco de dados

                new Curso.Domain.Departamento//Estou criando um novo Departamento
                {
                    Descricao = "Departamento 01", // Criação de departamento Novo
                    Funcionarios = new System.Collections.Generic.List<Curso.Domain.Funcionario> // Criando uma nova instancia de uma lista vazia e atribuindo a propriedade Funcionarios
                    {
                        new Funcionario // dizendo que será um novo funcionario e a baixo está todos os dados do empregado
                        {
                            Nome = "Henrique Mafort",
                            CPF = "11111111111",
                            RG = "130493696"
                        }
                    }
                },
                new Curso.Domain.Departamento // novamente está criando um novo Departamento
                {
                    Descricao = "Departamento 02", // Com novo nome
                    Funcionarios = new System.Collections.Generic.List<Curso.Domain.Funcionario> // Nova instancia vazia e dados a baixo desse Funcionario
                    {
                    new Funcionario{// Está sendo cadastrado novo FUncionario dentro da lista Funcionario para Funcionarios
                    Nome = "Carlos Rocha",
                    CPF = "111111111111",
                    RG = "131111111"
                    },
                    new Funcionario{
                        Nome = "Henrique Rocha",
                        CPF = "1111111112222",
                        RG = "122229384"
                        }
                    }
                }
                );
                db.SaveChanges();//metodo onde persiste as informações dentro do banco de dados
                db.ChangeTracker.Clear(); // usado para limpar as alterações que estão em memoria. Com isso ele retorna a todas as informações que foram buscados em banco de dados.
            }
        }
    
        //-----------------------------------------
        //Carregamento Explicito;

        static void CarregamentoExplicito()
        {
            using var db = new Curso.Domain.ApplicationContext(); //criando objeto db usando instancia do ApplicationContext
            SetupTiposCarregamentos(db); // metodo criado a parte para ....

            var departamentos = db //o departamentos vai receber dados do banco de dados usando o Include() que é uma função do EFCore que agiliza as consultas
            .Departamentos
            .ToList();// executa varias consultas na base de dados simultaneamente;
            // aqui esta usando  lambda para puxar os dados dos Funcionarios 

            foreach (var departamento in departamentos) // foreach para varrer os dados que encontrou dentro do departamentos
            {
            if (departamento.Id == 2)
            {
                //db.Entry(departamento).Collection(p => p.Funcionarios).Load(); //Collection identifica qual propriedade de navegação queremos que o EF core preencha os dados Load() = executa novo comando na base de dados e ira materializar os dados na propriedade Funcionarios
                db.Entry(departamento).Collection(p => p.Funcionarios).Query().Where(p => p.Id > 2).ToList(); // adiciona um filtro dentro da consulta trazendo somente os Ids maiores que 2.
            }
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"Departamento:{departamento.Descricao}"); // aqui ele irá escrever os departamentos que encontrou descrição dos departamentos

                if (departamento.Funcionarios?.Any() ?? false) // verifica se existe algum funcionario e se for verdadeiro entra em IF
                {
                    foreach (var funcionario in departamento.Funcionarios)  //varre os funcinarios caso seja verdadeiro
                        Console.WriteLine($"\tFuncionario:{funcionario.Nome}");
                }
                else
                {
                    Console.WriteLine($"\tnenhum funcionario encontrado"); //escreve no console que não encontrou nenhum funcionario
                }
            }

        }


        //---------------- Carregamento Lento ------------------------ "Pouco Usado"
        //Aqui ele faz uma consulta para cada foreach... para cada departamento ele iria abrir uma conexão para fazer uma consulta
            static void CarremantoLento()
        {
            using var db = new Curso.Domain.ApplicationContext(); //criando objeto db usando instancia do ApplicationContext
            SetupTiposCarregamentos(db); // metodo criado a parte para ....

            //db.ChangeTracker.LazyLoadingEnabled = false;

            var departamentos = db //o departamentos vai receber dados do banco de dados usando o Include() que é uma função do EFCore que agiliza as consultas
            .Departamentos
            .ToList();// executa varias consultas na base de dados simultaneamente;
            // aqui esta usando  lambda para puxar os dados dos Funcionarios 

            foreach (var departamento in departamentos) // foreach para varrer os dados que encontrou dentro do departamentos
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"Departamento:{departamento.Descricao}"); // aqui ele irá escrever os departamentos que encontrou descrição dos departamentos

                if (departamento.Funcionarios?.Any() ?? false) // verifica se existe algum funcionario e se for verdadeiro entra em IF
                {
                    foreach (var funcionario in departamento.Funcionarios)  //varre os funcinarios caso seja verdadeiro
                        Console.WriteLine($"\tFuncionario:{funcionario.Nome}");
                }
                else
                {
                    Console.WriteLine($"\tnenhum funcionario encontrado"); //escreve no console que não encontrou nenhum funcionario
                }
            }

        }
    }


}
