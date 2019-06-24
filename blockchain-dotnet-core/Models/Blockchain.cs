using blockchain_dotnet_core.API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blockchain_dotnet_core.API.Models
{
    public class Blockchain
    {
        public IList<Block> Chain { get; set; }

        public Blockchain()
        {
            Chain = new List<Block>
            {
                Block.GetGenesisBlock()
            };
        }

        public void ReplaceChain(Blockchain otherBlockchain,
            bool validateTransactionData)
        {
            if (otherBlockchain.Chain.Count <= Chain.Count)
            {
                return;
            }

            if (!otherBlockchain.IsValidChain())
            {
                return;
            }

            if (validateTransactionData && !otherBlockchain.AreValidTransactions())
            {
                return;
            }

            Chain = otherBlockchain.Chain;
        }

        public void AddBlock(IList<Transaction> transactions)
        {
            if (transactions == null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            var lastBlock = Chain[Chain.Count - 1];

            if (lastBlock == null)
            {
                throw new ArgumentNullException(nameof(lastBlock));
            }

            var block = Block.MineBlock(lastBlock, transactions);

            Chain.Add(block);
        }

        public bool IsValidChain()
        {
            var genesisBlock = Block.GetGenesisBlock();

            if (!Chain[0].Equals(genesisBlock))
            {
                return false;
            }

            for (int i = 1; i < Chain.Count; i++)
            {
                var block = Chain[i];

                var realIndex = Chain[i - 1].Index + 1;

                var realLastHash = Chain[i - 1].Hash;

                var lastDifficulty = Chain[i - 1].Difficulty;

                if (block.Index != realIndex)
                {
                    return false;
                }

                if (block.LastHash != realLastHash)
                {
                    return false;
                }

                var validHash = HashUtils.ComputeHash(block).ToBase64();

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

        public bool AreValidTransactions()
        {
            foreach (var block in Chain)
            {
                var minerRewardCount = 0;

                foreach (var transaction in block.Transactions)
                {
                    if (transaction.TransactionInput.Equals(TransactionInput.GetMinerTransactionInput()))
                    {
                        minerRewardCount++;

                        if (minerRewardCount > 1)
                        {
                            return false;
                        }

                        var firstTransactionOutput = transaction.TransactionOutputs.FirstOrDefault();

                        if (firstTransactionOutput.Value != Constants.MinerReward)
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
                            Wallet.CalculateBalance(this, transaction.TransactionInput.Address);

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

        public override bool Equals(object obj)
        {
            var blockchain = obj as Blockchain;

            if (blockchain == null)
            {
                return false;
            }

            return Equals(blockchain);
        }

        public bool Equals(Blockchain other)
        {
            return Chain.SequenceEqual(other.Chain);
        }
    }
}