using blockchain_dotnet_core.API.Models;

namespace blockchain_dotnet_core.API.Utils
{
    public static class TransactionInputUtils
    {
        public static TransactionInput GetMinerTransactionInput()
        {
            return new TransactionInput
            {
                Timestamp = TimestampUtils.GenerateTimestamp(),
                Address = ConfigurationOptions.MinerAddress,
                Amount = ConfigurationOptions.MinerReward,
                Signature = "miner-reward"
            };
        }
    }
}
