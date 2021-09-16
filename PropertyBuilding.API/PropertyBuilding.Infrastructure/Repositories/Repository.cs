using Microsoft.EntityFrameworkCore;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyBuilding.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly PropertyBuildingDataBaseContext _dataBaseContext;
        protected DbSet<T> _entities;
        public Repository(PropertyBuildingDataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
            _entities = _dataBaseContext.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            entity.Status = StatusType.Active;
            await _entities.AddAsync(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            T entity = await GetByIdAsync(id);
            entity.Status = StatusType.Inactive;
            _entities.Update(entity);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id && item.Status == StatusType.Active);
        }

        public IEnumerable<T> GetList()
        {
            return _entities.Where(item => item.Status == StatusType.Active).AsEnumerable();
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }
    }
}