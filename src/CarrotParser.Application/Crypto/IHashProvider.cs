using System.Security.Authentication;

namespace CarrotParser.Application.Crypto
{
    public interface IHashProvider
    {
        string Hash(string data, string salt, HashAlgorithmType hashAlgorithmType);
    }
}