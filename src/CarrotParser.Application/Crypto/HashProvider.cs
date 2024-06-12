using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace CarrotParser.Application.Crypto
{
    internal class HashProvider : IHashProvider
    {
        public string Hash(string data, string salt, HashAlgorithmType hashAlgorithmType)
        {
            return hashAlgorithmType switch
            {
                HashAlgorithmType.Sha1 => GetSha1Hash(data, salt),
                HashAlgorithmType.Sha256 => GetSha256Hash(data, salt),
                HashAlgorithmType.Md5 => GetMd5Hash(data, salt),
                _ => throw new InvalidOperationException($"Hash algorithm {hashAlgorithmType} is unsupported.")
            };
        }

        private static string GetSha1Hash(string data, string salt)
        {
            var hash = SHA1.HashData(Encoding.UTF8.GetBytes(data + salt));
            return Encoding.UTF8.GetString(hash);
        }

        private static string GetSha256Hash(string data, string salt)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(data + salt));
            return Encoding.UTF8.GetString(hash);
        }

        private static string GetMd5Hash(string data, string salt)
        {
            var hash = MD5.HashData(Encoding.UTF8.GetBytes(data + salt));
            return Encoding.UTF8.GetString(hash);
        }
    }
}
