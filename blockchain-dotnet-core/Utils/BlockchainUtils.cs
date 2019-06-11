using blockchain_dotnet_core.API.Extensions;
using blockchain_dotnet_core.API.Models;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Utils
{
    public static class BlockchainUtils
    {
        public static void ReplaceChain(ref List<Block> blockchain, List<Block> otherBlockchain, bool validateTransactionData)
        {
            if (otherBlockchain.Count <= blockchain.Count)
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
}