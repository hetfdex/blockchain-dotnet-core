using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class TransactionTests
    {
        private TransactionInput _transactionInput;

        private Transaction _transaction;

        [TestInitialize]
        public void TransactionTestsSetup()
        {
            var keyPairSender = KeyPairUtils.GenerateKeyPair();

            var keyPairRecipient = KeyPairUtils.GenerateKeyPair();

            var keyPair = KeyPairUtils.GenerateKeyPair();

            _transactionInput = new TransactionInput
            {
                Timestamp = TimestampUtils.GetTimestamp(),
                Address = keyPair.Public as ECPublicKeyParameters,
                Amount = 0,
                Signature = "test-signature"
            };

            _transaction = new Transaction
            {
                Sender = keyPairSender.Public as ECPublicKeyParameters,
                Recipient = keyPairRecipient.Public as ECPublicKeyParameters,
                Amount = 0,
                TransactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>(),
                TransactionInput = _transactionInput
            };
        }

        [TestMethod]
        public void TransactionToStringReturnsValid()
        {
            var result = _transaction.ToString();

            var expectedResult = _transaction.Id + _transaction.Sender.ToString() + _transaction.Recipient + _transaction.Amount + _transaction.TransactionOutputs + _transaction.TransactionInput;

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TransactionsAreEqual()
        {
            var sameObject = (object)_transaction;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_transaction.Equals(sameObject));
        }

        [TestMethod]
        public void TransactionsAreNotEqualDifferentProperties()
        {
            var keyPairSender = KeyPairUtils.GenerateKeyPair();
            var keyPairRecipient = KeyPairUtils.GenerateKeyPair();

            var differentTransaction = new Transaction
            {
                Sender = keyPairSender.Public as ECPublicKeyParameters,
                Recipient = keyPairRecipient.Public as ECPublicKeyParameters,
                Amount = 9.9m,
                TransactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>(),
                TransactionInput = _transactionInput
            };

            var differentObject = (object)differentTransaction;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_transaction.Equals(differentObject));
        }

        [TestMethod]
        public void TransactionAndObjectAreNotEqual()
        {
            var differentObject = new object();

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_transaction.Equals(differentObject));
        }

        [TestMethod]
        public void TransactionAndNullAreNotEqual()
        {
            object differentObject = null;

            Assert.IsNull(differentObject);
            Assert.IsFalse(_transaction.Equals(differentObject));
        }

        [TestMethod]
        public void TransactionsHaveSameHashCode()
        {
            var sameTransaction = _transaction;

            Assert.IsNotNull(sameTransaction);
            Assert.IsTrue(_transaction.GetHashCode() == sameTransaction.GetHashCode());
        }

        [TestMethod]
        public void TransactionsDoNotHaveSameHashCode()
        {
            var keyPairSender = KeyPairUtils.GenerateKeyPair();
            var keyPairRecipient = KeyPairUtils.GenerateKeyPair();

            var differentTransaction = new Transaction
            {
                Sender = keyPairSender.Public as ECPublicKeyParameters,
                Recipient = keyPairRecipient.Public as ECPublicKeyParameters,
                Amount = 9.9m,
                TransactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>(),
                TransactionInput = _transactionInput
            };

            Assert.IsNotNull(differentTransaction);
            Assert.IsFalse(_transaction.GetHashCode() == differentTransaction.GetHashCode());
        }
    }
}