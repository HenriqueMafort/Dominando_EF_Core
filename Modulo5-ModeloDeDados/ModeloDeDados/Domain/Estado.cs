using System.Collections.Generic;

namespace Curso.Data
{
    public class Estado
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public Governador Governador { get; set; }

        public ICollection<Cidade> Cidades { get; } = new List<Cidade>();
    }

    public class Governador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Partido { get; set; }

        public int EstadoId { get; set; } // o entity precisa encontrar qual é a classe dependente e no caso o governador seria dependente do estado estado pode existir porém não pode existir o governador se não existir estado
        public Estado Estado { get; set; }
    }

    public class Cidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}