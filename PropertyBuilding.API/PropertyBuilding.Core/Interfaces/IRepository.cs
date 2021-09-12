using PropertyBuilding.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropertyBuilding.Core.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        IEnumerable<T> GetList();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteByIdAsync(int id);
    }
}
