using PropertyBuilding.Core.Entities;
using System.Threading.Tasks;

namespace PropertyBuilding.Core.Interfaces
{
    public interface IUserService
    {
        string GenerateToken(User user);
        void GetUserByToken(string token);
        Task<bool> Register(User user);
        Task<User> GetLoginByCredentials(User user);
        Task<(User, bool)> IsValid(User user);
        Task<string> Authentication(User user);
        Task<bool> NotExistUserName(string userName);
    }
}
