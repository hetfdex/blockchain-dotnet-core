using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class ExtensionsUtilsTests
    {
        [TestMethod]
        public void HashesTransactionOutputs()
        {
            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var result = transactionOutputs.ToHash();

            var serialized = JsonConvert.SerializeObject(transactionOutputs);

            var expectedResult = HashUtils.ComputeHash(serialized);

            Assert.IsNotNull(result);
            Assert.IsNotNull(expectedResult);
            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        [TestMethod]
        public void ToHashNullTransactionOutputsReturnsException()
        {
            Dictionary<ECPublicKeyParameters, decimal> nullTransactionOutputs = null;

            Assert.ThrowsException<ArgumentNullException>(() => nullTransactionOutputs.ToHash());
        }

        [TestMethod]
        public void ConvertsBytesToBase64String()
        {
            var bytes = new byte[1];

            var result = bytes.ToBase64();

            var expectedResult = Convert.ToBase64String(bytes);

            Assert.IsNotNull(result);
            Assert.IsNotNull(expectedResult);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ToBase64NullBytesReturnsException()
        {
            byte[] nullBytes = null;

            Assert.ThrowsException<ArgumentNullException>(() => nullBytes.ToBase64());
        }

        [TestMethod]
        public void ConvertsBase64StringToBytes()
        {
            var s = "aGV0ZmRleA==";

            var result = s.FromBase64();

            var expectedResult = Convert.FromBase64String(s);

            Assert.IsNotNull(result);
            Assert.IsNotNull(expectedResult);
            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        [TestMethod]
        public void FromBase64NullStringReturnsException()
        {
            string s = null;

            Assert.ThrowsException<ArgumentNullException>(() => s.FromBase64());
        }
    }
}