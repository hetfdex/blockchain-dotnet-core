using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class BlockTests
    {
        private Block _block;

        [TestInitialize]
        public void BlockTestsSetup()
        {
            _block = new Block
            {
                Timestamp = TimestampUtils.GenerateTimestamp(),
                LastHash = HashUtils.ComputeSHA256("test-lastHash"),
                Transactions = new List<Transaction>(),
                Nonce = 0,
                Difficulty = Constants.InitialDifficulty
            };

            _block.Hash = HashUtils.ComputeSHA256(_block);
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
            var differentBlock = new Block
            {
                Timestamp = 0L,
                LastHash = "fake-lasHash",
                Hash = "fake-Hash",
                Transactions = new List<Transaction>(),
                Nonce = -1,
                Difficulty = -1
            };

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
            object differentObject = null;

            Assert.IsNull(differentObject);
            Assert.IsFalse(_block.Equals(differentObject));
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
            var differentBlock = new Block
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