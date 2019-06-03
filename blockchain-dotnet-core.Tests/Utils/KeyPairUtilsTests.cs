using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class KeyPairUtilsTests
    {
        private AsymmetricCipherKeyPair _keyPair;

        private ECPrivateKeyParameters _privateKey;

        private ECPublicKeyParameters _publicKey;

        [TestInitialize]
        public void KeyPairUtilsTestsSetup()
        {
            _keyPair = KeyPairUtils.GenerateKeyPair();

            _privateKey = _keyPair.Private as ECPrivateKeyParameters;

            _publicKey = _keyPair.Public as ECPublicKeyParameters;
        }

        [TestMethod]
        public void GeneratesKeyPair()
        {
            Assert.IsNotNull(_keyPair);
            Assert.IsNotNull(_privateKey);
            Assert.IsNotNull(_publicKey);
            Assert.IsInstanceOfType(_keyPair, typeof(AsymmetricCipherKeyPair));
            Assert.IsInstanceOfType(_privateKey, typeof(ECPrivateKeyParameters));
            Assert.IsInstanceOfType(_publicKey, typeof(ECPublicKeyParameters));
        }

        [TestMethod]
        public void GeneratesSignature()
        {
            var keyPair = KeyPairUtils.GenerateKeyPair();

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var publicKey = keyPair.Public as ECPublicKeyParameters;

            transactionOutputs.Add(publicKey, 10);

            var signature = KeyPairUtils.GenerateSignature(_privateKey, transactionOutputs);

            Assert.IsNotNull(signature);
            Assert.IsInstanceOfType(signature, typeof(string));
        }

        [TestMethod]
        public void VerifiesSignature()
        {
            var keyPair = KeyPairUtils.GenerateKeyPair();

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var publicKey = keyPair.Public as ECPublicKeyParameters;

            transactionOutputs.Add(publicKey, 10);

            var signature = KeyPairUtils.GenerateSignature(_privateKey, transactionOutputs);

            Assert.IsTrue(KeyPairUtils.VerifySignature(_publicKey, transactionOutputs, signature));
        }
    }
}