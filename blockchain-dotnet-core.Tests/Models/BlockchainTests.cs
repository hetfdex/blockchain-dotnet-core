using System;
using System.Collections.Generic;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class BlockchainTests
    {
        private readonly Blockchain _blockchain = new Blockchain();

        [TestMethod]
        public void ConstructsBlockchain()
        {
            Assert.IsNotNull(_blockchain);
            Assert.IsNotNull(_blockchain.Chain);
        }

        [TestMethod]
        public void ChainHasGenesisBlock()
        {
            var genesisBlock = Block.GetGenesisBlock();

            Assert.IsTrue(_blockchain.Chain.Count == 1);
            Assert.IsNotNull(_blockchain.Chain[_blockchain.Chain.Count - 1]);
            Assert.AreEqual(genesisBlock, _blockchain.Chain[0]);
        }

        [TestMethod]
        public void AddBlock()
        {
            var transactions = new List<Transaction>();

            _blockchain.AddBlock(transactions);

            Assert.IsTrue(_blockchain.Chain.Count == 2);
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
        public void IsValidChain()
        {
            Assert.IsTrue(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void IsValidChainFakeGenesisBlock()
        {
            _blockchain.Chain[0] = new Block(0, 0, "fake-lastHash", new List<Transaction>(), 0, 0);

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void IsValidChainFakeIndex()
        {
            var transactions = new List<Transaction>();

            _blockchain.AddBlock(transactions);

            _blockchain.Chain[_blockchain.Chain.Count - 1].Index = 99;

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void IsValidChainFakeLastHash()
        {
            var transactions = new List<Transaction>();

            _blockchain.AddBlock(transactions);

            _blockchain.Chain[_blockchain.Chain.Count - 1].LastHash = "fake-lastHash";

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void IsValidChainFakeTransactions()
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
        public void IsValidChainFakeDifficulty()
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
            var sameObject = (object) _blockchain;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_blockchain.Equals(sameObject));
        }

        [TestMethod]
        public void BlockchainsAreNotEqual()
        {
            var differentBlockchain = new Blockchain();

            var block = new Block(0, 0, "test-lastHash", new List<Transaction>(), 0, 1);

            differentBlockchain.Chain.Add(block);

            var differentObject = (object) differentBlockchain;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_blockchain.Equals(differentObject));
        }

        [TestMethod]
        public void BlockchainAndNullAreNotEqual()
        {
            Assert.IsFalse(_blockchain.Equals((object) null));
        }
    }
}