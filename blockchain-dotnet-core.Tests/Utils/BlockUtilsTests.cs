using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class BlockUtilsTests
    {
        private Block _block;

        [TestInitialize]
        public void BlockTestsSetup()
        {
            _block = new Block(TimestampUtils.GenerateTimestamp(), HashUtils.ComputeSHA256("test-lastHash"), new List<Transaction>(), 0, ConfigurationOptions.InitialDifficulty);
        }

        [TestMethod]
        public void GetsGenesisBlock()
        {
            var expectedTimestamp = 0L;

            var expectedLastHash = HashUtils.ComputeSHA256("genesis-lastHash");

            var expectedTransactions = new List<Transaction>();

            var expectedNonce = 0;

            var expectedDifficulty = ConfigurationOptions.InitialDifficulty;

            var expectedHash = HashUtils.ComputeSHA256(expectedTimestamp, expectedLastHash, expectedTransactions,
                expectedNonce, expectedDifficulty);

            var genesisBlock = BlockUtils.GetGenesisBlock();

            Assert.IsNotNull(genesisBlock);
            Assert.IsInstanceOfType(genesisBlock, typeof(Block));
            Assert.AreEqual(expectedTimestamp, genesisBlock.Timestamp);
            Assert.AreEqual(expectedLastHash, genesisBlock.LastHash);
            Assert.AreEqual(expectedHash, genesisBlock.Hash);
            Assert.IsTrue(expectedTransactions.SequenceEqual(genesisBlock.Transactions));
            Assert.AreEqual(expectedNonce, genesisBlock.Nonce);
            Assert.AreEqual(expectedDifficulty, genesisBlock.Difficulty);
        }

        [TestMethod]
        public void MinesBlock()
        {
            var lastBlock = BlockUtils.GetGenesisBlock();

            var transactions = new List<Transaction>();

            var minedBlock = BlockUtils.MineBlock(lastBlock, transactions);

            var expectedHash = HashUtils.ComputeSHA256(minedBlock);

            var expectedLeadingZeros = new string('0', minedBlock.Difficulty);

            Assert.IsNotNull(minedBlock);
            Assert.IsInstanceOfType(minedBlock, typeof(Block));
            Assert.IsNotNull(minedBlock.Timestamp);
            Assert.AreEqual(lastBlock.Hash, minedBlock.LastHash);
            Assert.AreEqual(expectedHash, minedBlock.Hash);
            Assert.AreEqual(expectedLeadingZeros, minedBlock.Hash.Substring(0, minedBlock.Difficulty));
            Assert.IsTrue(transactions.SequenceEqual(minedBlock.Transactions));
        }

        [TestMethod]
        public void AdjustsDifficultyUpwardsIfMiningTooQuick()
        {
            var expectedRaisedDifficulty = _block.Difficulty + 1;

            var result = BlockUtils.AdjustDifficulty(_block, _block.Timestamp + ConfigurationOptions.MiningRate - 100);

            Assert.AreEqual(expectedRaisedDifficulty, result);
        }

        [TestMethod]
        public void AdjustsDifficultyDownwardsIfMiningTooSlow()
        {
            var expectedLoweredDifficulty = _block.Difficulty - 1;

            var result = BlockUtils.AdjustDifficulty(_block, _block.Timestamp + ConfigurationOptions.MiningRate + 100);

            Assert.AreEqual(expectedLoweredDifficulty, result);
        }

        [TestMethod]
        public void DifficultyLowerLimitOf1()
        {
            _block.Difficulty = -1;

            var expectedLimitDifficulty = 1;

            var result = BlockUtils.AdjustDifficulty(_block, _block.Timestamp);

            Assert.AreEqual(expectedLimitDifficulty, result);
        }
    }
}