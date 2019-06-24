using blockchain_dotnet_core.API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blockchain_dotnet_core.API.Models
{
    public class Block
    {
        public int Index { get; set; }
        public long Timestamp { get; set; }

        public string LastHash { get; set; }

        public string Hash { get; set; }

        public IList<Transaction> Transactions { get; set; }

        public int Nonce { get; set; }

        public int Difficulty { get; set; }

        public Block(int index, long timestamp, string lastHash, IList<Transaction> transactions, int nonce,
            int difficulty)
        {
            Index = index;
            Timestamp = timestamp;
            LastHash = lastHash ?? throw new ArgumentNullException(nameof(lastHash));
            Transactions = transactions ?? throw new ArgumentNullException(nameof(transactions));
            Nonce = nonce;
            Difficulty = difficulty;

            Hash = HashUtils.ComputeHash(this).ToBase64();
        }

        public Block(int index, long timestamp, string lastHash, string hash, IList<Transaction> transactions,
            int nonce,
            int difficulty)
        {
            Index = index;
            Timestamp = timestamp;
            LastHash = lastHash ?? throw new ArgumentNullException(nameof(lastHash));
            Hash = hash ?? throw new ArgumentNullException(nameof(hash));
            Transactions = transactions ?? throw new ArgumentNullException(nameof(transactions));
            Nonce = nonce;
            Difficulty = difficulty;
        }

        public static Block MineBlock(Block lastBlock, IList<Transaction> transactions)
        {
            if (lastBlock == null)
            {
                throw new ArgumentNullException(nameof(lastBlock));
            }

            if (transactions == null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            var index = lastBlock.Index + 1;

            var lastHash = lastBlock.Hash;

            var nonce = 0;

            long timestamp;

            string hash;

            int difficulty;

            do
            {
                nonce++;

                timestamp = TimestampUtils.GenerateTimestamp();

                difficulty = AdjustDifficulty(lastBlock, timestamp);

                hash = HashUtils.ComputeHash(index, timestamp, lastHash, transactions, nonce,
                    difficulty).ToBase64();
            } while (hash.Substring(0, difficulty) != new string('0', difficulty));

            return new Block(index, timestamp, lastHash, hash, transactions, nonce, difficulty);
        }

        public static int AdjustDifficulty(Block lastBlock, long timestamp)
        {
            if (lastBlock == null)
            {
                throw new ArgumentNullException(nameof(lastBlock));
            }

            var difficulty = lastBlock.Difficulty;

            if (difficulty < 1)
            {
                return 1;
            }

            if (timestamp - lastBlock.Timestamp > Constants.MiningRate)
            {
                return difficulty - 1;
            }

            return difficulty + 1;
        }

        public static Block GetGenesisBlock()
        {
            return new Block(0, 0, HashUtils.ComputeHash("genesis-lastHash").ToBase64(),
                new List<Transaction>(), 0,
                Constants.InitialDifficulty);
        }

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
            return Index.Equals(other.Index) && Timestamp.Equals(other.Timestamp) &&
                   string.Equals(LastHash, other.LastHash) &&
                   string.Equals(Hash, other.Hash) && Transactions.SequenceEqual(other.Transactions) &&
                   Nonce.Equals(other.Nonce) && Difficulty.Equals(other.Difficulty);
        }
    }
}