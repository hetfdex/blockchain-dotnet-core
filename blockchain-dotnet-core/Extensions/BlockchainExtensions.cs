using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Options;
using blockchain_dotnet_core.API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blockchain_dotnet_core.API.Extensions
{
    public static class BlockchainExtensions
    {
        public static void AddBlock(this Blockchain blockchain, List<Transaction> transactions)
        {
            var lastBlock = blockchain.Chain[blockchain.Chain.Count - 1];

            var block = BlockUtils.MineBlock(lastBlock, transactions);

            blockchain.Chain.Add(block);
        }

        public static bool IsValidChain(this Blockchain blockchain)
        {
            var genesisBlock = BlockUtils.GetGenesisBlock();

            if (!blockchain.Chain[0].Equals(genesisBlock))
            {
                return false;
            }

            for (int i = 1; i < blockchain.Chain.Count; i++)
            {
                var block = blockchain.Chain[i];

                var realLastHash = blockchain.Chain[i - 1].Hash;

                var lastDifficulty = blockchain.Chain[i - 1].Difficulty;

                if (block.LastHash != realLastHash)
                {
                    return false;
                }

                var validHash = HexUtils.BytesToString(HashUtils.ComputeHash(block));

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

        public static bool AreValidTransactions(this Blockchain blockchain)
        {
            foreach (var block in blockchain.Chain)
            {
                var minerRewardCount = 0;

                foreach (var transaction in block.Transactions)
                {
                    if (transaction.TransactionInput.Signature ==
                        TransactionInputUtils.GetMinerTransactionInput().Signature)
                    {
                        minerRewardCount++;

                        if (minerRewardCount > 1)
                        {
                            return false;
                        }

                        var firstTransactionOutput = transaction.TransactionOutputs.First();

                        if (firstTransactionOutput.Value != ConfigurationOptions.MinerReward)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!transaction.IsValidTransaction())
                        {
                            return false;
                        }

                        var actualWalletBalance =
                            WalletUtils.CalculateBalance(blockchain, transaction.TransactionInput.Address);

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