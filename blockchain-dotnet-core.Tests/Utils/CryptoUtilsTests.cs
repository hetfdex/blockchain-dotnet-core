using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

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
    }
}