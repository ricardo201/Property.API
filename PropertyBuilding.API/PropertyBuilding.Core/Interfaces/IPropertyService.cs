using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.QueryFilters;
using System.Threading.Tasks;

namespace PropertyBuilding.Core.Interfaces
{
    public interface IPropertyService
    {
        Task<Property> GetPropertyByIdAsync(int id);
        PagedListDto<Property> GetProperties(PropertyQueryFilter propertyQueryFilter);
        Task<Property> SavePropertyAsync(Property property);
        Task<Property> ChangePricePropertyAsync(Property changePriceProperty);
        Task<Property> UpdatePropertyAsync(Property property);
        Task<bool> ValidatePropertyAsync(int id);
    }
}
