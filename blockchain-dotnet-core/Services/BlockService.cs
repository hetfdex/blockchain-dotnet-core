using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;

namespace blockchain_dotnet_core.API.Services
{
    public class BlockService : IBlockService
    {
        public Block CreateBlock(string hash, string lastHash, string data)
        {
            var block = new Block
            {
                LastHash = lastHash,
                Data = data
            };

            block.Hash = SHA256Util.ComputeSHA256(block);

            return block;
        }
    }
}
