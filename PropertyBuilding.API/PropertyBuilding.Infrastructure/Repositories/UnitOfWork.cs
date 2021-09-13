﻿using PropertyBuilding.Core.Entities;
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
        private readonly IRepository<Owner> _ownerRepository;
        public UnitOfWork(PropertyBuildingDataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }
        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_dataBaseContext);
        public IRepository<Owner> OwnerRepository => _ownerRepository ?? new Repository<Owner>(_dataBaseContext);
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
