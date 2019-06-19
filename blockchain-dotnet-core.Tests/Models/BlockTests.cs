using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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

        /*[TestMethod]
        public void ConstructBlockWithoutHash()
        {
            var expectedHash = HashUtils.ComputeHash(_block).ToBase64();

            Assert.IsNotNull(_block);
            Assert.AreEqual(_timestamp, _block.Timestamp);
            Assert.AreEqual(_lastHash, _block.LastHash);
            Assert.AreEqual(expectedHash, _block.Hash);
            Assert.AreEqual(_transactions, _block.Transactions);
            Assert.AreEqual(_nonce, _block.Nonce);
            Assert.AreEqual(_difficulty, _block.Difficulty);
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
        }*/

        [TestMethod]
        public void BlocksAreEqual()
        {
            var sameObject = (object) _block;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_block.Equals(sameObject));
        }

        /*[TestMethod]
        public void BlocksAreNotEqual()
        {
            var differentBlock = new Block(0, 0, "test-lashHash", "test-hash", new List<Transaction>(), -1, -1);

            var differentObject = (object) differentBlock;

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
            Assert.IsFalse(_block.Equals((object) null));
        }*/
    }
}