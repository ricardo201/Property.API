using PropertyBuilding.Core.Entities;

namespace PropertyBuilding.Core.Interfaces
{
    public interface IPropertyImageRepository : IRepository<PropertyImage>
    {
        PropertyImage GetPropertyImageByName(string fileName, int idUser);
    }
}
