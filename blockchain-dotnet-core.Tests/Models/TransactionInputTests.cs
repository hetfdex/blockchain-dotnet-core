using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class TransactionInputTests
    {
        private TransactionInput _transactionInput;

        [TestInitialize]
        public void TransactionInputTestsSetup()
        {
            var keyPair = KeyPairUtils.GenerateKeyPair();

            _transactionInput = new TransactionInput
            {
                Timestamp = TimestampUtils.GenerateTimestamp(),
                Address = keyPair.Public as ECPublicKeyParameters,
                Amount = 0,
                Signature = "test-signature"
            };
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
        public void TransactionInputsAreNotEqualDifferentProperties()
        {
            var keyPair = KeyPairUtils.GenerateKeyPair();

            var differentTransactionInput = new TransactionInput
            {
                Timestamp = 0L,
                Address = keyPair.Public as ECPublicKeyParameters,
                Amount = 9.9m,
                Signature = "fake-signature"
            };

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

            var differentTransactionInput = new TransactionInput
            {
                Timestamp = 0L,
                Address = keyPair.Public as ECPublicKeyParameters,
                Amount = 9.9m,
                Signature = "fake-signature"
            };

            Assert.IsNotNull(differentTransactionInput);
            Assert.IsFalse(_transactionInput.GetHashCode() == differentTransactionInput.GetHashCode());
        }
    }
}