using blockchain_dotnet_core.API.Models;

namespace blockchain_dotnet_core.API.Services
{
    public interface IBlockService
    {
        Block CreateBlock(string hash, string lastHash, string data);
    }
}
