using System.Threading.Tasks;
using blockchain_dotnet_core.API.Models;

namespace blockchain_dotnet_core.API.Services
{
    public interface IBlockService
    {
        Task<Block> CreateBlock(string hash, string lastHash, string data);
    }
}
