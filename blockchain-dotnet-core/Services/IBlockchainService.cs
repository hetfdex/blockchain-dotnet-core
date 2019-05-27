using System.Collections.Generic;
using System.Threading.Tasks;
using blockchain_dotnet_core.API.Models;

namespace blockchain_dotnet_core.API.Services
{
    public interface IBlockchainService
    {
        Task<Blockchain> CreateBlockchain(IEnumerable<Block> blocks);
    }
}
