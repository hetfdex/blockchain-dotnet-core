using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class BlockchainTests
    {
        private readonly Blockchain _blockchain = new Blockchain();

        private readonly Blockchain _replacementBlockchain = new Blockchain();

        [TestInitialize]
        public void BlockchainUtilsTestsSetup()
        {
            var senderWallet = new Wallet();

            var recipientWallet = new Wallet();

            var transactions = new List<Transaction>
            {
                senderWallet.GenerateTransaction(recipientWallet.PublicKey, 100, _blockchain)
            };

            _blockchain.AddBlock(transactions);

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            transactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 200, _blockchain)
            };

            _blockchain.AddBlock(transactions);
        }

        [TestMethod]
        public void ConstructBlockchain()
        {
            Assert.IsNotNull(_blockchain);
            Assert.IsNotNull(_blockchain.Chain);
            Assert.IsInstanceOfType(_blockchain.Chain, typeof(List<Block>));
            Assert.AreEqual(Block.GetGenesisBlock(), _blockchain.Chain[0]);
        }

        [TestMethod]
        public void AddBlock()
        {
            var transactions = new List<Transaction>();

            _blockchain.AddBlock(transactions);

            Assert.IsNotNull(_blockchain.Chain[_blockchain.Chain.Count - 1]);
            Assert.AreEqual(transactions, _blockchain.Chain[_blockchain.Chain.Count - 1].Transactions);
        }

        [TestMethod]
        public void AddBlockNullTransactionThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _blockchain.AddBlock(null));
        }

        [TestMethod]
        public void AddBlockNullLastBlockThrowsException()
        {
            var transactions = new List<Transaction>();

            _blockchain.Chain[_blockchain.Chain.Count - 1] = null;

            Assert.ThrowsException<ArgumentNullException>(() => _blockchain.AddBlock(transactions));
        }

        [TestMethod]
        public void ReplaceChainLongerValidBlockchain()
        {
            var senderWallet = new Wallet();

            var recipientWallet = new Wallet();

            var firstTransactions = new List<Transaction>
            {
                senderWallet.GenerateTransaction(recipientWallet.PublicKey, 100, _replacementBlockchain)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var secondTransactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 200, _replacementBlockchain)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var thirdTransactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 50, _replacementBlockchain)
            };

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);
            _replacementBlockchain.AddBlock(thirdTransactions);

            _blockchain.ReplaceChain(_replacementBlockchain, false);

            Assert.AreEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void ReplaceChainLongerInvalidBlockchain()
        {
            var senderWallet = new Wallet();

            var recipientWallet = new Wallet();

            var firstTransactions = new List<Transaction>
            {
                senderWallet.GenerateTransaction(recipientWallet.PublicKey, 100, _replacementBlockchain)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var secondTransactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 200, _replacementBlockchain)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var thirdTransactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 50, _replacementBlockchain)
            };

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);
            _replacementBlockchain.AddBlock(thirdTransactions);

            _replacementBlockchain.Chain[_replacementBlockchain.Chain.Count - 1].LastHash = "fake-lastHash";

            _blockchain.ReplaceChain(_replacementBlockchain, false);

            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void ReplaceChainShorterBlockchain()
        {
            _blockchain.ReplaceChain(_replacementBlockchain, false);

            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void ReplaceChainNullOtherBlockchainThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _blockchain.ReplaceChain(null, false));
        }

        [TestMethod]
        public void ReplaceChainValidTransactions()
        {
            var senderWallet = new Wallet();

            var recipientWallet = new Wallet();

            var firstTransactions = new List<Transaction>
            {
                senderWallet.GenerateTransaction(recipientWallet.PublicKey, 100, _replacementBlockchain),
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var secondTransactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 200, _replacementBlockchain),
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var thirdTransactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 50, _replacementBlockchain),
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);
            _replacementBlockchain.AddBlock(thirdTransactions);

            _blockchain.ReplaceChain(_replacementBlockchain, true);

            Assert.IsTrue(_replacementBlockchain.AreValidTransactions());
            Assert.AreEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void ReplaceChainMultipleMinerTransactions()
        {
            var senderWallet = new Wallet();

            var recipientWallet = new Wallet();

            var firstTransactions = new List<Transaction>
            {
                senderWallet.GenerateTransaction(recipientWallet.PublicKey, 100, _replacementBlockchain),
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var secondTransactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 200, _replacementBlockchain),
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var minerTransaction = Transaction.GetMinerRewardTransaction(senderWallet);

            var thirdTransactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 50, _replacementBlockchain),
                minerTransaction,
                minerTransaction
            };

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);
            _replacementBlockchain.AddBlock(thirdTransactions);

            _blockchain.ReplaceChain(_replacementBlockchain, true);

            Assert.IsFalse(_replacementBlockchain.AreValidTransactions());
            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void ReplaceChainDuplicateTransactions()
        {
            var senderWallet = new Wallet();

            var recipientWallet = new Wallet();

            var firstTransactions = new List<Transaction>
            {
                senderWallet.GenerateTransaction(recipientWallet.PublicKey, 100, _replacementBlockchain),
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var secondTransactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 200, _replacementBlockchain),
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var transaction = recipientWallet.GenerateTransaction(senderWallet.PublicKey, 50, _replacementBlockchain);

            var thirdTransactions = new List<Transaction>
            {
                transaction,
                transaction,
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);
            _replacementBlockchain.AddBlock(thirdTransactions);

            _blockchain.ReplaceChain(_replacementBlockchain, true);

            Assert.IsFalse(_replacementBlockchain.AreValidTransactions());
            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void ReplaceChainInvalidTransactionOutputs()
        {
            var senderWallet = new Wallet();

            var recipientWallet = new Wallet();

            var firstTransactions = new List<Transaction>
            {
                senderWallet.GenerateTransaction(recipientWallet.PublicKey, 100, _replacementBlockchain),
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var secondTransactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 200, _replacementBlockchain),
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var thirdTransactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 50, _replacementBlockchain),
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            thirdTransactions[0].TransactionOutputs[senderWallet.PublicKey] = 9999;

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);
            _replacementBlockchain.AddBlock(thirdTransactions);

            _blockchain.ReplaceChain(_replacementBlockchain, true);

            Assert.IsFalse(_replacementBlockchain.AreValidTransactions());
            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void ReplaceChainTransactionInvalidMinerTransactionOutputs()
        {
            var senderWallet = new Wallet();

            var recipientWallet = new Wallet();

            var firstTransactions = new List<Transaction>
            {
                senderWallet.GenerateTransaction(recipientWallet.PublicKey, 100, _replacementBlockchain),
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var secondTransactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 200, _replacementBlockchain),
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var minerTransaction = Transaction.GetMinerRewardTransaction(senderWallet);

            minerTransaction.TransactionOutputs[senderWallet.PublicKey] = 9999;

            var thirdTransactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 50, _replacementBlockchain),
                minerTransaction
            };

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);
            _replacementBlockchain.AddBlock(thirdTransactions);

            _blockchain.ReplaceChain(_replacementBlockchain, true);

            Assert.IsFalse(_replacementBlockchain.AreValidTransactions());
            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void ReplaceChainInvalidTransactionInput()
        {
            var senderWallet = new Wallet();

            var recipientWallet = new Wallet();

            var firstTransactions = new List<Transaction>
            {
                senderWallet.GenerateTransaction(recipientWallet.PublicKey, 100, _replacementBlockchain),
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            var secondTransactions = new List<Transaction>
            {
                recipientWallet.GenerateTransaction(senderWallet.PublicKey, 200, _replacementBlockchain),
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            senderWallet = new Wallet();

            recipientWallet = new Wallet();

            senderWallet.Balance = 9000;

            var transactionOutputs =
                Transaction.GenerateTransactionOutputs(senderWallet, recipientWallet.PublicKey, 100);

            var transactionInput = Transaction.GenerateTransactionInput(senderWallet, transactionOutputs);

            var transaction = new Transaction(transactionOutputs, transactionInput);

            var thirdTransactions = new List<Transaction>
            {
                transaction,
                Transaction.GetMinerRewardTransaction(senderWallet)
            };

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);
            _replacementBlockchain.AddBlock(thirdTransactions);

            _blockchain.ReplaceChain(_replacementBlockchain, true);

            Assert.IsFalse(_replacementBlockchain.AreValidTransactions());
            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void IsValidChain()
        {
            Assert.IsTrue(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void IsValidChainInvalidGenesisBlock()
        {
            _blockchain.Chain[0] = new Block(0, 0, "fake-lastHash", new List<Transaction>(), 0, 0);

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void IsValidChainInvalidIndex()
        {
            var transactions = new List<Transaction>();

            _blockchain.AddBlock(transactions);

            _blockchain.Chain[_blockchain.Chain.Count - 1].Index = 99;

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void IsValidChainInvalidLastHash()
        {
            var transactions = new List<Transaction>();

            _blockchain.AddBlock(transactions);

            _blockchain.Chain[_blockchain.Chain.Count - 1].LastHash = "fake-lastHash";

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void IsValidChainInvalidTransactions()
        {
            var keyPair = CryptoUtils.GenerateKeyPair();

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var transactionInput = new TransactionInput(0,
                keyPair.Public as ECPublicKeyParameters, 0, keyPair.Private as ECPrivateKeyParameters,
                transactionOutputs);

            var transactions = new List<Transaction>
            {
                new Transaction(transactionOutputs, transactionInput)
            };

            _blockchain.AddBlock(transactions);

            _blockchain.Chain[_blockchain.Chain.Count - 1].Transactions.RemoveAt(0);

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void IsValidChainInvalidDifficulty()
        {
            var transactions = new List<Transaction>();

            _blockchain.AddBlock(transactions);

            var lastBlock = _blockchain.Chain[_blockchain.Chain.Count - 1];

            var fakeBlock = new Block(lastBlock.Index + 1, TimestampUtils.GenerateTimestamp(), lastBlock.Hash,
                new List<Transaction>(),
                0, 10);

            _blockchain.Chain.Add(fakeBlock);

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void BlockchainsAreEqual()
        {
            var sameObject = (object)_blockchain;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_blockchain.Equals(sameObject));
        }

        [TestMethod]
        public void BlockchainsAreNotEqual()
        {
            var differentBlockchain = new Blockchain();

            var block = new Block(0, 0, "test-lastHash", new List<Transaction>(), 0, 1);

            differentBlockchain.Chain.Add(block);

            var differentObject = (object)differentBlockchain;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_blockchain.Equals(differentObject));
        }

        [TestMethod]
        public void BlockchainAndNullAreNotEqual()
        {
            Assert.IsFalse(_blockchain.Equals((object)null));
        }
    }
}