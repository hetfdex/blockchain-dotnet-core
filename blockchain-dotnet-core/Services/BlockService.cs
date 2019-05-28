using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using System;

namespace blockchain_dotnet_core.API.Services
{
    public class BlockService : IBlockService
    {
        public Block MineBlock(Block lastBlock, string data)
        {
            long timestamp;

            var lastHash = lastBlock.Hash;

            string hash;

            var nonce = 0;

            int difficulty;

            do
            {
                nonce++;

                timestamp = GetTimestamp();

                difficulty = AdjustDifficulty(lastBlock, timestamp);

                hash = SHA256Util.ComputeSHA256(timestamp, lastHash, data, nonce, difficulty);
            } while (hash.Substring(0, difficulty) != GetLeadingZeros(difficulty));

            return new Block
            {
                Timestamp = timestamp,
                LastHash = lastHash,
                Hash = hash,
                Data = data,
                Nonce = nonce,
                Difficulty = difficulty
            };
        }

        public Block GetGenesisBlock()
        {
            var genesisBlock = new Block
            {
                Timestamp = GetTimestamp(),
                LastHash = SHA256Util.ComputeSHA256("genesis-lasthash"),
                Data = "genesis-data",
                Nonce = 0,
                Difficulty = Constants.InitialDifficulty
            };

            genesisBlock.Hash = SHA256Util.ComputeSHA256(genesisBlock);

            return genesisBlock;
        }

        public int AdjustDifficulty(Block lastBlock, long timestamp)
        {
            var difficulty = lastBlock.Difficulty;

            if (difficulty < 1)
            {
                return 1;
            }

            if (timestamp - lastBlock.Timestamp > Constants.MineRate)
            {
                return difficulty - 1;
            }

            return difficulty + 1;
        }

        private long GetTimestamp() => (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        private string GetLeadingZeros(int difficulty) => new string('0', difficulty);
    }
}