using System.Collections.Generic;

namespace Domain
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public List<Empregado> Empregados { get; set; }
    }
}