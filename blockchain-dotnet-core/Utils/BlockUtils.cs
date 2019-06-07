﻿using blockchain_dotnet_core.API.Models;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Utils
{
    public static class BlockUtils
    {
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

                hash = HashUtils.ComputeSHA256(timestamp, lastHash, transactions, nonce, difficulty);
            } while (hash.Substring(0, difficulty) != GetLeadingZeros(difficulty));

            return new Block
            {
                Timestamp = timestamp,
                LastHash = lastHash,
                Hash = hash,
                Transactions = transactions,
                Nonce = nonce,
                Difficulty = difficulty
            };
        }

        public static Block GetGenesisBlock()
        {
            var genesisBlock = Constants.GenesisBlock;

            genesisBlock.Hash = HashUtils.ComputeSHA256(genesisBlock);

            return genesisBlock;
        }

        public static int AdjustDifficulty(Block lastBlock, long timestamp)
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

        private static string GetLeadingZeros(int difficulty) => new string('0', difficulty);
    }
}