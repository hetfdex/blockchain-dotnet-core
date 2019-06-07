using blockchain_dotnet_core.API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blockchain_dotnet_core.API.Models
{
    public class Block
    {
        public long Timestamp { get; set; }

        public string LastHash { get; set; }

        public string Hash { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public int Nonce { get; set; }

        public int Difficulty { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Hash) ? HashUtils.ComputeSHA256(this) : Hash;
        }

        public override int GetHashCode() => HashCode.Combine(Timestamp, LastHash, Hash, Transactions, Nonce, Difficulty);

        public override bool Equals(object obj)
        {
            var block = obj as Block;

            if (block == null)
            {
                return false;
            }

            return Equals(block);
        }

        public bool Equals(Block other)
        {
            return Timestamp.Equals(other.Timestamp) && string.Equals(LastHash, other.LastHash) &&
                   string.Equals(Hash, other.Hash) && Transactions.SequenceEqual(other.Transactions) &&
                   Nonce.Equals(other.Nonce) && Difficulty.Equals(other.Difficulty);
        }
    }
}