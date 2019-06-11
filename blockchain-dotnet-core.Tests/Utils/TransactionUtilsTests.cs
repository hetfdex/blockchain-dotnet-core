using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class TransactionUtilsTests
    {
        [TestInitialize]
        public void TransactionUtilsTestsSetup()
        {
        }

        [TestMethod]
        public void GetsMinerRewardTransaction()
        {
            var minerKeyPair = KeyPairUtils.GenerateKeyPair();

            var minerWallet = new Wallet
            {
                Balance = Constants.StartBalance,
                PrivateKey = minerKeyPair.Private as ECPrivateKeyParameters,
                PublicKey = minerKeyPair.Public as ECPublicKeyParameters
            };

            var rewardTransaction = TransactionUtils.GetMinerRewardTransaction(minerWallet);

            Assert.AreEqual(Constants.MinerTransactionInput, rewardTransaction.TransactionInput);
            Assert.AreEqual(Constants.MinerTransactionInput.Amount, rewardTransaction.TransactionOutputs[minerWallet.PublicKey]);
        }
    }
}