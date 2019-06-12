/*using blockchain_dotnet_core.API.Extensions;
using blockchain_dotnet_core.API.Models;

namespace blockchain_dotnet_core.API.Utils
{
    public static class BlockchainUtils
    {
        public static void ReplaceChain(ref Blockchain blockchain, Blockchain otherBlockchain, bool validateTransactionData)
        {
            if (otherBlockchain.Chain.Count <= blockchain.Chain.Count)
            {
                return;
            }

            if (!otherBlockchain.IsValidChain())
            {
                return;
            }

            if (validateTransactionData && !otherBlockchain.AreValidTransactions())
            {
                return;
            }

            blockchain = otherBlockchain;
        }
    }
}*/