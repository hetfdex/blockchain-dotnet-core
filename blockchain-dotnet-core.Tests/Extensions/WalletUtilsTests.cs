﻿using blockchain_dotnet_core.API.Extensions;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Options;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class WalletUtilsTests
    {
        private Wallet _wallet;

        [TestInitialize]
        public void WalletUtilsTestsSetup()
        {
            var keyPair = CryptoUtils.GenerateKeyPair();

            _wallet = new Wallet(keyPair.Private as ECPrivateKeyParameters, keyPair.Public as ECPublicKeyParameters,
                ConfigurationOptions.StartBalance);
        }

        [TestMethod]
        public void ConstructsWallet()
        {
            Assert.IsNotNull(_wallet.Balance);
            Assert.IsInstanceOfType(_wallet.Balance, typeof(decimal));
            Assert.AreEqual(ConfigurationOptions.StartBalance, _wallet.Balance);
            Assert.IsNotNull(_wallet.PrivateKey);
            Assert.IsInstanceOfType(_wallet.PrivateKey, typeof(ECPrivateKeyParameters));
            Assert.IsNotNull(_wallet.PublicKey);
            Assert.IsInstanceOfType(_wallet.PublicKey, typeof(ECPublicKeyParameters));
        }

        [TestMethod]
        public void VerifiesValidSignature()
        {
            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var signature = CryptoUtils.GenerateSignature(_wallet.PrivateKey, transactionOutputs.ToHash());

            Assert.IsTrue(CryptoUtils.VerifySignature(_wallet.PublicKey, transactionOutputs.ToHash(), signature));
        }

        [TestMethod]
        public void DoesNotVerifyInvalidSignature()
        {
            var keyPair = CryptoUtils.GenerateKeyPair();

            var wallet = new Wallet(keyPair.Private as ECPrivateKeyParameters, keyPair.Public as ECPublicKeyParameters,
                ConfigurationOptions.StartBalance);

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var signature = CryptoUtils.GenerateSignature(wallet.PrivateKey, transactionOutputs.ToHash());

            Assert.IsFalse(CryptoUtils.VerifySignature(_wallet.PublicKey, transactionOutputs.ToHash(), signature));
        }

        [TestMethod]
        public void GeneratesValidTransaction()
        {
            var blockchain = new Blockchain();

            var keyPair = CryptoUtils.GenerateKeyPair();

            var publicKey = keyPair.Public as ECPublicKeyParameters;

            var amount = 100M;

            var transaction = WalletUtils.GenerateTransaction(_wallet, publicKey, amount, blockchain);

            Assert.IsNotNull(transaction);
            Assert.IsInstanceOfType(transaction, typeof(Transaction));
            Assert.AreEqual(_wallet.PublicKey, transaction.TransactionInput.Address);
            Assert.AreEqual(amount, transaction.TransactionOutputs[publicKey]);
        }

        [TestMethod]
        public void DoesNotGenerateInvalidTransaction()
        {
            var blockchain = new Blockchain();

            var keyPair = CryptoUtils.GenerateKeyPair();

            var publicKey = keyPair.Public as ECPublicKeyParameters;

            var amount = 9999M;

            var transaction = WalletUtils.GenerateTransaction(_wallet, publicKey, amount, blockchain);

            Assert.IsNull(transaction);
        }

        [TestMethod]
        public void CalculatesBalanceWithoutOutputs()
        {
            var blockchain = new Blockchain();

            var balance = WalletUtils.CalculateBalance(blockchain, _wallet.PublicKey);

            Assert.AreEqual(ConfigurationOptions.StartBalance, balance);
        }

        [TestMethod]
        public void CalculatesBalanceWithOutputs()
        {
            var blockchain = new Blockchain();

            var keyPair = CryptoUtils.GenerateKeyPair();

            var publicKey = keyPair.Public as ECPublicKeyParameters;

            var transactionOne = WalletUtils.GenerateTransaction(_wallet, publicKey, 100, blockchain);

            var transactionTwo = WalletUtils.GenerateTransaction(_wallet, publicKey, 50, blockchain);

            var transactions = new List<Transaction>
            {
                transactionOne,
                transactionTwo
            };

            blockchain.AddBlock(transactions);

            var balance = WalletUtils.CalculateBalance(blockchain, publicKey);

            var expectedBalance = ConfigurationOptions.StartBalance + transactionOne.TransactionOutputs[publicKey] +
                                  transactionTwo.TransactionOutputs[publicKey];

            Assert.AreEqual(expectedBalance, balance);
        }

        [TestMethod]
        public void CalculatesBalanceWithPreviousTransactions()
        {
            var blockchain = new Blockchain();

            var keyPair = CryptoUtils.GenerateKeyPair();

            var wallet = new Wallet(keyPair.Private as ECPrivateKeyParameters, keyPair.Public as ECPublicKeyParameters,
                ConfigurationOptions.StartBalance);

            var transactionOne = WalletUtils.GenerateTransaction(_wallet, wallet.PublicKey, 100, blockchain);

            var transactionTwo = WalletUtils.GenerateTransaction(_wallet, wallet.PublicKey, 50, blockchain);

            var transactions = new List<Transaction>
            {
                transactionOne,
                transactionTwo
            };

            blockchain.AddBlock(transactions);

            var transaction = WalletUtils.GenerateTransaction(_wallet, wallet.PublicKey, 100, blockchain);

            transactions = new List<Transaction>
            {
                transaction
            };

            blockchain.AddBlock(transactions);

            var balance = WalletUtils.CalculateBalance(blockchain, _wallet.PublicKey);

            var expectedBalance = transaction.TransactionOutputs[_wallet.PublicKey];

            Assert.AreEqual(expectedBalance, balance);

            transaction = WalletUtils.GenerateTransaction(_wallet, wallet.PublicKey, 100, blockchain);

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(_wallet);

            expectedBalance = transaction.TransactionOutputs[_wallet.PublicKey] +
                              minerRewardTransaction.TransactionOutputs[_wallet.PublicKey];

            transactions = new List<Transaction>
            {
                transaction,
                minerRewardTransaction
            };

            blockchain.AddBlock(transactions);

            keyPair = CryptoUtils.GenerateKeyPair();

            wallet = new Wallet(keyPair.Private as ECPrivateKeyParameters, keyPair.Public as ECPublicKeyParameters,
                ConfigurationOptions.StartBalance);

            transaction = WalletUtils.GenerateTransaction(wallet, _wallet.PublicKey, 100, blockchain);

            transactions = new List<Transaction>
            {
                transaction
            };

            blockchain.AddBlock(transactions);

            balance = WalletUtils.CalculateBalance(blockchain, _wallet.PublicKey);

            expectedBalance += transaction.TransactionOutputs[_wallet.PublicKey];

            Assert.AreEqual(expectedBalance, balance);
        }
    }
}