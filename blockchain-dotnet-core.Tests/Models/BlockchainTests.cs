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
            Assert.IsNotNull(_blockchain.Chain);
        }

        [TestMethod]
        public void ChainHasGenesisBlock()
        {
            var genesisBlock = Block.GetGenesisBlock();

            Assert.IsTrue(_blockchain.Chain.Count == 1);
            Assert.IsNotNull(_blockchain.Chain[0]);
            Assert.AreEqual(genesisBlock, _blockchain.Chain[0]);
        }

        /*[TestMethod]
        public void BlockchainsAreEqual()
        {
            var sameObject = (object) _blockchain;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_blockchain.Equals(sameObject));
        }

        [TestMethod]
        public void BlockchainsAreNotEqual()
        {
            var differentBlockchain = new Blockchain();

            var block = new Block(0, string.Empty, string.Empty, new List<Transaction>(), -1, -1);

            differentBlockchain.Chain.Add(block);

            var differentObject = (object) differentBlockchain;

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
            Assert.IsFalse(_blockchain.Equals((object) null));
        }*/
    }
}