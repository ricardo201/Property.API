using PropertyBuilding.Infrastructure.Repository;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Infrastructure.Data;
using System.Threading.Tasks;

namespace PropertyBuilding.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PropertyBuildingDataBaseContext _dataBaseContext;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Owner> _ownerRepository;
        private readonly IPropertyRepositoy _propertyRepository;
        private readonly PropertyImageRepository _propertyImageRepository;
        public UnitOfWork(PropertyBuildingDataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }
        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_dataBaseContext);
        public IRepository<Owner> OwnerRepository => _ownerRepository ?? new Repository<Owner>(_dataBaseContext);
        public IPropertyRepositoy PropertyRepository => _propertyRepository ?? new PropertyRepository(_dataBaseContext);
        public IPropertyImageRepository PropertyImageRepository => _propertyImageRepository ?? new PropertyImageRepository(_dataBaseContext);
        public void Dispose()
        {
            if (_dataBaseContext != null)
            {
                _dataBaseContext.Dispose();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dataBaseContext.SaveChangesAsync();
        }

        public void ChangeTrackerClear()
        {
            _dataBaseContext.ChangeTracker.Clear();
        }
    }
}
