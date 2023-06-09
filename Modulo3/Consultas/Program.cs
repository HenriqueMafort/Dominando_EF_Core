using System;
using System.Collections.Generic;
using System.Linq;
using Curso.Data;
using Microsoft.EntityFrameworkCore;

namespace Consultas
{
    class Program
    {
        static void Main(string[] args)
        {
            //FiltroGlobal();
            //IgnoreFiltroGlobal();
            //AtualizaDado();
            //ConsultasProjetadas();
            //ConsultaParametrizada();
            //ConsultaComTag();
            //EntendendoConsulta1NeN1();
            //DivisaoDeConsultas();
            //CriarStoredProcedure();
            //InserirDadosViaProcedure();
            //CriarStoredProcedureDeConsulta();
            //DeletarProcedure();
            //ConsultaViaProcedure();
        }
        static void AtualizaDado()
        {
            using var db = new ApplicationContext();

            var funcionario = db.Funcionarios.FirstOrDefault(p => p.Id == 3);
            funcionario.Nome = "Carlos Rocha";
            // db.Clientes.Update(cliente);
            db.SaveChanges();
        }


        static void IgnoreFiltroGlobal() // Filtro global
        {
            using var db = new ApplicationContext(); // cria objeto a partir de um novo aplication context

            Setup(db); // executa o parametro Setup criado passando o objeto como parametro

            //ignorequery filter irá ignorar o filtro global para essa configuração que está logo a baixo
            var departamentos = db.Departamentos.IgnoreQueryFilters().Where(p => p.Id > 0).ToList(); // cria uma variavel departamentos recebenco o db.Departamentos("Aqui temos um filtro que diz ("onde o id for maior que 0")" o ToList signfica que podemos fazer mais de uma consulta pois ela esta em lista e não em objeto) 

            foreach (var departamento in departamentos) // varre tudo oque tem dentro de departamentos jogando para departamento
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}"); // escreve as descrições e oque foi excluido
            }
        }
        static void FiltroGlobal() // Filtro global
        {
            using var db = new ApplicationContext(); // cria objeto a partir de um novo aplication context

            Setup(db); // executa o parametro Setup criado passando o objeto como parametro

            var departamentos = db.Departamentos.Where(p => p.Id > 0).ToList(); // cria uma variavel departamentos recebenco o db.Departamentos("Aqui temos um filtro que diz ("onde o id for maior que 0")" o ToList signfica que podemos fazer mais de uma consulta pois ela esta em lista e não em objeto) 

            foreach (var departamento in departamentos) // varre tudo oque tem dentro de departamentos jogando para departamento
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}"); // escreve as descrições e oque foi excluido
            }
        }
        static void Setup(ApplicationContext db) //Metodo criado esperando parametro do aplication context db
        {
            if (db.Database.EnsureCreated()) // espera um valor bool true para executar essa regra de negocio espera que exista pelo menos algum dado dentro do db
            {
                db.Departamentos.AddRange(//aqui ele ira adicionar oque estiver dentro do parenteses em memoria para quando eu quiser eu usar o savechange e persistir os dados dentro do banco de dados
                    new Departamento // cria um novo departamento no aplicationcontext db
                    {
                        Ativo = true,
                        Descricao = "Departamento 01",
                        Funcionarios = new List<Funcionario> // adiciona um novo funcionario usando a lita funcionarios
                        {
                            new Funcionario
                            {
                                Nome = "Carlos Henrique 01",
                                CPF = "11111111111",
                                RG = "111111111"
                            }
                        },
                        Excluido = true // estou dizendo que empregado já foi excluido.
                    },
                    new Departamento // criação de novo departamento igual ao de cima
                    {
                        Ativo = true,
                        Descricao = "Departamento 02",
                        Funcionarios = new List<Funcionario> //novo funcionario
                        {
                            new Funcionario
                            {
                                Nome = "Carlos Henrique 02",
                                CPF = "2222222222",
                                RG = "222222222"
                            },
                            new Funcionario
                            {
                                Nome = "Carlos Henrique 02",
                                CPF = "2222222222",
                                RG = "222222222"
                            }
                        }
                    });
                db.SaveChanges(); // persiste os dados que estão em memoria e salva dntro do banco de dados
                db.ChangeTracker.Clear(); // limpa todos os dados que estão em memória
            }
        }



        //------------- Consultas projetadas----------------------//


        static void ConsultasProjetadas() // Esse filtro faz consultas mais otimizadas (usado para quando voce quer trazer do banco de dados somente algumas colunas de dados e não totas . isso evita varias consultas indesejadas otimizando o processo de consultas)
        {
            using var db = new ApplicationContext(); // cria objeto a partir de um novo aplication context

            Setup(db); // executa o parametro Setup criado passando o objeto como parametro

            //ignorequery filter irá ignorar o filtro global para essa configuração que está logo a baixo
            var departamentos = db.Departamentos
            .Where(p => p.Id > 0)
            .Select(p => new { p.Descricao, Funcionarios = p.Funcionarios.Select(f => f.Nome) })
            .ToList(); // cria uma variavel departamentos recebenco o db.Departamentos("Aqui temos um filtro que diz ("onde o id for maior que 0")" o ToList signfica que podemos fazer mais de uma consulta pois ela esta em lista e não em objeto) 

            foreach (var departamento in departamentos) // varre tudo oque tem dentro de departamentos jogando para departamento
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}"); // escreve as descrições e oque foi excluido

                foreach (var funcionario in departamento.Funcionarios) // varre tudo oque tem dentro de departamentos jogando para departamento
                {
                    Console.WriteLine($"Nome: {funcionario} "); // escreve as descrições e oque foi excluido
                }
            }
        }



        //-------------------------- Consultas parametrizadas --------------------------------''



        static void ConsultaParametrizada() // Esse filtro faz consultas mais otimizadas (usado para quando voce quer trazer do banco de dados somente algumas colunas de dados e não totas . isso evita varias consultas indesejadas otimizando o processo de consultas) 
        {
            using var db = new ApplicationContext(); // cria objeto a partir de um novo aplication context

            Setup(db); // executa o parametro Setup criado passando o objeto como parametro

            var id = 1;

            //ignorequery filter irá ignorar o filtro global para essa configuração que está logo a baixo
            var departamentos = db.Departamentos
            .FromSqlRaw("SELECT * FROM Departamentos WHERE Id>{0}", id) // Recebe uma string e executa aconsulta na base de dados
            .Where(p => !p.Excluido) // onde tudo for false em excluido
           .ToList(); // cria uma variavel departamentos recebenco o db.Departamentos("Aqui temos um filtro que diz ("onde o id for maior que 0")" o ToList signfica que podemos fazer mais de uma consulta pois ela esta em lista e não em objeto) 

            foreach (var departamento in departamentos) // varre tudo oque tem dentro de departamentos jogando para departamento
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}"); // escreve as descrições e oque foi excluido


            }
        }



        // ----------------------------  Consultas Interpoladas --------------------------------//


        static void ConsultaInterpolada() // Esse filtro faz consultas mais otimizadas (usado para quando voce quer trazer do banco de dados somente algumas colunas de dados e não totas . isso evita varias consultas indesejadas otimizando o processo de consultas)
        {
            using var db = new ApplicationContext(); // cria objeto a partir de um novo aplication context

            Setup(db); // executa o parametro Setup criado passando o objeto como parametro

            var id = 1;

            //ignorequery filter irá ignorar o filtro global para essa configuração que está logo a baixo
            var departamentos = db.Departamentos
            .FromSqlInterpolated($"SELECT * FROM Departamentos WHERE Id>{id}") // Recebe uma string interpolada e executa aconsulta na base de dados

           .ToList(); // cria uma variavel departamentos recebenco o db.Departamentos("Aqui temos um filtro que diz ("onde o id for maior que 0")" o ToList signfica que podemos fazer mais de uma consulta pois ela esta em lista e não em objeto) 

            foreach (var departamento in departamentos) // varre tudo oque tem dentro de departamentos jogando para departamento
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}"); // escreve as descrições e oque foi excluido


            }
        }



        // --------------------------------- Consultas usando Tags -----------------------""



        static void ConsultaComTag() // Esse filtro faz consultas mais otimizadas (usado para quando voce quer trazer do banco de dados somente algumas colunas de dados e não totas . isso evita varias consultas indesejadas otimizando o processo de consultas)
        {
            using var db = new ApplicationContext(); // cria objeto a partir de um novo aplication context

            Setup(db); // executa o parametro Setup criado passando o objeto como parametro



            //ignorequery filter irá ignorar o filtro global para essa configuração que está logo a baixo
            var departamentos = db.Departamentos
            .TagWith("Estou enviando um comentario para o servidor") // enviando comentario para o servidor. por meio de TAGS

           .ToList(); // cria uma variavel departamentos recebenco o db.Departamentos("Aqui temos um filtro que diz ("onde o id for maior que 0")" o ToList signfica que podemos fazer mais de uma consulta pois ela esta em lista e não em objeto) 

            foreach (var departamento in departamentos) // varre tudo oque tem dentro de departamentos jogando para departamento
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}"); // escreve as descrições e oque foi excluido


            }
        }




        // ------------------------------- entendendo consultas de N1 ou 1N  ----------------------------------------//



        static void EntendendoConsulta1NeN1() // Esse filtro faz consultas mais otimizadas (usado para quando voce quer trazer do banco de dados somente algumas colunas de dados e não totas . isso evita varias consultas indesejadas otimizando o processo de consultas)
        {
            using var db = new ApplicationContext(); // cria objeto a partir de um novo aplication context

            Setup(db); // executa o parametro Setup criado passando o objeto como parametro

            var funcionarios = db.Funcionarios
            .Include(p => p.Departamento)
            .ToList();

            foreach (var funcionario in funcionarios) // varre tudo oque tem dentro de departamentos jogando para departamento
            {
                Console.WriteLine($"Nome: {funcionario.Nome} Descrição Dep: {funcionario.Departamento.Descricao}"); // escreve as descrições e oque foi excluido




            }

            //ignorequery filter irá ignorar o filtro global para essa configuração que está logo a baixo
            /* var departamentos = db.Departamentos
             .Include(p => p.Funcionarios)

            .ToList(); // cria uma variavel departamentos recebenco o db.Departamentos("Aqui temos um filtro que diz ("onde o id for maior que 0")" o ToList signfica que podemos fazer mais de uma consulta pois ela esta em lista e não em objeto) 

             foreach (var departamento in departamentos) // varre tudo oque tem dentro de departamentos jogando para departamento
             {
                 Console.WriteLine($"Descrição: {departamento.Descricao}"); // escreve as descrições e oque foi excluido

                 foreach (var funcionario in departamento.Funcionarios) // varre tudo oque tem dentro de departamentos jogando para departamento
                 {
                     Console.WriteLine($"Descrição: {funcionario.Nome}"); // escreve as descrições e oque foi excluido


                 }

             }*/

        }



        // ---------------------- Divisão de consultas com SplitQuery ----------------------------------//

        static void DivisaoDeConsultas()
        {
            using var db = new Curso.Data.ApplicationContext(); //Criando variavel db recebendo os dados do Application context.
            Setup(db); //passando o db como parametro para o metodo Setup

            var departamentos = db.Departamentos //departamentos vai receber todos os dados dos Departamentos
            .Include(p => p.Funcionarios) //estou instruindo o EFCore a trazer todos os dados da propriedade Funcionarios
            .Where(p => p.Id < 3) //filtro Where onde ID for menor que 3
            //.AsSplitQuery() //faz um filtro parecido com o explicito. tras somente dados relarionados da tabela evitando duplicidade e explosão cartesiana
            .AsSingleQuery() // ignora o splitquery. traz tudo em uma consulta só
            .ToList(); //mais de uma consulta simultanea

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\tNome: {funcionario.Nome}");
                }
            }

        }


        // ---------------------------------- Criação de uma Stored Procedure  ------------------------------------------//


        static void CriarStoredProcedure()
        {
            //temos uma variavel que vai receber os dados para gerar a stored procedure. ela irá esperar 2 dados que é a descricao e ativo e ira introduzir no banco atraver do Begin insert into. departamentos e ja levar o excluido junto.
            var criarDepartamento = @"
                    CREATE OR ALTER PROCEDURE CriarDepartamento
                    @Descricao VARCHAR(50),
                    @Ativo bit
                    
                    AS
                    BEGIN
                        INSERT INTO
                            Departamentos(Descricao, Ativo, Excluido)
                        VALUES(@Descricao, @Ativo, 0)
                    END
                ";

            using var db = new Curso.Data.ApplicationContext(); // criando um parametro db 

            db.Database.ExecuteSqlRaw(criarDepartamento); // executando o metodo database para executar o sql no banco
        }


        static void InserirDadosViaProcedure() // processo simples usado para persistir os dados no banco de dados atraves do procedure.
        {

            using var db = new Curso.Data.ApplicationContext();

            db.Database.ExecuteSqlRaw("execute CriarDepartamento @p0, @p1", "Departamento Via Procedure", true); // executando a procedure criada e passando @p0 como primeiro parametro "Departamento " e @p1 como segundo true.
        }


        static void CriarStoredProcedureDeConsulta()
        {
            var consultaDepartamento = @"
                    CREATE OR ALTER PROCEDURE GetDepartamentos
                    @Descricao VARCHAR(50)
                    
                    AS
                    BEGIN
                       SELECT * FROM Departamentos WHERE Descricao Like @Descricao + '%'
                    END
                ";

            using var db = new Curso.Data.ApplicationContext();

            db.Database.ExecuteSqlRaw(consultaDepartamento);
        }

        static void ConsultaViaProcedure()
        {

            using var db = new Curso.Data.ApplicationContext();

            var departamentos = db.Departamentos // variavel recebendo dados dos Departamentos
            .FromSqlRaw("EXECUTE GetDepartamentos @p0", "Departamento") // FromSQLRaw => Consultas personalizadas para que recupere resultados como objetos do EF.
            .ToList(); // Permite mais de 1 consulta simultanea.

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Departamento: {departamento.Descricao}");
            }
        }

        static void DeletarProcedure()
        {
            var deletarProcedure = @"
                    DROP PROCEDURE GetDepartamento";

            using var db = new Curso.Data.ApplicationContext();

            db.Database.ExecuteSqlRaw(deletarProcedure);
        }
    
    
            // --------------------------- INFRAESTRUTURA ----------------------//
    }
}

