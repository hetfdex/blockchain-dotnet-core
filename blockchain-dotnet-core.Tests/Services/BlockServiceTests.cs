using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Services;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Services
{
    [TestClass]
    public class BlockServiceTests
    {
        private IBlockService _blockService;

        private readonly long _timestamp = TimestampUtils.GenerateTimestamp();

        private readonly string _lastHash = HashUtils.ComputeSHA256("test-lasthash");

        private const List<Transaction> Transactions = null;

        private const int Nonce = 0;

        private readonly int _difficulty = Constants.InitialDifficulty;

        private string Hash => HashUtils.ComputeSHA256(_timestamp, _lastHash, Transactions,
            Nonce, _difficulty);

        private Block _block;

        [TestInitialize]
        public void BlockServiceTestsSetup()
        {
            _blockService = new BlockService();

            _block = new Block()
            {
                Timestamp = _timestamp,
                LastHash = _lastHash,
                Hash = Hash,
                Transactions = Transactions,
                Nonce = Nonce,
                Difficulty = _difficulty
            };
        }

        [TestMethod]
        public void ConstructBlock()
        {
            Assert.IsNotNull(_block);
            Assert.AreEqual(_timestamp, _block.Timestamp);
            Assert.AreEqual(_lastHash, _block.LastHash);
            Assert.AreEqual(Hash, _block.Hash);
            Assert.AreEqual(Transactions, _block.Transactions);
            Assert.AreEqual(Nonce, _block.Nonce);
            Assert.AreEqual(_difficulty, _block.Difficulty);
        }

        [TestMethod]
        public void GetsGenesisBlock()
        {
            var expectedTimestamp = 0L;

            var expectedLastHash = HashUtils.ComputeSHA256("genesis-lasthash");

            var expectedTransactions = new List<Transaction>();

            var expectedNonce = 0;

            var expectedDifficulty = Constants.InitialDifficulty;

            var expectedHash = HashUtils.ComputeSHA256(expectedTimestamp, expectedLastHash, expectedTransactions,
                expectedNonce, expectedDifficulty);

            var genesisBlock = _blockService.GetGenesisBlock();

            Assert.IsNotNull(genesisBlock);
            Assert.IsInstanceOfType(genesisBlock, typeof(Block));
            Assert.AreEqual(expectedTimestamp, genesisBlock.Timestamp);
            Assert.AreEqual(expectedLastHash, genesisBlock.LastHash);
            Assert.AreEqual(expectedHash, genesisBlock.Hash);
            Assert.IsInstanceOfType(genesisBlock.Transactions, typeof(List<Transaction>));
            Assert.IsNotNull(genesisBlock.Transactions);
            Assert.AreEqual(0, genesisBlock.Transactions.Count);
            Assert.AreEqual(expectedNonce, genesisBlock.Nonce);
            Assert.AreEqual(expectedDifficulty, genesisBlock.Difficulty);
        }

        [TestMethod]
        public void MinesBlock()
        {
            var lastBlock = _blockService.GetGenesisBlock();

            var transactions = new List<Transaction>();

            var minedBlock = _blockService.MineBlock(lastBlock, transactions);

            var expectedHash = HashUtils.ComputeSHA256(minedBlock.Timestamp, lastBlock.Hash, transactions, minedBlock.Nonce, minedBlock.Difficulty);

            var expectedLeadingZeros = new string('0', minedBlock.Difficulty);

            Assert.IsNotNull(minedBlock);
            Assert.IsInstanceOfType(minedBlock, typeof(Block));
            Assert.IsNotNull(minedBlock.Timestamp);
            Assert.AreEqual(lastBlock.Hash, minedBlock.LastHash);
            Assert.AreEqual(expectedHash, minedBlock.Hash);
            Assert.AreEqual(expectedLeadingZeros, minedBlock.Hash.Substring(0, minedBlock.Difficulty));
            Assert.AreEqual(transactions, minedBlock.Transactions);
        }

        [TestMethod]
        public void AdjustsDifficultyUpwardsIfMiningTooQuick()
        {
            var expectedRaisedDifficulty = _block.Difficulty + 1;

            var result = _blockService.AdjustDifficulty(_block, _block.Timestamp + Constants.MineRate - 100);

            Assert.AreEqual(expectedRaisedDifficulty, result);
        }

        [TestMethod]
        public void AdjustsDifficultyDownwardsIfMiningTooSlow()
        {
            var expectedLoweredDifficulty = _block.Difficulty - 1;

            var result = _blockService.AdjustDifficulty(_block, _block.Timestamp + Constants.MineRate + 100);

            Assert.AreEqual(expectedLoweredDifficulty, result);
        }

        [TestMethod]
        public void DifficultyLowerLimitOf1()
        {
            _block.Difficulty = -1;

            var expectedLimitDifficulty = 1;

            var result = _blockService.AdjustDifficulty(_block, _block.Timestamp);

            Assert.AreEqual(expectedLimitDifficulty, result);
        }
    }
}