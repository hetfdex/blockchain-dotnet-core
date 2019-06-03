using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blockchain_dotnet_core.API.Extensions
{
    public static class BlockchainExtensions
    {
        public static void AddBlock(this List<Block> blockchain, List<Transaction> transactions)
        {
            var lastBlock = blockchain[blockchain.Count - 1];

            var block = BlockExtensions.MineBlock(lastBlock, transactions);

            blockchain.Add(block);
        }

        public static void ReplaceChain(ref List<Block> blockchain, List<Block> otherBlockchain, bool validateTransactionData)
        {
            if (otherBlockchain.Count <= blockchain.Count)
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

            blockchain = otherBlockchain;
        }

        public static bool IsValidChain(this List<Block> blockchain)
        {
            var genesisBlock = BlockExtensions.GetGenesisBlock();

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

                var validHash = HashUtils.ComputeSHA256(block.Timestamp, block.LastHash, block.Transactions, block.Nonce,
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

        public static bool IsValidTransactionData(this List<Block> blockchain)
        {
            for (int i = 0; i < blockchain.Count; i++)
            {
                var block = blockchain[i];

                var minerRewardCount = 0;

                foreach (var transaction in block.Transactions)
                {
                    if (transaction.TransactionInput.Signature == Constants.MinerTransactionInput.Signature)
                    {
                        minerRewardCount++;

                        if (minerRewardCount > 1)
                        {
                            return false;
                        }

                        var firstTransactionOutput = transaction.TransactionOutputs.First();

                        if (firstTransactionOutput.Value != Constants.MinerRewardAmount)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!TransactionExtensions.IsValidTransaction(transaction))
                        {
                            return false;
                        }

                        var actualWalletBalance =
                            WalletExtensions.CalculateBalance(blockchain, transaction.TransactionInput.Address);

                        if (transaction.TransactionInput.Amount != actualWalletBalance)
                        {
                            return false;
                        }
                    }
                }

                if (block.Transactions.Count != block.Transactions.Distinct().Count())
                {
                    return false;
                }
            }

            return true;
        }
    }
}