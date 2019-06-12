using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Models
{
    public class Blockchain
    {
        public List<Block> Chain { get; set; } = new List<Block>
        {
            //BlockUtils.GetGenesisBlock()
        };
    }
}