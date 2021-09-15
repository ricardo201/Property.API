using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Infrastructure.Data;
using System.Linq;

namespace PropertyBuilding.Infrastructure.Repositories
{
    public class PropertyImageRepository : Repository<PropertyImage>, IPropertyImageRepository
    {
        public PropertyImageRepository(PropertyBuildingDataBaseContext dataBaseContext) : base(dataBaseContext)
        { }
        public PropertyImage GetPropertyImageByName(string fileName, int idUser)
        {
            return _entities.FirstOrDefault(item => item.File == fileName && item.IdUser == idUser && item.Enabled == true);
        }
    }
}
