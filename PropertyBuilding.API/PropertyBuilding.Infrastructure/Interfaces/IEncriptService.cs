namespace PropertyBuilding.Infrastructure.Interfaces
{
    public interface IEncriptService
    {
        string GetSHA256(string password);
    }
}