using System.Diagnostics.CodeAnalysis;

namespace blockchain_dotnet_core.API.Utils
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        public const int InitialDifficulty = 2;

        public const int MineRate = 2;

        public const decimal StartBalance = 1000;

        public const decimal MinerRewardAmmount = 50;
    }
}