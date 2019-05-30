using System.Collections.Generic;
using blockchain_dotnet_core.API.Models;
using System.Security.Cryptography;
using System.Text;

namespace blockchain_dotnet_core.API.Utils
{
    public static class SHA256Util
    {
        public static string ComputeSHA256(Block block)
        {
            var toHash = block.Timestamp + block.LastHash + block.Transactions + block.Nonce + block.Difficulty;

            return ComputeSHA256(toHash);
        }

        public static string ComputeSHA256(long timestamp, string lastHash, List<Transaction> transactions, int nonce, int difficulty)
        {
            var toHash = timestamp + lastHash + transactions + nonce + difficulty;

            return ComputeSHA256(toHash);
        }

        public static string ComputeSHA256(string toHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.Default.GetBytes(toHash));

                return ToHex(hash);
            }
        }

        private static string ToHex(byte[] bytes)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            foreach (var b in bytes)
            {
                result.Append(b.ToString("x2"));
            }

            return result.ToString();
        }
    }
}