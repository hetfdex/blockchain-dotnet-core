using blockchain_dotnet_core.API.Models;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Services
{
    public interface IBlockService
    {
        Block MineBlock(Block lastBlock, List<Transaction> transactions);

        Block GetGenesisBlock();

        int AdjustDifficulty(Block lastBlock, long timestamp);
    }
}