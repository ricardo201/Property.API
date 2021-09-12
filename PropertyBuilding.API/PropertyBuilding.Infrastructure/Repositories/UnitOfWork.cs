using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PropertyBuildingDataBaseContext _dataBaseContext;
        private readonly IUserRepository _userRepository;
        public UnitOfWork(PropertyBuildingDataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }
        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_dataBaseContext);
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
    }
}
