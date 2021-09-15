using PropertyBuilding.Core.Entities;
using System.Threading.Tasks;

namespace PropertyBuilding.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRepository<Owner> OwnerRepository { get; }
        IPropertyRepositoy PropertyRepository { get; }
        IPropertyImageRepository PropertyImageRepository { get; }
        IRepository<PropertyTrace> PropertyTraceRepository { get; }
        void Dispose();
        Task SaveChangesAsync();
        void ChangeTrackerClear();
    }
}