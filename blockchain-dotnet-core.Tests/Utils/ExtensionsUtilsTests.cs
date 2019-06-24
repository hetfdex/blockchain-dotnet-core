using blockchain_dotnet_core.API.Models;
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
        public void TransactionToHashableString()
        {
            var keyPair = CryptoUtils.GenerateKeyPair();

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var transactionInput = new TransactionInput(0,
                keyPair.Public as ECPublicKeyParameters, 0, keyPair.Private as ECPrivateKeyParameters,
                transactionOutputs);

            var transactions = new List<Transaction>
            {
                new Transaction(transactionOutputs, transactionInput)
            };

            var result = transactions.ToHashableString();

            var expectedResult = string.Empty;

            foreach (var transaction in transactions)
            {
                expectedResult += transaction.Id;
                expectedResult += transaction.TransactionOutputs.ToHashableString();
                expectedResult += transaction.TransactionInput.Timestamp;
                expectedResult += transaction.TransactionInput.Address;
                expectedResult += transaction.TransactionInput.Amount;
                expectedResult += transaction.TransactionInput.Signature;
            }

            Assert.IsNotNull(result);
            Assert.IsNotNull(expectedResult);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TransactionToHashableStringNullTransactionsThrowsException()
        {
            List<Transaction> nullTransactions = null;

            Assert.ThrowsException<ArgumentNullException>(() => nullTransactions.ToHashableString());
        }

        [TestMethod]
        public void TransactionOutputsToHashableString()
        {
            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var result = transactionOutputs.ToHashableString();

            var expectedResult = JsonConvert.SerializeObject(transactionOutputs);

            Assert.IsNotNull(result);
            Assert.IsNotNull(expectedResult);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TransactionOutputsToHashableStringNullTransactionsThrowsException()
        {
            Dictionary<ECPublicKeyParameters, decimal> nullTransactionOutputs = null;

            Assert.ThrowsException<ArgumentNullException>(() => nullTransactionOutputs.ToHashableString());
        }

        [TestMethod]
        public void TransactionOutputsToHash()
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
        public void TransactionOutputsToHashNullTransactionOutputsThrowsException()
        {
            Dictionary<ECPublicKeyParameters, decimal> nullTransactionOutputs = null;

            Assert.ThrowsException<ArgumentNullException>(() => nullTransactionOutputs.ToHash());
        }

        [TestMethod]
        public void ToBase64()
        {
            var bytes = new byte[1];

            var result = bytes.ToBase64();

            var expectedResult = Convert.ToBase64String(bytes);

            Assert.IsNotNull(result);
            Assert.IsNotNull(expectedResult);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ToBase64NullBytesThrowsException()
        {
            byte[] nullBytes = null;

            Assert.ThrowsException<ArgumentNullException>(() => nullBytes.ToBase64());
        }

        [TestMethod]
        public void FromBase64()
        {
            var s = "aGV0ZmRleA==";

            var result = s.FromBase64();

            var expectedResult = Convert.FromBase64String(s);

            Assert.IsNotNull(result);
            Assert.IsNotNull(expectedResult);
            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        [TestMethod]
        public void FromBase64NullStringThrowsException()
        {
            string s = null;

            Assert.ThrowsException<ArgumentNullException>(() => s.FromBase64());
        }
    }
}