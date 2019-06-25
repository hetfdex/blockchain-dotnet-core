using blockchain_dotnet_core.API.Utils;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.API
{
    public static class Constants
    {
        public const int InitialDifficulty = 2;

        public const int MiningRate = 10;

        public const decimal StartBalance = 1000;

        public static ECPublicKeyParameters MinerAddress = CryptoUtils.LoadPublicKey(
            "MIIBMzCB7AYHKoZIzj0CATCB4AIBATAsBgcqhkjOPQEBAiEA/////////////////////////////////////v///C8wRAQgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHBEEEeb5mfvncu6xVoGKVzocLBwKb/NstzijZWfKBWxb4F5hIOtp3JqPEZV2k+/wOEQio/Re0SKaFVBmcR9CP+xDUuAIhAP////////////////////66rtzmr0igO7/SXozQNkFBAgEBA0IABG+Iu+O3FJGQhZHBUVn+4/EEw41r13myLyTRqZfeklWN/VIiUjE5WC574vIV9tYErJf/tE2h51rH/5KB246NRfg=");

        public const decimal MinerReward = 50;
    }
}