using blockchain_dotnet_core.API.Extensions;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Extensions
{
    [TestClass]
    public class BlockchainExtensionsTests
    {
        private readonly List<Block> _blockchain = new List<Block>();

        [TestInitialize]
        public void BlockchainExtensionsTestsSetup()
        {
            _blockchain.Add(BlockUtils.GetGenesisBlock());

            var transactionsOne = new List<Transaction>
             {
                 new Transaction()
             };

            var transactionsTwo = new List<Transaction>
             {
                 new Transaction(),
                 new Transaction()
             };

            _blockchain.AddBlock(transactionsOne);
            _blockchain.AddBlock(transactionsTwo);
        }

        [TestMethod]
        public void ConstructBlockchain()
        {
            Assert.IsNotNull(_blockchain);
            Assert.IsInstanceOfType(_blockchain, typeof(List<Block>));
            Assert.AreEqual(BlockUtils.GetGenesisBlock(), _blockchain[0]);
        }

        [TestMethod]
        public void AddsToBlockchain()
        {
            var transactions = new List<Transaction>();

            _blockchain.AddBlock(transactions);

            Assert.AreEqual(transactions, _blockchain[_blockchain.Count - 1].Transactions);
        }

        [TestMethod]
        public void BlockchainIsValid()
        {
            Assert.IsTrue(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void BlockchainIsNotValidNoGenesisBlock()
        {
            _blockchain[0] = new Block();

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void BlockchainIsNotValidFakeLastHash()
        {
            _blockchain[_blockchain.Count - 1].LastHash = "fake-lastHash";

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void BlockchainIsNotValidFakeTransactions()
        {
            _blockchain[_blockchain.Count - 1].Transactions = null;

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void BlockchainIsNotValidFakeDifficulty()
        {
            var lastBlock = _blockchain[_blockchain.Count - 1];

            var fakeBlock = new Block
            {
                Timestamp = TimestampUtils.GenerateTimestamp(),
                LastHash = lastBlock.Hash,
                Transactions = new List<Transaction>(),
                Nonce = 0,
                Difficulty = lastBlock.Difficulty - 2
            };

            fakeBlock.Hash = HashUtils.ComputeSHA256(fakeBlock);

            _blockchain.Add(fakeBlock);

            Assert.IsFalse(_blockchain.IsValidChain());
        }
    }
}