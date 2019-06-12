using System;
using System.Collections.Generic;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class HashUtilsTests
    {
        [TestMethod]
        public void GeneratesSHA256Output()
        {
            var expectedResult = "5aa9f91f2c781d70f4201aff39ea9026a5566a28515825d5164979d46831c5e0";

            var result = HashUtils.ComputeSHA256("hetfdex");

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GeneratesUniqueHashWhenArgumentsChange()
        {
            var block = new Block(0, string.Empty, string.Empty, new List<Transaction>(), -1, -1);

            var blockHash = HashUtils.ComputeSHA256(block);

            block.LastHash = "fake-lastHash";

            var modifiedBlockHash = HashUtils.ComputeSHA256(block);

            Assert.IsNotNull(blockHash);
            Assert.IsNotNull(modifiedBlockHash);
            Assert.AreNotEqual(blockHash, modifiedBlockHash);
        }
    }
}