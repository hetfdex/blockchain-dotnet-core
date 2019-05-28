using blockchain_dotnet_core.API.Models;

namespace blockchain_dotnet_core.API.Services
{
    public interface IBlockService
    {
        Block MineBlock(Block lastBlock, string data);

        Block GetGenesisBlock();

        int AdjustDifficulty(Block lastBlock, long timestamp);
    }
}