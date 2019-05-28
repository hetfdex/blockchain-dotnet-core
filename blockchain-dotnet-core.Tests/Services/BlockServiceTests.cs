using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Services;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace blockchain_dotnet_core.Tests.Services
{
    [TestClass]
    public class BlockServiceTests
    {
        private IBlockService _blockService;

        private readonly long _timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        private readonly string _lastHash = SHA256Util.ComputeSHA256("test-lasthash");

        private const string Data = "test-data";

        private const int Nonce = 0;

        private readonly int _difficulty = Constants.InitialDifficulty;

        private string Hash => SHA256Util.ComputeSHA256(_timestamp, _lastHash, Data,
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
                Data = Data,
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
            Assert.AreEqual(Data, _block.Data);
            Assert.AreEqual(Nonce, _block.Nonce);
            Assert.AreEqual(_difficulty, _block.Difficulty);
        }

        [TestMethod]
        public void GetsGenesisBlock()
        {
            var expectedTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            var expectedLastHash = SHA256Util.ComputeSHA256("genesis-lasthash");

            var expectedData = "genesis-data";

            var expectedNonce = 0;

            var expectedDifficulty = Constants.InitialDifficulty;

            var expectedHash = SHA256Util.ComputeSHA256(expectedTimestamp, expectedLastHash, expectedData,
                expectedNonce, expectedDifficulty);

            var genesisBlock = _blockService.GetGenesisBlock();

            Assert.IsNotNull(genesisBlock);
            Assert.IsInstanceOfType(genesisBlock, typeof(Block));
            Assert.AreEqual(expectedTimestamp, genesisBlock.Timestamp);
            Assert.AreEqual(expectedLastHash, genesisBlock.LastHash);
            Assert.AreEqual(expectedHash, genesisBlock.Hash);
            Assert.AreEqual(expectedData, genesisBlock.Data);
            Assert.AreEqual(expectedNonce, genesisBlock.Nonce);
            Assert.AreEqual(expectedDifficulty, genesisBlock.Difficulty);
        }

        [TestMethod]
        public void MinesBlock()
        {
            var lastBlock = _blockService.GetGenesisBlock();

            var data = string.Empty;

            var minedBlock = _blockService.MineBlock(lastBlock, data);

            var expectedHash = SHA256Util.ComputeSHA256(minedBlock.Timestamp, lastBlock.Hash, data, minedBlock.Nonce, minedBlock.Difficulty);

            var expectedLeadingZeros = new string('0', minedBlock.Difficulty);

            Assert.IsNotNull(minedBlock);
            Assert.IsInstanceOfType(minedBlock, typeof(Block));
            Assert.IsNotNull(minedBlock.Timestamp);
            Assert.AreEqual(lastBlock.Hash, minedBlock.LastHash);
            Assert.AreEqual(expectedHash, minedBlock.Hash);
            Assert.AreEqual(expectedLeadingZeros, minedBlock.Hash.Substring(0, minedBlock.Difficulty));
            Assert.AreEqual(data, minedBlock.Data);
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