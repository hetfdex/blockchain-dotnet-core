using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class BlockTests
    {
        private readonly long _timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

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
            var sameBlockObject = (object)_block;

            Assert.IsNotNull(sameBlockObject);
            Assert.IsTrue(_block.Equals(sameBlockObject));
        }

        [TestMethod]
        public void BlocksAreNotEqualDifferentProperties()
        {
            var differentBlock = new Block()
            {
                Timestamp = 0L,
                LastHash = "fake-lasHash",
                Hash = "fake-Hash",
                Transactions = new List<Transaction>(),
                Nonce = -1,
                Difficulty = -1
            };

            var differentBlockObject = (object)differentBlock;

            Assert.IsNotNull(differentBlockObject);
            Assert.IsFalse(_block.Equals(differentBlockObject));
        }

        [TestMethod]
        public void BlockAndObjectAreNotEqual()
        {
            var differentBlockObject = new object();

            Assert.IsNotNull(differentBlockObject);
            Assert.IsFalse(_block.Equals(differentBlockObject));
        }

        [TestMethod]
        public void BlockAndNullAreNotEqual()
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
            var differentBlock = new Block()
            {
                Timestamp = 0L,
                LastHash = "fake-lasHash",
                Hash = "fake-Hash",
                Transactions = new List<Transaction>(),
                Nonce = -1,
                Difficulty = -1
            };

            Assert.IsNotNull(differentBlock);
            Assert.IsFalse(_block.GetHashCode() == differentBlock.GetHashCode());
        }
    }
}