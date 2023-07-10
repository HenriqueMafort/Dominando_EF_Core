using System;
using System.Linq;
using Domain;

namespace Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            //CriandoPessoa();
            //AlteraPessoa();
           //DeletePessoa();
            //BuscarPessoa();
        }
        static void CriandoPessoa()
    {
        using var db = new Data.ApplicationContext();
        
        db.Database.EnsureCreated();

        var funcionario = new Pessoa{
            Nome = "Henrique Mafort Rocha",
        };
        db.Add(funcionario);
        db.SaveChanges();
    }

        static void BuscarPessoa()
        {
            using var db = new Data.ApplicationContext();

            var pessoas = db.Pessoas.Where(p => p.Id  > 0 ).ToList(); // cria uma variavel departamentos recebenco o db.Departamentos("Aqui temos um filtro que diz ("onde o id for maior que 0")" o ToList signfica que podemos fazer mais de uma consulta pois ela esta em lista e não em objeto) 

            foreach (var pessoa in pessoas) // varre tudo oque tem dentro de departamentos jogando para departamento
            {
                Console.WriteLine($"Id: {pessoa.Id} \t Nome: {pessoa.Nome}"); // escreve as descrições da busca
            }
        }

        static void AlteraPessoa()
        {
            using var db = new Data.ApplicationContext();

            var pessoa = db.Pessoas.FirstOrDefault(p => p.Id == 4); 
           pessoa.Id = 3 ;

            db.SaveChanges();
        }
        static void DeletePessoa()
        {
            using var db = new Data.ApplicationContext();

            var p1 = db.Pessoas.FirstOrDefault(p => p.Id == 3);
         
                db.Pessoas.Remove(p1);
                db.SaveChanges();      
        }

        static void AdicionarCarro()
        {
            var newCarro = new Carro(
            {
                CarroId = 1,

            }
        );
        }
    }
    
}
