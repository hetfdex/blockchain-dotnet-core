using blockchain_dotnet_core.API.Models;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Services
{
    public interface IBlockchainService
    {
        List<Block> Blockchain { get; set; }

        void AddBlock(List<Transaction> transactions);

        void ReplaceChain(List<Block> otherBlockchain, bool validateTransactionData);

        bool IsValidChain(List<Block> blockchain);

        bool IsValidTransactionData(List<Block> blockchain);
    }
}