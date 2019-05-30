using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Services
{
    public class BlockchainService : IBlockchainService
    {
        private readonly IBlockService _blockService;

        public List<Block> Blockchain { get; set; }

        public BlockchainService(IBlockService blockService)
        {
            _blockService = blockService;

            Blockchain = new List<Block>
            {
                _blockService.GetGenesisBlock()
            };
        }

        public void AddBlock(List<Transaction> transactions)
        {
            var lastBlock = Blockchain[Blockchain.Count - 1];

            var block = _blockService.MineBlock(lastBlock, transactions);

            Blockchain.Add(block);
        }

        public void ReplaceChain(List<Block> otherBlockchain, bool validateTransactionData)
        {
            if (otherBlockchain.Count <= Blockchain.Count)
            {
                return;
            }

            if (!IsValidChain(otherBlockchain))
            {
                return;
            }

            if (validateTransactionData && !IsValidTransactionData(otherBlockchain))
            {
                return;
            }

            Blockchain = otherBlockchain;
        }

        public bool IsValidChain(List<Block> blockchain)
        {
            var genesisBlock = _blockService.GetGenesisBlock();

            if (!blockchain[0].Equals(genesisBlock))
            {
                return false;
            }

            for (int i = 1; i < blockchain.Count; i++)
            {
                var block = blockchain[i];

                var realLastHash = blockchain[i - 1].Hash;

                var lastDifficulty = blockchain[i - 1].Difficulty;

                if (block.LastHash != realLastHash)
                {
                    return false;
                }

                var validHash = SHA256Util.ComputeSHA256(block.Timestamp, block.LastHash, block.Transactions, block.Nonce,
                    block.Difficulty);

                if (block.Hash != validHash)
                {
                    return false;
                }

                if (Math.Abs(lastDifficulty - block.Difficulty) > 1)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsValidTransactionData(List<Block> blockchain)
        {
            throw new System.NotImplementedException();
        }
    }
}