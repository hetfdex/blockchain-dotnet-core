using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class TransactionInputTests
    {
        private readonly long _timestamp = TimestampUtils.GenerateTimestamp();

        private readonly decimal _amount = 0;

        private readonly string _signature = "test-signature";

        private ECPublicKeyParameters _address;

        private TransactionInput _transactionInput;

        [TestInitialize]
        public void TransactionInputTestsSetup()
        {
            var keyPair = KeyPairUtils.GenerateKeyPair();

            _address = keyPair.Public as ECPublicKeyParameters;

            _transactionInput = new TransactionInput(_timestamp, _address, _amount, _signature);
        }

        [TestMethod]
        public void ConstructsTransactionInput()
        {
            Assert.IsNotNull(_transactionInput);
            Assert.IsInstanceOfType(_transactionInput, typeof(TransactionInput));
            Assert.AreEqual(_timestamp, _transactionInput.Timestamp);
            Assert.AreEqual(_address, _transactionInput.Address);
            Assert.AreEqual(_amount, _transactionInput.Amount);
            Assert.AreEqual(_signature, _transactionInput.Signature);
        }

        [TestMethod]
        public void TransactionInputToStringReturnsValid()
        {
            var result = _transactionInput.ToString();

            var expectedResult = _transactionInput.Timestamp + _transactionInput.Address.ToString() + _transactionInput.Amount + _transactionInput.Signature;

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TransactionInputsAreEqual()
        {
            var sameObject = (object)_transactionInput;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_transactionInput.Equals(sameObject));
        }

        [TestMethod]
        public void TransactionInputsAreNotEqual()
        {
            var keyPair = KeyPairUtils.GenerateKeyPair();

            var differentTransactionInput = new TransactionInput(0, keyPair.Public as ECPublicKeyParameters, 10, string.Empty);

            var differentObject = (object)differentTransactionInput;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_transactionInput.Equals(differentObject));
        }

        [TestMethod]
        public void TransactionInputAndObjectAreNotEqual()
        {
            var differentObject = new object();

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_transactionInput.Equals(differentObject));
        }

        [TestMethod]
        public void TransactionInputAndNullAreNotEqual()
        {
            Assert.IsFalse(_transactionInput.Equals((object)null));
        }

        [TestMethod]
        public void TransactionInputsHaveSameHashCode()
        {
            var sameTransactionInput = _transactionInput;

            Assert.IsNotNull(sameTransactionInput);
            Assert.IsTrue(_transactionInput.GetHashCode() == sameTransactionInput.GetHashCode());
        }

        [TestMethod]
        public void TransactionInputsDoNotHaveSameHashCode()
        {
            var keyPair = KeyPairUtils.GenerateKeyPair();

            var differentTransactionInput = new TransactionInput(0, keyPair.Public as ECPublicKeyParameters, 10, string.Empty);

            Assert.IsNotNull(differentTransactionInput);
            Assert.IsFalse(_transactionInput.GetHashCode() == differentTransactionInput.GetHashCode());
        }
    }
}