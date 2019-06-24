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
        private readonly long _timestamp = TimestampUtils.GenerateTimestamp();

        private readonly decimal _amount = 0;

        private readonly string _signature = "test-signature";

        private readonly IDictionary<ECPublicKeyParameters, decimal> _transactionOutputs =
            new Dictionary<ECPublicKeyParameters, decimal>();

        private ECPublicKeyParameters _address;

        private TransactionInput _transactionInput;

        private Transaction _transaction;

        [TestInitialize]
        public void TransactionTestsSetup()
        {
            var keyPair = CryptoUtils.GenerateKeyPair();

            _address = keyPair.Public as ECPublicKeyParameters;

            _transactionInput = new TransactionInput(_timestamp, _address, _amount, _signature);

            _transaction = new Transaction(_transactionOutputs, _transactionInput);
        }

        [TestMethod]
        public void ConstructsTransaction()
        {
            Assert.IsNotNull(_transaction);
            Assert.AreEqual(_transactionOutputs, _transaction.TransactionOutputs);
            Assert.AreEqual(_transactionInput, _transaction.TransactionInput);
        }

        [TestMethod]
        public void TransactionsAreEqual()
        {
            var sameObject = (object) _transaction;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_transaction.Equals(sameObject));
        }

        [TestMethod]
        public void TransactionsAreNotEqualDifferentProperties()
        {
            var differentTransaction =
                new Transaction(new Dictionary<ECPublicKeyParameters, decimal>(), _transactionInput);

            var differentObject = (object) differentTransaction;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_transaction.Equals(differentObject));
        }

        [TestMethod]
        public void TransactionAndNullAreNotEqual()
        {
            Assert.IsFalse(_transaction.Equals((object) null));
        }
    }
}