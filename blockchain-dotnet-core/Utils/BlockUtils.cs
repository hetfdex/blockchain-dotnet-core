/*using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Options;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Utils
{
    public static class BlockUtils
    {
        public static Block GetGenesisBlock()
        {
            return new Block(0, HexUtils.BytesToString(HashUtils.ComputeHash("genesis-lastHash")), new List<Transaction>(), 0,
                ConfigurationOptions.InitialDifficulty);
        }

        public static Block MineBlock(Block lastBlock, List<Transaction> transactions)
        {
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

                hash = HexUtils.BytesToString(HashUtils.ComputeHash(timestamp, lastHash, transactions, nonce, difficulty));
            } while (hash.Substring(0, difficulty) != new string('0', difficulty));

            return new Block(timestamp, lastHash, hash, transactions, nonce, difficulty);
        }

        public static int AdjustDifficulty(Block lastBlock, long timestamp)
        {
            var difficulty = lastBlock.Difficulty;

            if (difficulty < 1)
            {
                return 1;
            }

            if (timestamp - lastBlock.Timestamp > ConfigurationOptions.MiningRate)
            {
                return difficulty - 1;
            }

            return difficulty + 1;
        }
    }
}*/