using System.Diagnostics.CodeAnalysis;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.API.Utils
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        public const int InitialDifficulty = 1;

        public const int MiningRate = 10;

        public const decimal StartBalance = 1000;

        public const ECPublicKeyParameters MinerAddress = null;

        public const decimal MinerReward = 50;


    }
}