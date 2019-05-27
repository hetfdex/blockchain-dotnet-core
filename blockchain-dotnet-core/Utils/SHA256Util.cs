using System.Security.Cryptography;
using System.Text;
using blockchain_dotnet_core.API.Models;

namespace blockchain_dotnet_core.API.Utils
{
    public static class SHA256Util
    {
        public static string ComputeSHA256(Block block)
        {
            var toHash = block.LastHash + block.TimeStamp + block.Data;

            return ComputeSHA256(toHash);
        }

        public static string ComputeSHA256(string toHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(toHash));

                return Encoding.UTF8.GetString(hash);
            }
        }
    }
}
