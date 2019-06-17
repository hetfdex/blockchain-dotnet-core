using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Options;

namespace blockchain_dotnet_core.API.Utils
{
    public static class TransactionInputUtils
    {
        public static TransactionInput GetMinerTransactionInput()
        {
            return new TransactionInput(TimestampUtils.GenerateTimestamp(), ConfigurationOptions.MinerAddress, ConfigurationOptions.MinerReward, "miner-reward");

        }
    }
}