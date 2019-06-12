using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class HashUtilsTests
    {
        [TestMethod]
        public void ComputesValidHash()
        {
            var expectedResult = "5aa9f91f2c781d70f4201aff39ea9026a5566a28515825d5164979d46831c5e0";

            var result = HashUtils.ComputeHash("hetfdex");

            //todo: test bytes
            var resultAsString = HexUtils.BytesToString(result);

            Assert.IsNotNull(resultAsString);
            Assert.AreEqual(expectedResult, resultAsString);
        }

        [TestMethod]
        public void ComputesUniqueHashes()
        {
            var block = new Block(0, string.Empty, string.Empty, new List<Transaction>(), -1, -1);

            var hash = HashUtils.ComputeHash(block);

            block.LastHash = "test-lastHash";

            var modifiedHash = HashUtils.ComputeHash(block);

            Assert.IsNotNull(hash);
            Assert.IsNotNull(modifiedHash);
            Assert.AreNotEqual(hash, modifiedHash);
        }
    }
}