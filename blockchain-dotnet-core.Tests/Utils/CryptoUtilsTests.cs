using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class CryptoUtilsTests
    {
        private AsymmetricCipherKeyPair _keyPair;

        private ECPrivateKeyParameters _privateKey;

        private ECPublicKeyParameters _publicKey;

        [TestInitialize]
        public void KeyPairUtilsTestsSetup()
        {
            _keyPair = CryptoUtils.GenerateKeyPair();

            _privateKey = _keyPair.Private as ECPrivateKeyParameters;

            _publicKey = _keyPair.Public as ECPublicKeyParameters;
        }

        [TestMethod]
        public void GeneratesKeyPair()
        {
            Assert.IsNotNull(_keyPair);
            Assert.IsNotNull(_privateKey);
            Assert.IsNotNull(_publicKey);
        }

        [TestMethod]
        public void LoadsPublicKey()
        {
            var result = CryptoUtils.LoadPublicKey(
                "MIIBMzCB7AYHKoZIzj0CATCB4AIBATAsBgcqhkjOPQEBAiEA/////////////////////////////////////v///C8wRAQgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHBEEEeb5mfvncu6xVoGKVzocLBwKb/NstzijZWfKBWxb4F5hIOtp3JqPEZV2k+/wOEQio/Re0SKaFVBmcR9CP+xDUuAIhAP////////////////////66rtzmr0igO7/SXozQNkFBAgEBA0IABG+Iu+O3FJGQhZHBUVn+4/EEw41r13myLyTRqZfeklWN/VIiUjE5WC574vIV9tYErJf/tE2h51rH/5KB246NRfg=");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ECPublicKeyParameters));
        }

        [TestMethod]
        public void GeneratesSignature()
        {
            var bytes = HashUtils.ComputeHash("hetfdex");

            var signature = CryptoUtils.GenerateSignature(_privateKey, bytes);

            Assert.IsNotNull(signature);
        }

        [TestMethod]
        public void VerifiesSignature()
        {
            var bytes = HashUtils.ComputeHash("hetfdex");

            var signature = CryptoUtils.GenerateSignature(_privateKey, bytes);

            Assert.IsTrue(CryptoUtils.VerifySignature(_publicKey, bytes, signature));
        }

        [TestMethod]
        public void LoadPublicKeyNullReturnsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                CryptoUtils.LoadPublicKey(null));
        }

        [TestMethod]
        public void GenerateSignatureNullPrivateKeyReturnsException()
        {
            var bytes = HashUtils.ComputeHash("hetfdex");

            Assert.ThrowsException<ArgumentNullException>(() =>
                CryptoUtils.GenerateSignature(null, bytes));
        }

        [TestMethod]
        public void GenerateSignatureNullBytesReturnsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                CryptoUtils.GenerateSignature(_privateKey, null));
        }

        [TestMethod]
        public void VerifySignatureNullPublicKeyReturnsException()
        {
            var bytes = HashUtils.ComputeHash("hetfdex");

            var signature = CryptoUtils.GenerateSignature(_privateKey, bytes);

            Assert.ThrowsException<ArgumentNullException>(() =>
                CryptoUtils.VerifySignature(null, bytes, signature));
        }

        [TestMethod]
        public void VerifySignatureNullBytesReturnsException()
        {
            var bytes = HashUtils.ComputeHash("hetfdex");

            var signature = CryptoUtils.GenerateSignature(_privateKey, bytes);

            Assert.ThrowsException<ArgumentNullException>(() =>
                CryptoUtils.VerifySignature(_publicKey, null, signature));
        }

        [TestMethod]
        public void VerifySignatureNullSignatureReturnsException()
        {
            var bytes = HashUtils.ComputeHash("hetfdex");

            Assert.ThrowsException<ArgumentNullException>(() =>
                CryptoUtils.VerifySignature(_publicKey, bytes, null));
        }
    }
}