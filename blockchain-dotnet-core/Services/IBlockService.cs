using System.Collections.Generic;
using blockchain_dotnet_core.API.Models;

namespace blockchain_dotnet_core.API.Services
{
    public interface IBlockService
    {
        Block MineBlock(Block lastBlock, List<Transaction> transactions);

        Block GetGenesisBlock();

        int AdjustDifficulty(Block lastBlock, long timestamp);
    }
}