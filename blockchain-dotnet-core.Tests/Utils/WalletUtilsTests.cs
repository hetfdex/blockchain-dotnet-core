using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using blockchain_dotnet_core.API.Extensions;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class WalletUtilsTests
    {
        private Wallet _wallet;

        [TestInitialize]
        public void WalletUtilsTestsSetup()
        {
            var keyPair = KeyPairUtils.GenerateKeyPair();

            _wallet = new Wallet
            {
                Balance = Constants.StartBalance,
                PrivateKey = keyPair.Private as ECPrivateKeyParameters,
                PublicKey = keyPair.Public as ECPublicKeyParameters
            };
        }

        [TestMethod]
        public void ConstructsWallet()
        {
            Assert.IsNotNull(_wallet.Balance);
            Assert.IsInstanceOfType(_wallet.Balance, typeof(decimal));
            Assert.AreEqual(Constants.StartBalance, _wallet.Balance);
            Assert.IsNotNull(_wallet.PrivateKey);
            Assert.IsInstanceOfType(_wallet.PrivateKey, typeof(ECPrivateKeyParameters));
            Assert.IsNotNull(_wallet.PublicKey);
            Assert.IsInstanceOfType(_wallet.PublicKey, typeof(ECPublicKeyParameters));
        }

        [TestMethod]
        public void VerifiesValidSignature()
        {
            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var signature = KeyPairUtils.GenerateSignature(_wallet.PrivateKey, transactionOutputs);

            Assert.IsTrue(KeyPairUtils.VerifySignature(_wallet.PublicKey, transactionOutputs, signature));
        }

        [TestMethod]
        public void DoesNotVerifyInvalidSignature()
        {
            var keyPair = KeyPairUtils.GenerateKeyPair();

            var wallet = new Wallet
            {
                Balance = Constants.StartBalance,
                PrivateKey = keyPair.Private as ECPrivateKeyParameters,
                PublicKey = keyPair.Public as ECPublicKeyParameters
            };

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var signature = KeyPairUtils.GenerateSignature(wallet.PrivateKey, transactionOutputs);

            Assert.IsFalse(KeyPairUtils.VerifySignature(_wallet.PublicKey, transactionOutputs, signature));
        }

        [TestMethod]
        public void GeneratesValidTransaction()
        {
            var blockchain = new List<Block>();

            blockchain.Add(BlockUtils.GetGenesisBlock());

            var keyPair = KeyPairUtils.GenerateKeyPair();

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
            var blockchain = new List<Block>();

            blockchain.Add(BlockUtils.GetGenesisBlock());

            var keyPair = KeyPairUtils.GenerateKeyPair();

            var publicKey = keyPair.Public as ECPublicKeyParameters;

            var amount = 9999M;

            var transaction = WalletUtils.GenerateTransaction(_wallet, publicKey, amount, blockchain);

            Assert.IsNull(transaction);
        }

        [TestMethod]
        public void CalculatesBalanceWithoutOutputs()
        {
            var blockchain = new List<Block>();

            blockchain.Add(BlockUtils.GetGenesisBlock());

            var balance = WalletUtils.CalculateBalance(blockchain, _wallet.PublicKey);

            Assert.AreEqual(Constants.StartBalance, balance);
        }

        [TestMethod]
        public void CalculatesBalanceWithOutputs()
        {
            var blockchain = new List<Block>();

            blockchain.Add(BlockUtils.GetGenesisBlock());

            var keyPair = KeyPairUtils.GenerateKeyPair();

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

            var expectedBalance = Constants.StartBalance + transactionOne.TransactionOutputs[publicKey] +
                                  transactionTwo.TransactionOutputs[publicKey];

            Assert.AreEqual(expectedBalance, balance);
        }

        [TestMethod]
        public void CalculatesBalanceWithPreviousTransactions()
        {
            var blockchain = new List<Block>();

            blockchain.Add(BlockUtils.GetGenesisBlock());

            var keyPair = KeyPairUtils.GenerateKeyPair();

            var wallet = new Wallet
            {
                Balance = Constants.StartBalance,
                PrivateKey = keyPair.Private as ECPrivateKeyParameters,
                PublicKey = keyPair.Public as ECPublicKeyParameters
            };

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

            expectedBalance = transaction.TransactionOutputs[_wallet.PublicKey] + minerRewardTransaction.TransactionOutputs[_wallet.PublicKey];

            transactions = new List<Transaction>
            {
                transaction,
                minerRewardTransaction
            };

            blockchain.AddBlock(transactions);

            keyPair = KeyPairUtils.GenerateKeyPair();

            wallet = new Wallet
            {
                Balance = Constants.StartBalance,
                PrivateKey = keyPair.Private as ECPrivateKeyParameters,
                PublicKey = keyPair.Public as ECPublicKeyParameters
            };

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