using blockchain_dotnet_core.API.Extensions;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Options;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class TransactionUtilsTests
    {
        private Wallet _senderWallet;

        private ECPublicKeyParameters _recipientPublicKey;

        private Transaction _transaction;

        private readonly decimal _amount = 100;

        [TestInitialize]
        public void TransactionUtilsTestsSetup()
        {
            var senderKeyPair = KeyPairUtils.GenerateKeyPair();

            _senderWallet = new Wallet(senderKeyPair.Private as ECPrivateKeyParameters,
                senderKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var recipientKeyPair = KeyPairUtils.GenerateKeyPair();

            _recipientPublicKey = recipientKeyPair.Public as ECPublicKeyParameters;

            var transactionOutputs =
                TransactionUtils.GenerateTransactionOutput(_senderWallet, _recipientPublicKey, _amount);

            var transactionInput = TransactionUtils.GenerateTransactionInput(_senderWallet, transactionOutputs);

            _transaction = new Transaction(transactionOutputs, transactionInput);
        }

        [TestMethod]
        public void GetsMinerRewardTransaction()
        {
            var minerKeyPair = KeyPairUtils.GenerateKeyPair();

            var minerWallet = new Wallet(minerKeyPair.Private as ECPrivateKeyParameters,
                minerKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var rewardTransaction = TransactionUtils.GetMinerRewardTransaction(minerWallet);

            Assert.AreEqual(TransactionInputUtils.GetMinerTransactionInput(), rewardTransaction.TransactionInput);
            Assert.AreEqual(TransactionInputUtils.GetMinerTransactionInput().Amount,
                rewardTransaction.TransactionOutputs[minerWallet.PublicKey]);
        }

        [TestMethod]
        public void GeneratesValidTransactionOuputs()
        {
            Assert.AreEqual(_amount, _transaction.TransactionOutputs[_recipientPublicKey]);
            Assert.AreEqual(_senderWallet.Balance - _amount, _transaction.TransactionOutputs[_senderWallet.PublicKey]);
        }

        [TestMethod]
        public void GeneratesValidTransactionInput()
        {
            Assert.IsNotNull(_transaction.TransactionInput.Timestamp);
            Assert.IsInstanceOfType(_transaction.TransactionInput.Timestamp, typeof(long));
            Assert.IsNotNull(_transaction.TransactionInput.Address);
            Assert.IsInstanceOfType(_transaction.TransactionInput.Address, typeof(ECPublicKeyParameters));
            Assert.IsNotNull(_transaction.TransactionInput.Amount);
            Assert.IsInstanceOfType(_transaction.TransactionInput.Amount, typeof(decimal));
            Assert.IsNotNull(_transaction.TransactionInput.Signature);
            Assert.IsInstanceOfType(_transaction.TransactionInput.Signature, typeof(string));
            Assert.AreEqual(_senderWallet.PublicKey, _transaction.TransactionInput.Address);
            Assert.AreEqual(_senderWallet.Balance, _transaction.TransactionInput.Amount);
            Assert.IsTrue(KeyPairUtils.VerifySignature(_senderWallet.PublicKey,
                _transaction.TransactionOutputs.ToHash(), _transaction.TransactionInput.Signature));
        }
    }
}