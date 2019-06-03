using blockchain_dotnet_core.API.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace blockchain_dotnet_core.API.Utils
{
    public static class HashUtils
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

                return HexUtils.BytesToString(hash);
            }
        }
    }
}