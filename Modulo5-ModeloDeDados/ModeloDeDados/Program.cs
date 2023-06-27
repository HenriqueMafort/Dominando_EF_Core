using System;
using System.Collections.Generic;
using System.Linq;
using Curso.Data;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;

namespace Consultas
{
    class Program
    {
        static void Main(string[] args)
        {
            Relacionamento1Para1();
            //TiposDePropriedades();
            //TrabalhandoComPropriedadesDeSombra();
            //PropriedadesDeSombra();
            //ConversorCustomizado();
            //ConversordeValor();
            //Esquema();
            //Collactions();
            //PropagarDados();

        }
        //-- Indices: --//
        static void Esquema()
        {
            using var db = new Curso.Data.ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }


        //-- Collactions são formas que o caracteres são codificados e interpretados na base de dados --// como meus dados sao ordenados e comparados na base de dados.
        static void Collactions()
        {
            using var db = new Curso.Data.ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
        //EnsureDeleted deleta a base de dados e a seguir o created cria a base de dados

        static void PropagarDados()
        {
            using var db = new Curso.Data.ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }
        static void ConversordeValor() => Esquema();

        static void ConversorCustomizado()
        {
            using var db = new Curso.Data.ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Conversores.Add(
                new Conversor
                {
                    Status = Status.Devolvido,
                });
            db.SaveChanges();

            var conversorEmAnalise = db.Conversores.AsNoTracking().FirstOrDefault(p => p.Status == Status.Analise);
            var conversorDevolvido = db.Conversores.AsNoTracking().FirstOrDefault(p => p.Status == Status.Devolvido);

        }

        static void PropriedadesDeSombra()
        {
            using var db = new Curso.Data.ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

        }

        static void TrabalhandoComPropriedadesDeSombra()
        {
            using var db = new Curso.Data.ApplicationContext();
            // db.Database.EnsureDeleted();
            // db.Database.EnsureCreated();

            // var departamento = new Departamento
            // {
            //     Descricao = "Departamento Propriedade de Sombra"
            // };

            // db.Departamentos.Add(departamento);

            // db.Entry(departamento).Property("UltimaAtualizacao").CurrentValue = DateTime.Now;



            // db.SaveChanges();

            var departamentos = db.Departamentos.Where(p => EF.Property<DateTime>(p, "UltimaAtualizacao") < DateTime.Now).ToArray();
        }

        static void TiposDePropriedades()
        {
            using var db = new Curso.Data.ApplicationContext(); //instancia do Vardb usando o app context para trazer o mapeamento do banco de dados
            db.Database.EnsureDeleted(); //deletando base de dados
            db.Database.EnsureCreated(); //criando base de dados

            var cliente = new Cliente // instanciando um novo Cliente na variavel cliente
            {
                Nome = "Fulano de Tal",
                Telefone = "(45) 999522762",
                Endereco = new Endereco { Bairro = "Centro", Cidade = "Toledo", Estado = "Paraná" }
            };

            db.Clientes.Add(cliente); // adicionando esse cliente que foi instanciado dentro da base dedados (Nâo foi salvo ainda. somente está na memória flash do banco)

            db.SaveChanges(); // agora sim esta salvo as informações hehe



            var clientes = db.Clientes.AsNoTracking().ToList(); // aqui criando uma variavel onde o clientes esta recebendo uma consulta no baco de dados. trazendo todas as consultas de Clientes.

            var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true }; // outra variavel options onde esse system irá formatar o arquivo options com identação.

            clientes.ForEach(cli => // esse codigo a seguir faz um foreach de clientes onde pega todos os dados de clientes e passa dentro do json formatando e identando o codigo.
            // logo a seguir ele escreve na tela o resultado do código.
            {
                var json = System.Text.Json.JsonSerializer.Serialize(cli, options);

                Console.WriteLine(json);
            }
            );


        }

        static void Relacionamento1Para1()
        {
            using var db = new Curso.Data.ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var estado = new Estado
            {
                Nome = "Paraná",
                Governador = new Governador { Nome = "Carlos Alberto de Nobrega" }
            };
            db.Estados.Add(estado);

            db.SaveChanges();

            var estados = db.Estados.AsNoTracking().ToList();

            estados.ForEach(est =>
                Console.WriteLine($"estado: {estado}, Governador: {est.Governador.Nome}")
            );
        }
    
        
    }
}

