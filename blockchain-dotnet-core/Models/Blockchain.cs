using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Models
{
    public class Blockchain
    {
        public IEnumerable<Block> Blocks { get; set; }
    }
}
