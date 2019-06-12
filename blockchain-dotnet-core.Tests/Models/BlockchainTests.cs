using System.Collections.Generic;
using blockchain_dotnet_core.API.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class BlockchainTests
    {
        private readonly Blockchain _blockchain = new Blockchain();

        [TestMethod]
        public void ConstructsBlockchain()
        {
            Assert.IsNotNull(_blockchain);
            Assert.IsInstanceOfType(_blockchain, typeof(Blockchain));
        }

        [TestMethod]
        public void BlockchainHasChain()
        {
            Assert.IsNotNull(_blockchain.Chain);
            Assert.IsInstanceOfType(_blockchain.Chain, typeof(List<Block>));
        }

        [TestMethod]
        public void ChainHasGenesisBlock()
        {
            //todo: add genesis block testing
        }

        [TestMethod]
        public void BlockchainToStringReturnsValid()
        {
            var result = _blockchain.ToString();

            var expectedResult = _blockchain.Chain.ToString();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void BlockchainsAreEqual()
        {
            var sameObject = (object)_blockchain;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_blockchain.Equals(sameObject));
        }

        [TestMethod]
        public void BlockchainsAreNotEqual()
        {
            var differentBlockchain = new Blockchain();

            var block = new Block(0, string.Empty, string.Empty, new List<Transaction>(), -1, -1);

            differentBlockchain.Chain.Add(block);

            var differentObject = (object)differentBlockchain;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_blockchain.Equals(differentObject));
        }

        [TestMethod]
        public void BlockchainAndObjectAreNotEqual()
        {
            var differentObject = new object();

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_blockchain.Equals(differentObject));
        }

        [TestMethod]
        public void BlockchainAndNullAreNotEqual()
        {
            Assert.IsFalse(_blockchain.Equals((object)null));
        }

        [TestMethod]
        public void BlockchainsHaveSameHashCode()
        {
            var sameBlockchain = _blockchain;

            Assert.IsNotNull(sameBlockchain);
            Assert.IsTrue(_blockchain.GetHashCode() == sameBlockchain.GetHashCode());
        }

        [TestMethod]
        public void BlockchainsDoNotHaveSameHashCode()
        {
            var differentBlockchain = new Blockchain();

            Assert.IsNotNull(differentBlockchain);
            Assert.IsFalse(_blockchain.GetHashCode() == differentBlockchain.GetHashCode());
        }
    }
}
