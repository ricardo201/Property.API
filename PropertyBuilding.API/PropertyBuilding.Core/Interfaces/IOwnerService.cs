using PropertyBuilding.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropertyBuilding.Core.Interfaces
{
    public interface IOwnerService
    {
        Task<bool> DeleteOwnerAsync(int id);
        Task<Owner> GetOwnerAsync(int id);
        Task<bool> ValidateOwner(int id);
        IEnumerable<Owner> GetOwners();
        Task<Owner> SaveOwnerAsync(Owner owner);
    }
}
