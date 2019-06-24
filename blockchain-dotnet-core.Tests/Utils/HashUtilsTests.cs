using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class HashUtilsTests
    {
        [TestMethod]
        public void ComputesValidHash()
        {
            var expectedResult = "Wqn5Hyx4HXD0IBr/OeqQJqVWaihRWCXVFkl51GgxxeA=";

            var result = HashUtils.ComputeHash("hetfdex").ToBase64();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ComputesUniqueHashes()
        {
            var firstResult = HashUtils.ComputeHash("hetfdex");

            var secondResult = HashUtils.ComputeHash("bla");

            Assert.IsNotNull(firstResult);
            Assert.IsNotNull(secondResult);
            Assert.AreNotEqual(firstResult, secondResult);
        }

        [TestMethod]
        public void NullBlockReturnsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => HashUtils.ComputeHash((Block)null));
        }

        [TestMethod]
        public void NullLastHashReturnsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                HashUtils.ComputeHash(0, 0, string.Empty, new List<Transaction>(), 0, 0));
        }

        [TestMethod]
        public void NullTransactionsHashReturnsException()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => HashUtils.ComputeHash(0, 0, "test-lastHash", null, 0, 0));
        }

        [TestMethod]
        public void NullStringReturnsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => HashUtils.ComputeHash(string.Empty));
        }

        [TestMethod]
        public void NullBytesReturnsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => HashUtils.ComputeHash((byte[])null));
        }
    }
}