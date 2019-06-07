using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Extensions
{
    [TestClass]
    public class TransactionExtensionsTests
    {
        private Wallet _wallet;

        private ECPublicKeyParameters _recipientPublicKey;

        private Transaction _transaction;

        private readonly decimal _amount = 100;

        [TestInitialize]
        public void TransactionExtensionsTestsSetup()
        {
            var senderKeyPair = KeyPairUtils.GenerateKeyPair();

            _wallet = new Wallet
            {
                Balance = Constants.StartBalance,
                PrivateKey = senderKeyPair.Private as ECPrivateKeyParameters,
                PublicKey = senderKeyPair.Public as ECPublicKeyParameters
            };

            var recipientKeyPair = KeyPairUtils.GenerateKeyPair();

            _recipientPublicKey = recipientKeyPair.Public as ECPublicKeyParameters;

            var transactionOutputs = TransactionUtils.GenerateTransactionOutput(_wallet, _recipientPublicKey, _amount);

            var transactionInput = TransactionUtils.GenerateTransactionInput(_wallet, transactionOutputs);

            _transaction = new Transaction
            {
                TransactionInput = transactionInput,
                TransactionOutputs = transactionOutputs
            };
        }

        [TestMethod]
        public void ConstructsTransaction()
        {
            Assert.IsNotNull(_transaction.Id);
            Assert.IsInstanceOfType(_transaction.Id, typeof(Guid));
            Assert.IsNotNull(_transaction.TransactionOutputs);
            Assert.IsInstanceOfType(_transaction.TransactionOutputs, typeof(Dictionary<ECPublicKeyParameters, decimal>));
            Assert.IsNotNull(_transaction.TransactionInput);
            Assert.IsInstanceOfType(_transaction.TransactionInput, typeof(TransactionInput));
            Assert.AreEqual(_amount, _transaction.TransactionOutputs[_recipientPublicKey]);
            Assert.AreEqual(_wallet.Balance - _amount, _transaction.TransactionOutputs[_wallet.PublicKey]);
        }

        [TestMethod]
        public void ValidatesValidTransaction()
        {
            /*

            var transaction = WalletUtils.GenerateTransaction(senderWallet, recipientPublicKey, 100, _replacementBlockchain);

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(senderWallet);

            var transactions = new List<Transaction>()
            {
                transaction,
                minerRewardTransaction
            };

            _replacementBlockchain.AddBlock(transactions);

            Assert.IsTrue(_replacementBlockchain.IsValidTransactionData());*/
        }

        [TestMethod]
        public void ValidatesInvalidTransaction()
        {
        }

        //ISVALIDTRANSACTION
        //UPDATETRANSACTION
    }
}