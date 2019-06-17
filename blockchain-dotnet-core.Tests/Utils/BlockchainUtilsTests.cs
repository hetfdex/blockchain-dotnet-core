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
            var transactionsOne = new List<Transaction>
            {
                new Transaction(null, null)
            };

            var transactionsTwo = new List<Transaction>
            {
                new Transaction(null, null),
                new Transaction(null, null)
            };

            _blockchain.AddBlock(transactionsOne);
            _blockchain.AddBlock(transactionsTwo);
        }

        [TestMethod]
        public void ReplacesBlockchainWithLongerValidBlockchain()
        {
            var lastBlock = _replacementBlockchain.Chain[_replacementBlockchain.Chain.Count - 1];

            var blockOne = new Block(TimestampUtils.GenerateTimestamp(), lastBlock.Hash, new List<Transaction>(), 0,
                lastBlock.Difficulty);

            var blockTwo = new Block(TimestampUtils.GenerateTimestamp(), blockOne.Hash, new List<Transaction>(), 0,
                blockOne.Difficulty);

            var blockThree = new Block(TimestampUtils.GenerateTimestamp(), blockTwo.Hash, new List<Transaction>(), 0,
                blockTwo.Difficulty);

            _replacementBlockchain.Chain.Add(blockOne);
            _replacementBlockchain.Chain.Add(blockTwo);
            _replacementBlockchain.Chain.Add(blockThree);

            BlockchainUtils.ReplaceChain(ref _blockchain, _replacementBlockchain, false);

            Assert.AreEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithLongerInvalidBlockchain()
        {
            var lastBlock = _replacementBlockchain.Chain[_replacementBlockchain.Chain.Count - 1];

            var blockOne = new Block(TimestampUtils.GenerateTimestamp(), lastBlock.Hash, new List<Transaction>(), 0,
                lastBlock.Difficulty);

            var blockTwo = new Block(TimestampUtils.GenerateTimestamp(), blockOne.Hash, new List<Transaction>(), 0,
                blockOne.Difficulty);

            var blockThree = new Block(TimestampUtils.GenerateTimestamp(), blockTwo.Hash, new List<Transaction>(), 0,
                blockTwo.Difficulty);

            _replacementBlockchain.Chain.Add(blockOne);
            _replacementBlockchain.Chain.Add(blockTwo);
            _replacementBlockchain.Chain.Add(blockThree);

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
            var lastBlock = _replacementBlockchain.Chain[_replacementBlockchain.Chain.Count - 1];

            var blockOne = new Block(TimestampUtils.GenerateTimestamp(), lastBlock.Hash, new List<Transaction>(), 0,
                lastBlock.Difficulty);

            var blockTwo = new Block(TimestampUtils.GenerateTimestamp(), blockOne.Hash, new List<Transaction>(), 0,
                blockOne.Difficulty);

            var blockThree = new Block(TimestampUtils.GenerateTimestamp(), blockTwo.Hash, new List<Transaction>(), 0,
                blockTwo.Difficulty);

            _replacementBlockchain.Chain.Add(blockOne);
            _replacementBlockchain.Chain.Add(blockTwo);
            _replacementBlockchain.Chain.Add(blockThree);

            var senderKeyPair = KeyPairUtils.GenerateKeyPair();

            var senderWallet = new Wallet(senderKeyPair.Private as ECPrivateKeyParameters,
                senderKeyPair.Public as ECPublicKeyParameters,
                ConfigurationOptions.StartBalance);

            var recipientKeyPair = KeyPairUtils.GenerateKeyPair();

            var recipientWallet = new Wallet(recipientKeyPair.Private as ECPrivateKeyParameters,
                recipientKeyPair.Public as ECPublicKeyParameters,
                ConfigurationOptions.StartBalance);

            var transaction =
                WalletUtils.GenerateTransaction(senderWallet, recipientWallet.PublicKey, 100, _replacementBlockchain);

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(senderWallet);

            var transactions = new List<Transaction>
            {
                transaction,
                minerRewardTransaction
            };

            _replacementBlockchain.AddBlock(transactions);

            BlockchainUtils.ReplaceChain(ref _blockchain, _replacementBlockchain, true);

            Assert.IsTrue(_replacementBlockchain.AreValidTransactions());
            Assert.AreEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithMultipleMinerRewards()
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

            var transactions = new List<Transaction>()
            {
                transaction,
                minerRewardTransaction,
                minerRewardTransaction
            };

            _replacementBlockchain.AddBlock(transactions);

            BlockchainUtils.ReplaceChain(ref _blockchain, _replacementBlockchain, true);

            Assert.IsFalse(_replacementBlockchain.AreValidTransactions());
            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithDuplicateTransactions()
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

            var transactions = new List<Transaction>()
            {
                transaction,
                transaction,
                minerRewardTransaction
            };

            _replacementBlockchain.AddBlock(transactions);

            BlockchainUtils.ReplaceChain(ref _blockchain, _replacementBlockchain, true);

            Assert.IsFalse(_replacementBlockchain.AreValidTransactions());
            Assert.AreNotEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithTransactionWithWrongOutput()
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

            transaction.TransactionOutputs[senderWallet.PublicKey] = 9999;

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
            var senderKeyPair = KeyPairUtils.GenerateKeyPair();

            var senderWallet = new Wallet(senderKeyPair.Private as ECPrivateKeyParameters,
                senderKeyPair.Public as ECPublicKeyParameters,
                9999);

            var recipientKeyPair = KeyPairUtils.GenerateKeyPair();

            var recipientPublicKey = recipientKeyPair.Public as ECPublicKeyParameters;

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>
            {
                {senderWallet.PublicKey, 9899},
                {recipientPublicKey, 100}
            };

            var transactionInput = new TransactionInput(TimestampUtils.GenerateTimestamp(), senderWallet.PublicKey,
                senderWallet.Balance,
                KeyPairUtils.GenerateSignature(senderWallet.PrivateKey, transactionOutputs.ToBytes()));

            var transaction = new Transaction(transactionOutputs, transactionInput);

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(senderWallet);

            transaction.TransactionOutputs[senderWallet.PublicKey] = 9999;

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
    }
}