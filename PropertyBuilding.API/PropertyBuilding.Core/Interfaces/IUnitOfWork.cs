using System.Threading.Tasks;

namespace PropertyBuilding.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        void Dispose();
        Task SaveChangesAsync();
    }
}