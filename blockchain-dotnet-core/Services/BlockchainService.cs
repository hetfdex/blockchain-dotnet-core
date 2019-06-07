﻿using System.Collections.Generic;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;

namespace blockchain_dotnet_core.API.Services
{
    public class BlockchainService : IBlockchainService
    {
        public List<Block> Blockchain { get; set; } = new List<Block>();

        public BlockchainService()
        {
            Blockchain.Add(BlockUtils.GetGenesisBlock());
        }
    }
}