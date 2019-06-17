using blockchain_dotnet_core.API.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace blockchain_dotnet_core.API.Utils
{
    public static class HashUtils
    {
        public static byte[] ComputeHash(Block block)
        {
            return ComputeHash(block.Timestamp, block.LastHash, block.Transactions, block.Nonce, block.Difficulty);
        }

        public static byte[] ComputeHash(long timestamp, string lastHash, List<Transaction> transactions, int nonce,
            int difficulty)
        {
            return ComputeHash(timestamp + lastHash + transactions + nonce + difficulty);
        }

        public static byte[] ComputeHash(string toHash)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.Default.GetBytes(toHash));
            }
        }
    }
}