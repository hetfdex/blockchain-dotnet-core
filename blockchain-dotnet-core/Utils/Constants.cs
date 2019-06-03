using blockchain_dotnet_core.API.Models;
using System.Diagnostics.CodeAnalysis;

namespace blockchain_dotnet_core.API.Utils
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        public const int InitialDifficulty = 2;

        public const int MineRate = 2;

        public const decimal StartBalance = 1000;

        public const decimal MinerRewardAmount = 50;

        public static readonly TransactionInput MinerTransactionInput = new TransactionInput
        {
            Timestamp = TimestampUtils.GenerateTimestamp(),
            Address = null,
            Amount = -1,
            Signature = "miner-reward"
        };
    }
}