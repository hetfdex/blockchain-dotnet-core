using blockchain_dotnet_core.API.Utils;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Models
{
    public class Block
    {
        public long Timestamp { get; set; }

        public string LastHash { get; set; }

        public string Hash { get; set; }

        public List<Transaction> Transactions { get; set; }

        public int Nonce { get; set; }

        public int Difficulty { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Hash) ? HashUtils.ComputeSHA256(this) : Hash;
        }

        public override int GetHashCode() => HashCode.Combine(Timestamp, LastHash, Hash, Transactions, Nonce, Difficulty);

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Block))
            {
                return false;
            }

            return Equals((Block)obj);
        }

        public bool Equals(Block other)
        {
            return Timestamp == other.Timestamp && string.Equals(LastHash, other.LastHash) &&
                   string.Equals(Hash, other.Hash) /*&& Equals(Transactions, other.Transactions) */&& Nonce == other.Nonce &&
                   Difficulty == other.Difficulty;
        }
    }
}