using PropertyBuilding.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropertyBuilding.Core.Interfaces
{
    public interface IPropertyRepositoy : IRepository<Property>
    {
        Task<IEnumerable<Property>> GetPropertiesByOwner(int idOwner);
    }
}
