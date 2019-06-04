using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class WalletTests
    {
        private Wallet _wallet;

        [TestInitialize]
        public void WalletTestsSetup()
        {
            var keyPair = KeyPairUtils.GenerateKeyPair();

            _wallet = new Wallet
            {
                Balance = 0,
                KeyPair = keyPair,
                PublicKey = keyPair.Public as ECPublicKeyParameters
            };
        }

        [TestMethod]
        public void WalletToStringReturnsValid()
        {
            var result = _wallet.ToString();

            var expectedResult = _wallet.Balance + _wallet.KeyPair.ToString() + _wallet.PublicKey;

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void WalletsAreEqual()
        {
            var sameObject = (object)_wallet;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_wallet.Equals(sameObject));
        }

        [TestMethod]
        public void WalletsAreNotEqualDifferentProperties()
        {
            var keyPair = KeyPairUtils.GenerateKeyPair();

            var differentWallet = new Wallet
            {
                Balance = 0,
                KeyPair = keyPair,
                PublicKey = keyPair.Public as ECPublicKeyParameters
            };

            var differentObject = (object)differentWallet;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_wallet.Equals(differentObject));
        }

        [TestMethod]
        public void WalletAndObjectAreNotEqual()
        {
            var differentObject = new object();

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_wallet.Equals(differentObject));
        }

        [TestMethod]
        public void WalletAndNullAreNotEqual()
        {
            Assert.IsFalse(_wallet.Equals((object)null));
        }

        [TestMethod]
        public void WalletsHaveSameHashCode()
        {
            var sameWallet = _wallet;

            Assert.IsNotNull(sameWallet);
            Assert.IsTrue(_wallet.GetHashCode() == sameWallet.GetHashCode());
        }

        [TestMethod]
        public void WalletsDoNotHaveSameHashCode()
        {
            var keyPair = KeyPairUtils.GenerateKeyPair();

            var differentWallet = new Wallet
            {
                Balance = 0,
                KeyPair = keyPair,
                PublicKey = keyPair.Public as ECPublicKeyParameters
            };

            Assert.IsNotNull(differentWallet);
            Assert.IsFalse(_wallet.GetHashCode() == differentWallet.GetHashCode());
        }
    }
}