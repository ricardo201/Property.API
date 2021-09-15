using PropertyBuilding.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropertyBuilding.Core.Interfaces
{
    public interface IPropertyImageService
    {
        Task<bool> DeletePropertyImageAsync(int id);
        IEnumerable<PropertyImage> GetPropertyImages();
        Task<PropertyImage> SavePropertyImageAsync(PropertyImage PropertyImage);
        Task<PropertyImage> GetPropertyImageByIdAsync(int id, int idUser);
        PropertyImage GetPropertyImageByFileName(string fileName, int idUser);
        Task<PropertyImage> ChangeEnabledPropertyImageAsyn(PropertyImage changeStatePropertyImage, int idUser);
    }
}
