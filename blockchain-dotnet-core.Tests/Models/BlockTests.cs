using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class BlockTests
    {
        [TestMethod]
        public void BlockWithHash_ToString_ReturnsHash()
        {
            var block = new Block
            {
                Timestamp = 0,
                LastHash = SHA256Util.ComputeSHA256("genesis-lasthash"),
                Data = "genesis-data",
                Nonce = 0,
                Difficulty = Constants.InitialDifficulty
            };

            block.Hash = SHA256Util.ComputeSHA256(block);

            var result = block.ToString();

            Assert.IsNotNull(result);
            Assert.AreEqual(result, block.Hash);
        }

        [TestMethod]
        public void BlockNoHash_ToString_ReturnsHash()
        {
            var block = new Block
            {
                Timestamp = 0,
                LastHash = SHA256Util.ComputeSHA256("genesis-lasthash"),
                Data = "genesis-data",
                Nonce = 0,
                Difficulty = Constants.InitialDifficulty
            };

            var expectedHash = SHA256Util.ComputeSHA256(block);

            var result = block.ToString();

            Assert.IsNotNull(result);
            Assert.AreEqual(result, expectedHash);
        }
    }
}