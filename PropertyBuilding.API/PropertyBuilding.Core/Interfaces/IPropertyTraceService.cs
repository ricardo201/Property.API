using PropertyBuilding.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropertyBuilding.Core.Interfaces
{
    public interface IPropertyTraceService
    {
        Task<bool> DeletePropertyTraceAsync(int id);
        Task<PropertyTrace> GetPropertyTraceAsync(int id);
        IEnumerable<PropertyTrace> GetPropertyTraces();
        Task<PropertyTrace> SavePropertyTraceAsync(PropertyTrace PropertyTrace);
        Task<PropertyTrace> UpdatePropertyTraceAsyn(PropertyTrace propertyTrace);
    }
}
