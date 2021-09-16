using Microsoft.EntityFrameworkCore;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(PropertyBuildingDataBaseContext dataBaseContext) : base(dataBaseContext)
        { }

        public async Task<int> CountByUserNameAsync(string userName)
        {
            return (await _entities.CountAsync(item => item.UserName == userName));
        }

        public async Task<User> GetLoginByCredentials(User user)
        {
            return await _entities.FirstOrDefaultAsync(item => item.UserName == user.UserName && item.Password == user.Password);
        }
    }
}
