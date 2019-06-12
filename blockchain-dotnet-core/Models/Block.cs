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

        public List<Transaction> Transactions { get; set; }

        public int Nonce { get; set; }

        public int Difficulty { get; set; }

        public Block(long timestamp, string lastHash, List<Transaction> transactions, int nonce, int difficulty)
        {
            Timestamp = timestamp;
            LastHash = lastHash;
            Transactions = transactions;
            Nonce = nonce;
            Difficulty = difficulty;

            Hash = HexUtils.BytesToString(HashUtils.ComputeHash(this));
        }

        public Block(long timestamp, string lastHash, string hash, List<Transaction> transactions, int nonce, int difficulty)
        {
            Timestamp = timestamp;
            LastHash = lastHash;
            Hash = hash;
            Transactions = transactions;
            Nonce = nonce;
            Difficulty = difficulty;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Hash) ? HexUtils.BytesToString(HashUtils.ComputeHash(this)) : Hash;
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