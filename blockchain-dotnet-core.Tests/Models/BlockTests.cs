using blockchain_dotnet_core.API;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class BlockTests
    {
        private readonly int _index = 0;

        private readonly long _timestamp = TimestampUtils.GenerateTimestamp();

        private readonly string _lastHash = HashUtils.ComputeHash("test-lastHash").ToBase64();

        private readonly IList<Transaction> _transactions = new List<Transaction>();

        private readonly int _nonce = 0;

        private readonly int _difficulty = 1;

        private Block _block;

        [TestInitialize]
        public void BlockTestsSetup()
        {
            _block = new Block(_index, _timestamp, _lastHash, _transactions, _nonce, _difficulty);
        }

        [TestMethod]
        public void ConstructBlockWithoutHash()
        {
            var expectedHash = HashUtils.ComputeHash(_block).ToBase64();

            Assert.IsNotNull(_block);
            Assert.AreEqual(_index, _block.Index);
            Assert.AreEqual(_timestamp, _block.Timestamp);
            Assert.AreEqual(_lastHash, _block.LastHash);
            Assert.AreEqual(expectedHash, _block.Hash);
            Assert.AreEqual(_transactions, _block.Transactions);
            Assert.AreEqual(_nonce, _block.Nonce);
            Assert.AreEqual(_difficulty, _block.Difficulty);
        }

        [TestMethod]
        public void ConstructBlockWithoutHashNullLastHashThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new Block(_index, _timestamp, null, _transactions, _nonce, _difficulty));
        }

        [TestMethod]
        public void ConstructBlockWithoutHashNullTransactionsThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new Block(_index, _timestamp, _lastHash, null, _nonce, _difficulty));
        }

        [TestMethod]
        public void ConstructBlockWithHash()
        {
            var hash = HashUtils.ComputeHash(_index, _timestamp, _lastHash, _transactions, _nonce,
                _difficulty).ToBase64();

            var blockWithHash = new Block(_index, _timestamp, _lastHash, hash, _transactions, _nonce, _difficulty);

            var expectedHash = HashUtils.ComputeHash(blockWithHash).ToBase64();

            Assert.IsNotNull(blockWithHash);
            Assert.AreEqual(_timestamp, blockWithHash.Timestamp);
            Assert.AreEqual(_lastHash, blockWithHash.LastHash);
            Assert.AreEqual(expectedHash, blockWithHash.Hash);
            Assert.AreEqual(_transactions, blockWithHash.Transactions);
            Assert.AreEqual(_nonce, blockWithHash.Nonce);
            Assert.AreEqual(_difficulty, blockWithHash.Difficulty);
        }

        [TestMethod]
        public void ConstructBlockWithHashNullLastHashThrowsException()
        {
            var hash = HashUtils.ComputeHash(_index, _timestamp, _lastHash, _transactions, _nonce,
                _difficulty).ToBase64();

            Assert.ThrowsException<ArgumentNullException>(() =>
                new Block(_index, _timestamp, null, hash, _transactions, _nonce, _difficulty));
        }

        [TestMethod]
        public void ConstructBlockWithHashNullTransactionsThrowsException()
        {
            var hash = HashUtils.ComputeHash(_index, _timestamp, _lastHash, _transactions, _nonce,
                _difficulty).ToBase64();

            Assert.ThrowsException<ArgumentNullException>(() =>
                new Block(_index, _timestamp, _lastHash, hash, null, _nonce, _difficulty));
        }

        [TestMethod]
        public void ConstructBlockWithHashNullHashThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new Block(_index, _timestamp, _lastHash, null, _transactions, _nonce, _difficulty));
        }

        [TestMethod]
        public void MineBlock()
        {
            var lastBlock = Block.GetGenesisBlock();

            var transactions = new List<Transaction>();

            var minedBlock = Block.MineBlock(lastBlock, transactions);

            var expectedHash = HashUtils.ComputeHash(minedBlock).ToBase64();

            var expectedLeadingZeros = new string('0', minedBlock.Difficulty);

            Assert.IsNotNull(minedBlock);
            Assert.AreEqual(lastBlock.Index + 1, minedBlock.Index);
            Assert.IsNotNull(minedBlock.Timestamp);
            Assert.AreEqual(lastBlock.Hash, minedBlock.LastHash);
            Assert.AreEqual(expectedHash, minedBlock.Hash);
            Assert.AreEqual(expectedLeadingZeros, minedBlock.Hash.Substring(0, minedBlock.Difficulty));
            Assert.IsTrue(transactions.SequenceEqual(minedBlock.Transactions));
        }

        [TestMethod]
        public void MineBlockNullLastBlockThrowsException()
        {
            var transactions = new List<Transaction>();

            Assert.ThrowsException<ArgumentNullException>(() => Block.MineBlock(null, transactions));
        }

        [TestMethod]
        public void MineBlockNullTransactionsThrowsException()
        {
            var lastBlock = Block.GetGenesisBlock();

            Assert.ThrowsException<ArgumentNullException>(() => Block.MineBlock(lastBlock, null));
        }

        [TestMethod]
        public void AdjustDifficultyUpwardsIfMiningTooQuick()
        {
            var expectedRaisedDifficulty = _block.Difficulty + 1;

            var result = Block.AdjustDifficulty(_block, _block.Timestamp + Constants.MiningRate - 100);

            Assert.AreEqual(expectedRaisedDifficulty, result);
        }

        [TestMethod]
        public void AdjustDifficultyDownwardsIfMiningTooSlow()
        {
            var expectedLoweredDifficulty = _block.Difficulty - 1;

            var result = Block.AdjustDifficulty(_block, _block.Timestamp + Constants.MiningRate + 100);

            Assert.AreEqual(expectedLoweredDifficulty, result);
        }

        [TestMethod]
        public void AdjustDifficultyLowerLimitOf1()
        {
            _block.Difficulty = -1;

            var expectedLimitDifficulty = 1;

            var result = Block.AdjustDifficulty(_block, _block.Timestamp);

            Assert.AreEqual(expectedLimitDifficulty, result);
        }

        [TestMethod]
        public void AdjustDifficultyNullLastBlockThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Block.AdjustDifficulty(null, 0));
        }

        [TestMethod]
        public void GetGenesisBlock()
        {
            var expectedIndex = 0;

            var expectedTimestamp = 0L;

            var expectedLastHash = HashUtils.ComputeHash("genesis-lastHash").ToBase64();

            var expectedTransactions = new List<Transaction>();

            var expectedNonce = 0;

            var expectedDifficulty = Constants.InitialDifficulty;

            var expectedHash = HashUtils.ComputeHash(expectedIndex, expectedTimestamp, expectedLastHash,
                expectedTransactions,
                expectedNonce, expectedDifficulty).ToBase64();

            var genesisBlock = Block.GetGenesisBlock();

            Assert.IsNotNull(genesisBlock);
            Assert.AreEqual(expectedIndex, genesisBlock.Index);
            Assert.AreEqual(expectedTimestamp, genesisBlock.Timestamp);
            Assert.AreEqual(expectedLastHash, genesisBlock.LastHash);
            Assert.AreEqual(expectedHash, genesisBlock.Hash);
            Assert.IsTrue(expectedTransactions.SequenceEqual(genesisBlock.Transactions));
            Assert.AreEqual(expectedNonce, genesisBlock.Nonce);
            Assert.AreEqual(expectedDifficulty, genesisBlock.Difficulty);
        }

        [TestMethod]
        public void BlocksAreEqual()
        {
            var sameObject = (object)_block;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_block.Equals(sameObject));
        }

        [TestMethod]
        public void BlocksAreNotEqual()
        {
            var differentBlock = new Block(0, 0, "test-lashHash", "test-hash", new List<Transaction>(), -1, -1);

            var differentObject = (object)differentBlock;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_block.Equals(differentObject));
        }

        [TestMethod]
        public void BlockAndNullAreNotEqual()
        {
            Assert.IsFalse(_block.Equals((object)null));
        }
    }
}