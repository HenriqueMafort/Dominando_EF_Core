using System;
using System.Collections.Generic;
using System.Linq;
using Curso.Data;

namespace Consultas
{
    class Program
    {
        static void Main(string[] args)
        {

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
                                Nome = "Carlos Henrique",
                                CPF = "11111111111",
                                RG = "111111111"
                            }
                        },
                        Excluido = true // estou dizendo que empregado já foi excluido.
                    },
                    new Departamento // criação de novo departamento igual ao de cima
                    {
                        Ativo = true,
                        Descricao = "Departamento 01",
                        Funcionarios = new List<Funcionario> //novo funcionario
                        {
                            new Funcionario
                            {
                                Nome = "Carlos Henrique 02",
                                CPF = "2222222222",
                                RG = "222222222"
                            }
                        },
                    });
                db.SaveChanges(); // persiste os dados que estão em memoria e salva dntro do banco de dados
                db.ChangeTracker.Clear(); // limpa todos os dados que estão em memória
            }
        }
    }
}
