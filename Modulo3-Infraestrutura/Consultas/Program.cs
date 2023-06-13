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

            //TempoComandoGeral();
            //HabilitarBatchSize();
            //DadosSensiveis();
            //ConsultarDepartamentos();
        }
        static void ExecutarEstrategiaResiliencia()
        {
            using var db = new Curso.Data.ApplicationContext();


            var strategy = db.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using var transaction = db.Database.BeginTransaction();

                db.Departamentos.Add(new Departamento { Descricao = "Departamento Transacao" });
                db.SaveChanges();

                transaction.Commit();
            });



        }
        static void TempoComandoGeral() // 
        {


            using var db = new Curso.Data.ApplicationContext();

            db.Database.SetCommandTimeout(10); // aumenta o tempo de timeout do banco de dados. 

            db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07'; SELECT 1"); // executa um comando de SQL dentro do banco de dados atraves de EFCore
        }

        static void HabilitarBatchSize() // Esse metodo ele ira criar um total de 50 departamentos sempre colocando um Departamento + o numero atual da variavel
        {
            using var db = new Curso.Data.ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();


            for (var i = 0; i < 50; i++)
            {


                db.Departamentos.Add(
                    new Departamento
                    {
                        Descricao = "Departamento" + i
                    });
            }

            db.SaveChanges();
        }


        // -- A baixo => Habilitar vizualização de dados sensiveis --//
        static void DadosSensiveis() // deve ser usado em conjunto com o EnableSensitiveDataLogging() dentro do ApplicationContext que Habilita os dados sensiveis.
        {
            using var db = new Curso.Data.ApplicationContext();
            var descricao = "Departamento";
            var departamentos = db.Departamentos.Where(p => p.Descricao == descricao).ToArray();
        }

        //-- Metodo usado para gerenciar demonstração dos filtros que foram criados dentro do ApplicationContex() --//
        static void ConsultarDepartamentos()
        {
            using var db = new Curso.Data.ApplicationContext();

            var departamentos = db.Departamentos.Where(p => p.Id > 0).ToArray();
        }


    }


}

