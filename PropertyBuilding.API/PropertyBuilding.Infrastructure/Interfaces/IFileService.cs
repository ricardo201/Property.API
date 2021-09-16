using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PropertyBuilding.Infrastructure.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(IFormFile file, int idUser, int propertyId);
        string GetFilePath(string fileName, int idUser, int propertyId);
    }
}
