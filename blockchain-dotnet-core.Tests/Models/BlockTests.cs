using System;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class BlockTests
    {
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
        public void BlockWithHash_ToString_ReturnsHash()
        {
            var result = _block.ToString();

            Assert.IsNotNull(result);
            Assert.AreEqual(result, _block.Hash);
        }

        [TestMethod]
        public void BlockNoHash_ToString_ReturnsHash()
        {
            var expectedHash = Hash;

            _block.Hash = null;

            var result = _block.ToString();

            Assert.IsNotNull(result);
            Assert.AreEqual(result, expectedHash);
        }

        [TestMethod]
        public void BlocksAreEqual()
        {
            var sameBlock = _block;

            Assert.IsNotNull(sameBlock);
            Assert.IsTrue(_block.Equals(sameBlock));
        }

        [TestMethod]
        public void BlocksAreNotEqual()
        {
            var differentBlock = new Block();

            Assert.IsNotNull(differentBlock);
            Assert.IsFalse(_block.Equals(differentBlock));
        }

        [TestMethod]
        public void BlockObjectsAreEqual()
        {
            var sameBlock = (object)_block;

            Assert.IsNotNull(sameBlock);
            Assert.IsTrue(_block.Equals(sameBlock));
        }

        [TestMethod]
        public void BlockObjectsAreNotEqual()
        {
            var differentBlock = new object();

            Assert.IsNotNull(differentBlock);
            Assert.IsFalse(_block.Equals(differentBlock));
        }

        [TestMethod]
        public void BlockObjectsAreNotEqualWithNull()
        {
            object differentBlock = null;

            Assert.IsNull(differentBlock);
            Assert.IsFalse(_block.Equals(differentBlock));
        }

        [TestMethod]
        public void BlocksHaveSameHashCode()
        {
            var sameBlock = _block;

            Assert.IsNotNull(sameBlock);
            Assert.IsTrue(_block.GetHashCode() == sameBlock.GetHashCode());
        }

        [TestMethod]
        public void BlocksDoNotHaveSameHashCode()
        {
            var differentBlock = new Block();

            Assert.IsNotNull(differentBlock);
            Assert.IsFalse(_block.GetHashCode() == differentBlock.GetHashCode());
        }
    }
}