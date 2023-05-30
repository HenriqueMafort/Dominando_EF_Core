using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Curso.Domain
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public Departamento()
        {

        }
        private ILazyLoader _lazyLoader {get;set;}
        private Departamento (ILazyLoader lazeLoader)
        {
            _lazyLoader = lazeLoader;
        }
        private List<Funcionario> _funcionarios;
        public virtual List<Funcionario> Funcionarios 
        {
            get => _lazyLoader.Load(this, ref _funcionarios);
            set => _funcionarios = value;
        }

    }
}