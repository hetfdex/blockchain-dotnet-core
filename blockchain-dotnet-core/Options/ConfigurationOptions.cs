using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.API.Options
{
    public static class ConfigurationOptions
    {
        public static int InitialDifficulty = 1;

        public static int MiningRate = 10;

        public static decimal StartBalance = 1000;

        public static ECPublicKeyParameters MinerAddress = null;

        public static decimal MinerReward = 50;
    }
}