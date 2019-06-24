using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class WalletTests
    {
        private readonly decimal _balance = 0;

        private ECPrivateKeyParameters _privateKey;

        private ECPublicKeyParameters _publicKey;

        private Wallet _wallet;

        [TestInitialize]
        public void WalletTestsSetup()
        {
            var keyPair = CryptoUtils.GenerateKeyPair();

            _privateKey = keyPair.Private as ECPrivateKeyParameters;

            _publicKey = keyPair.Public as ECPublicKeyParameters;

            _wallet = new Wallet(_privateKey, _publicKey, _balance);
        }

        [TestMethod]
        public void ConstructsWallet()
        {
            Assert.IsNotNull(_wallet);
            Assert.IsInstanceOfType(_wallet, typeof(Wallet));
            Assert.AreEqual(_privateKey, _wallet.PrivateKey);
            Assert.AreEqual(_publicKey, _wallet.PublicKey);
            Assert.AreEqual(_balance, _wallet.Balance);
        }

        [TestMethod]
        public void WalletsAreEqual()
        {
            var sameObject = (object) _wallet;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_wallet.Equals(sameObject));
        }

        [TestMethod]
        public void WalletsAreNotEqualDifferentProperties()
        {
            var keyPair = CryptoUtils.GenerateKeyPair();

            var differentWallet = new Wallet(keyPair.Private as ECPrivateKeyParameters,
                keyPair.Public as ECPublicKeyParameters, 10);

            var differentObject = (object) differentWallet;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_wallet.Equals(differentObject));
        }

        [TestMethod]
        public void WalletAndNullAreNotEqual()
        {
            Assert.IsFalse(_wallet.Equals((object) null));
        }
    }
}