using blockchain_dotnet_core.API;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class WalletTests
    {
        private readonly decimal _balance = Constants.StartBalance;

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
        public void ConstructWalletNoParameters()
        {
            var wallet = new Wallet();

            Assert.IsNotNull(wallet);
            Assert.IsNotNull(wallet.PrivateKey);
            Assert.IsNotNull(wallet.PublicKey);
            Assert.AreEqual(Constants.StartBalance, wallet.Balance);
        }

        [TestMethod]
        public void ConstructWallet()
        {
            Assert.IsNotNull(_wallet);
            Assert.AreEqual(_privateKey, _wallet.PrivateKey);
            Assert.AreEqual(_publicKey, _wallet.PublicKey);
            Assert.AreEqual(_balance, _wallet.Balance);
        }

        [TestMethod]
        public void ConstructWalletNullPrivateKeyThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Wallet(null, _publicKey, _balance));
        }

        [TestMethod]
        public void ConstructWalletNullPublicThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Wallet(_privateKey, null, _balance));
        }

        [TestMethod]
        public void VerifySignature()
        {
            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var signature = CryptoUtils.GenerateSignature(_wallet.PrivateKey, transactionOutputs.ToHash());

            Assert.IsTrue(CryptoUtils.VerifySignature(_wallet.PublicKey, transactionOutputs.ToHash(), signature));
        }

        [TestMethod]
        public void VerifySignatureWrongWallet()
        {
            var wallet = new Wallet();

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var signature = CryptoUtils.GenerateSignature(wallet.PrivateKey, transactionOutputs.ToHash());

            Assert.IsFalse(CryptoUtils.VerifySignature(_wallet.PublicKey, transactionOutputs.ToHash(), signature));
        }

        [TestMethod]
        public void GenerateTransaction()
        {
            var blockchain = new Blockchain();

            var recipientWallet = new Wallet();

            var amount = 100m;

            var transaction = _wallet.GenerateTransaction(recipientWallet.PublicKey, amount, blockchain);

            Assert.IsNotNull(transaction);
            Assert.AreEqual(_wallet.PublicKey, transaction.TransactionInput.Address);
            Assert.AreEqual(amount, transaction.TransactionOutputs[recipientWallet.PublicKey]);
        }

        [TestMethod]
        public void GenerateTransactionInvalidTransaction()
        {
            var blockchain = new Blockchain();

            var recipientWallet = new Wallet();

            var amount = 9999M;

            var transaction = _wallet.GenerateTransaction(recipientWallet.PublicKey, amount, blockchain);

            Assert.IsNull(transaction);
        }

        [TestMethod]
        public void GenerateTransactionNullRecipientThrowsException()
        {
            var blockchain = new Blockchain();

            var amount = 9999M;

            Assert.ThrowsException<ArgumentNullException>(() => _wallet.GenerateTransaction(null, amount, blockchain));
        }

        [TestMethod]
        public void CalculateBalanceWithoutOutputs()
        {
            var blockchain = new Blockchain();

            var balance = Wallet.CalculateBalance(blockchain, _wallet.PublicKey);

            Assert.AreEqual(Constants.StartBalance, balance);
        }

        [TestMethod]
        public void CalculatesBalanceWithOutputs()
        {
            var blockchain = new Blockchain();

            var recipientWallet = new Wallet();

            var transactionOne = _wallet.GenerateTransaction(recipientWallet.PublicKey, 100, blockchain);

            var transactionTwo = _wallet.GenerateTransaction(recipientWallet.PublicKey, 50, blockchain);

            var transactions = new List<Transaction>
            {
                transactionOne,
                transactionTwo
            };

            blockchain.AddBlock(transactions);

            var balance = Wallet.CalculateBalance(blockchain, recipientWallet.PublicKey);

            var expectedBalance = Constants.StartBalance +
                                  transactionOne.TransactionOutputs[recipientWallet.PublicKey] +
                                  transactionTwo.TransactionOutputs[recipientWallet.PublicKey];

            Assert.AreEqual(expectedBalance, balance);
        }

        [TestMethod]
        public void CalculatesBalanceWithPreviousTransactions()
        {
            var blockchain = new Blockchain();

            var recipientWallet = new Wallet();

            var transactionOne = _wallet.GenerateTransaction(recipientWallet.PublicKey, 100, blockchain);

            var transactionTwo = _wallet.GenerateTransaction(recipientWallet.PublicKey, 50, blockchain);

            var transactions = new List<Transaction>
            {
                transactionOne,
                transactionTwo
            };

            blockchain.AddBlock(transactions);

            var transaction = _wallet.GenerateTransaction(recipientWallet.PublicKey, 100, blockchain);

            transactions = new List<Transaction>
            {
                transaction
            };

            blockchain.AddBlock(transactions);

            var balance = Wallet.CalculateBalance(blockchain, _wallet.PublicKey);

            var expectedBalance = transaction.TransactionOutputs[_wallet.PublicKey];

            Assert.AreEqual(expectedBalance, balance);

            transaction = _wallet.GenerateTransaction(recipientWallet.PublicKey, 100, blockchain);

            var minerRewardTransaction = Transaction.GetMinerRewardTransaction(_wallet);

            expectedBalance = transaction.TransactionOutputs[_wallet.PublicKey] +
                              minerRewardTransaction.TransactionOutputs[_wallet.PublicKey];

            transactions = new List<Transaction>
            {
                transaction,
                minerRewardTransaction
            };

            blockchain.AddBlock(transactions);

            var senderWallet = new Wallet();

            transaction = senderWallet.GenerateTransaction(_wallet.PublicKey, 100, blockchain);

            transactions = new List<Transaction>
            {
                transaction
            };

            blockchain.AddBlock(transactions);

            balance = Wallet.CalculateBalance(blockchain, _wallet.PublicKey);

            expectedBalance += transaction.TransactionOutputs[_wallet.PublicKey];

            Assert.AreEqual(expectedBalance, balance);
        }

        [TestMethod]
        public void CalculateBalanceNullBlockchainThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Wallet.CalculateBalance(null, _wallet.PublicKey));
        }

        [TestMethod]
        public void CalculateBalanceNullAddressThrowsException()
        {
            var blockchain = new Blockchain();

            Assert.ThrowsException<ArgumentNullException>(() => Wallet.CalculateBalance(blockchain, null));
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
            var keyPair = CryptoUtils.GenerateKeyPair();

            var differentWallet = new Wallet(keyPair.Private as ECPrivateKeyParameters,
                keyPair.Public as ECPublicKeyParameters, 10);

            var differentObject = (object)differentWallet;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_wallet.Equals(differentObject));
        }

        [TestMethod]
        public void WalletAndNullAreNotEqual()
        {
            Assert.IsFalse(_wallet.Equals((object)null));
        }
    }
}