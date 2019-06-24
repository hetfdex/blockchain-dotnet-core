using blockchain_dotnet_core.API.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace blockchain_dotnet_core.API.Utils
{
    public static class HashUtils
    {
        public static byte[] ComputeHash(Block block)
        {
            if (block == null)
            {
                throw new ArgumentNullException(nameof(block));
            }

            return ComputeHash(block.Index, block.Timestamp, block.LastHash, block.Transactions, block.Nonce,
                block.Difficulty);
        }

        public static byte[] ComputeHash(int index, long timestamp, string lastHash, IList<Transaction> transactions,
            int nonce,
            int difficulty)
        {
            if (string.IsNullOrEmpty(lastHash))
            {
                throw new ArgumentNullException(nameof(lastHash));
            }

            if (transactions == null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            return ComputeHash(index + timestamp + lastHash + transactions.ToHashableString()+ nonce + difficulty);
        }

        public static byte[] ComputeHash(string toHash)
        {
            if (string.IsNullOrEmpty(toHash))
            {
                throw new ArgumentNullException(nameof(toHash));
            }

            return ComputeHash(Encoding.ASCII.GetBytes(toHash));
        }

        public static byte[] ComputeHash(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(bytes);
            }
        }
    }
}