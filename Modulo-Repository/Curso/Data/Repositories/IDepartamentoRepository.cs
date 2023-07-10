using System.Threading.Tasks;
using Domain;

namespace Data.Repositories
{
    public interface IDepartamentoRepository
    {
        Task<Departamento> GetByIdAsync(int id);
        void Add(Departamento departamento);
        bool Save();
        
    }
}