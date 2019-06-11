using blockchain_dotnet_core.API.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace blockchain_dotnet_core.API.Utils
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        public const int InitialDifficulty = 5;

        public const int MineRate = 10;

        public const decimal StartBalance = 1000;

        public const decimal MinerRewardAmount = 50;

        public static readonly TransactionInput MinerTransactionInput = new TransactionInput
        {
            Timestamp = TimestampUtils.GenerateTimestamp(),
            Address = null,
            Amount = 50,
            Signature = "miner-reward"
        };

        public static readonly Block GenesisBlock = new Block
        {
            Timestamp = 0L,
            LastHash = HashUtils.ComputeSHA256("genesis-lastHash"),
            Transactions = new List<Transaction>(),
            Nonce = 0,
            Difficulty = InitialDifficulty
        };
    }
}