using PropertyBuilding.Core.Entities;
using System.Threading.Tasks;

namespace PropertyBuilding.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<int> CountByUserNameAsync(string userName);
    }
}