using Microsoft.EntityFrameworkCore;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Infrastructure.Data;
using PropertyBuilding.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyBuilding.Infrastructure.Repository
{
    public class PropertyRepository : Repository<Property>, IPropertyRepositoy
    {
        public PropertyRepository(PropertyBuildingDataBaseContext dataBaseContext) : base(dataBaseContext)
        { }
        public async Task<IEnumerable<Property>> GetPropertiesByOwner(int idOwner)
        {
            return await _entities.Where(item => item.IdOwner == idOwner).ToListAsync(); ;
        }
    }
}