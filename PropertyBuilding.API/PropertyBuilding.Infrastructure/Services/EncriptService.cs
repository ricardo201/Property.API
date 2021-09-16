using PropertyBuilding.Infrastructure.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PropertyBuilding.Infrastructure.Services
{
    public class EncriptService : IEncriptService
    {
        public string GetSHA256(string password)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder stringBuilder = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(password));
            for (int index = 0; index < stream.Length; index++) stringBuilder.AppendFormat("{0:x2}", stream[index]);
            return stringBuilder.ToString();
        }
    }
}
