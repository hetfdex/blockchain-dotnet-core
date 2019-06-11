using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class BlockTests
    {
        private readonly long _timestamp = TimestampUtils.GenerateTimestamp();

        private readonly string _lastHash = HashUtils.ComputeSHA256("test-lastHash");

        private readonly List<Transaction> _transactions = new List<Transaction>();

        private readonly int _nonce = 0;

        private readonly int _difficulty = 1;

        private Block _block;

        [TestInitialize]
        public void BlockTestsSetup()
        {
            _block = new Block(_timestamp, _lastHash, _transactions, _nonce, _difficulty);

            _block.Hash = HashUtils.ComputeSHA256(_block);
        }

        [TestMethod]
        public void ConstructBlock()
        {
            var expectedHash = HashUtils.ComputeSHA256(_block);

            Assert.IsNotNull(_block);
            Assert.AreEqual(_timestamp, _block.Timestamp);
            Assert.AreEqual(_lastHash, _block.LastHash);
            Assert.AreEqual(expectedHash, _block.Hash);
            Assert.AreEqual(_transactions, _block.Transactions);
            Assert.AreEqual(_nonce, _block.Nonce);
            Assert.AreEqual(_difficulty, _block.Difficulty);
        }

        [TestMethod]
        public void BlockWithHashToStringReturnsHash()
        {
            var result = _block.ToString();

            Assert.IsNotNull(result);
            Assert.AreEqual(_block.Hash, result);
        }

        [TestMethod]
        public void BlockWithoutHashToStringReturnsHash()
        {
            var expectedResult = HashUtils.ComputeSHA256(_block);

            _block.Hash = null;

            var result = _block.ToString();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void BlocksAreEqual()
        {
            var sameObject = (object)_block;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_block.Equals(sameObject));
        }

        [TestMethod]
        public void BlocksAreNotEqualDifferentProperties()
        {
            var differentBlock = new Block(0, "fake-lashHash", "fake-hash", new List<Transaction>(), -1, -1);

            var differentObject = (object)differentBlock;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_block.Equals(differentObject));
        }

        [TestMethod]
        public void BlockAndObjectAreNotEqual()
        {
            var differentObject = new object();

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_block.Equals(differentObject));
        }

        [TestMethod]
        public void BlockAndNullAreNotEqual()
        {
            Assert.IsFalse(_block.Equals((object)null));
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
            var differentBlock = new Block(0, "fake-lashHash", "fake-hash", new List<Transaction>(), -1, -1);

            Assert.IsNotNull(differentBlock);
            Assert.IsFalse(_block.GetHashCode() == differentBlock.GetHashCode());
        }
    }
}