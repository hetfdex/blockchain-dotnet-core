using blockchain_dotnet_core.API.Extensions;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Options;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class BlockchainUtilsTests
    {
        private Blockchain _blockchain = new Blockchain();

        private readonly Blockchain _replacementBlockchain = new Blockchain();

        [TestInitialize]
        public void BlockchainUtilsTestsSetup()
        {
            var firstWallet = new Wallet();

            var secondWallet = new Wallet();

            var transactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(firstWallet, secondWallet.PublicKey, 100, _blockchain),
                WalletUtils.GenerateTransaction(secondWallet, firstWallet.PublicKey, 200, _blockchain)
            };

            _blockchain.AddBlock(transactions);
        }

        [TestMethod]
        public void ReplacesBlockchainWithLongerValidBlockchain()
        {
            var firstWallet = new Wallet();

            var secondWallet = new Wallet();

            var firstTransactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(firstWallet, secondWallet.PublicKey, 100, _replacementBlockchain),
            };

            var secondTransactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(secondWallet, firstWallet.PublicKey, 200, _replacementBlockchain)
            };

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);

            BlockchainUtils.ReplaceChain(ref _blockchain, _replacementBlockchain, false);

            Assert.AreEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithLongerInvalidBlockchain()
        {
            var firstKeyPair = KeyPairUtils.GenerateKeyPair();

            var secondKeyPair = KeyPairUtils.GenerateKeyPair();

            var firstWallet = new Wallet(firstKeyPair.Private as ECPrivateKeyParameters,
                firstKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var secondWallet = new Wallet(secondKeyPair.Private as ECPrivateKeyParameters,
                secondKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var firstTransactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(firstWallet, secondWallet.PublicKey, 100, _blockchain),
            };

            var secondTransactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(secondWallet, firstWallet.PublicKey, 200, _blockchain)
            };

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);

            _replacementBlockchain.Chain[_replacementBlockchain.Chain.Count - 1].LastHash = "fake-lastHash";

            BlockchainUtils.ReplaceChain(ref _blockchain, _replacementBlockchain, false);

            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithShorterBlockchain()
        {
            BlockchainUtils.ReplaceChain(ref _blockchain, _replacementBlockchain, false);

            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void ReplacesBlockchainWithValidTransactionData()
        {
            var firstKeyPair = KeyPairUtils.GenerateKeyPair();

            var secondKeyPair = KeyPairUtils.GenerateKeyPair();

            var minerKeyPair = KeyPairUtils.GenerateKeyPair();

            var firstWallet = new Wallet(firstKeyPair.Private as ECPrivateKeyParameters,
                firstKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var secondWallet = new Wallet(secondKeyPair.Private as ECPrivateKeyParameters,
                secondKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var minerWallet = new Wallet(minerKeyPair.Private as ECPrivateKeyParameters,
                minerKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(minerWallet);

            var firstTransactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(firstWallet, secondWallet.PublicKey, 100, _blockchain),
                minerRewardTransaction
            };

            var secondTransactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(secondWallet, firstWallet.PublicKey, 200, _blockchain)
            };

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);

            BlockchainUtils.ReplaceChain(ref _blockchain, _replacementBlockchain, true);

            Assert.IsTrue(_replacementBlockchain.AreValidTransactions());
            Assert.AreEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithMultipleMinerRewards()
        {
            var firstKeyPair = KeyPairUtils.GenerateKeyPair();

            var secondKeyPair = KeyPairUtils.GenerateKeyPair();

            var firstWallet = new Wallet(firstKeyPair.Private as ECPrivateKeyParameters,
                firstKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var secondWallet = new Wallet(secondKeyPair.Private as ECPrivateKeyParameters,
                secondKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(firstWallet);

            var firstTransactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(firstWallet, secondWallet.PublicKey, 100, _blockchain),
                minerRewardTransaction,
                minerRewardTransaction
            };

            var secondTransactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(secondWallet, firstWallet.PublicKey, 200, _blockchain)
            };

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);

            BlockchainUtils.ReplaceChain(ref _blockchain, _replacementBlockchain, true);

            Assert.IsFalse(_replacementBlockchain.AreValidTransactions());
            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithDuplicateTransactions()
        {
            var firstKeyPair = KeyPairUtils.GenerateKeyPair();

            var secondKeyPair = KeyPairUtils.GenerateKeyPair();

            var firstWallet = new Wallet(firstKeyPair.Private as ECPrivateKeyParameters,
                firstKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var secondWallet = new Wallet(secondKeyPair.Private as ECPrivateKeyParameters,
                secondKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(firstWallet);

            var firstTransactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(firstWallet, secondWallet.PublicKey, 100, _blockchain),
                WalletUtils.GenerateTransaction(firstWallet, secondWallet.PublicKey, 100, _blockchain)
            };

            var secondTransactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(secondWallet, firstWallet.PublicKey, 200, _blockchain)
            };

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);

            BlockchainUtils.ReplaceChain(ref _blockchain, _replacementBlockchain, true);

            Assert.IsFalse(_replacementBlockchain.AreValidTransactions());
            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithTransactionWithWrongOutput()
        {
            var firstKeyPair = KeyPairUtils.GenerateKeyPair();

            var secondKeyPair = KeyPairUtils.GenerateKeyPair();

            var firstWallet = new Wallet(firstKeyPair.Private as ECPrivateKeyParameters,
                firstKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var secondWallet = new Wallet(secondKeyPair.Private as ECPrivateKeyParameters,
                secondKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var firstTransactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(firstWallet, secondWallet.PublicKey, 100, _blockchain),
            };

            var secondTransactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(secondWallet, firstWallet.PublicKey, 200, _blockchain)
            };

            secondTransactions[0].TransactionOutputs[secondWallet.PublicKey] = 9999;

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);

            BlockchainUtils.ReplaceChain(ref _blockchain, _replacementBlockchain, true);

            Assert.IsFalse(_replacementBlockchain.AreValidTransactions());
            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithTransactionWithWrongOutputMiner()
        {
            var senderKeyPair = KeyPairUtils.GenerateKeyPair();

            var senderWallet = new Wallet(senderKeyPair.Private as ECPrivateKeyParameters,
                senderKeyPair.Public as ECPublicKeyParameters,
                ConfigurationOptions.StartBalance);

            var recipientKeyPair = KeyPairUtils.GenerateKeyPair();

            var recipientPublicKey = recipientKeyPair.Public as ECPublicKeyParameters;

            var transaction =
                WalletUtils.GenerateTransaction(senderWallet, recipientPublicKey, 100, _replacementBlockchain);

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(senderWallet);

            minerRewardTransaction.TransactionOutputs[senderWallet.PublicKey] = 9999;

            var transactions = new List<Transaction>()
            {
                transaction,
                minerRewardTransaction
            };

            _replacementBlockchain.AddBlock(transactions);

            BlockchainUtils.ReplaceChain(ref _blockchain, _replacementBlockchain, true);

            Assert.IsFalse(_replacementBlockchain.AreValidTransactions());
            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithTransactionWithWrongInput()
        {
            var firstKeyPair = KeyPairUtils.GenerateKeyPair();

            var secondKeyPair = KeyPairUtils.GenerateKeyPair();

            var firstWallet = new Wallet(firstKeyPair.Private as ECPrivateKeyParameters,
                firstKeyPair.Public as ECPublicKeyParameters, 9999);

            var secondWallet = new Wallet(secondKeyPair.Private as ECPrivateKeyParameters,
                secondKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var firstTransactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(firstWallet, secondWallet.PublicKey, 100, _blockchain),
            };

            var secondTransactions = new List<Transaction>
            {
                WalletUtils.GenerateTransaction(secondWallet, firstWallet.PublicKey, 200, _blockchain)
            };

            secondTransactions[0].TransactionOutputs[secondWallet.PublicKey] = 9999;

            _replacementBlockchain.AddBlock(firstTransactions);
            _replacementBlockchain.AddBlock(secondTransactions);

            BlockchainUtils.ReplaceChain(ref _blockchain, _replacementBlockchain, true);

            Assert.IsFalse(_replacementBlockchain.AreValidTransactions());
            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }
    }
}