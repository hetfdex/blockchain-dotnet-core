using blockchain_dotnet_core.API.Extensions;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class BlockchainUtilsTests
    {
        private List<Block> _blockchain = new List<Block>();

        private readonly List<Block> _replacementBlockchain = new List<Block>();

        [TestInitialize]
        public void BlockchainUtilsTestsSetup()
        {
            _blockchain.Add(BlockUtils.GetGenesisBlock());

            _replacementBlockchain.Add(BlockUtils.GetGenesisBlock());

            var transactionsOne = new List<Transaction>
            {
                new Transaction()
            };

            var transactionsTwo = new List<Transaction>
            {
                new Transaction(),
                new Transaction()
            };

            _blockchain.AddBlock(transactionsOne);
            _blockchain.AddBlock(transactionsTwo);
        }

        [TestMethod]
        public void ReplacesBlockchainWithLongerValidBlockchain()
        {
            var lastBlock = _replacementBlockchain[_replacementBlockchain.Count - 1];

            var blockOne = new Block
            {
                Timestamp = TimestampUtils.GenerateTimestamp(),
                LastHash = lastBlock.Hash,
                Transactions = new List<Transaction>(),
                Nonce = 0,
                Difficulty = lastBlock.Difficulty
            };

            blockOne.Hash = HashUtils.ComputeSHA256(blockOne);

            var blockTwo = new Block
            {
                Timestamp = TimestampUtils.GenerateTimestamp(),
                LastHash = blockOne.Hash,
                Transactions = new List<Transaction>(),
                Nonce = 0,
                Difficulty = blockOne.Difficulty
            };

            blockTwo.Hash = HashUtils.ComputeSHA256(blockTwo);

            var blockThree = new Block
            {
                Timestamp = TimestampUtils.GenerateTimestamp(),
                LastHash = blockTwo.Hash,
                Transactions = new List<Transaction>(),
                Nonce = 0,
                Difficulty = blockTwo.Difficulty
            };

            blockThree.Hash = HashUtils.ComputeSHA256(blockThree);

            _replacementBlockchain.Add(blockOne);
            _replacementBlockchain.Add(blockTwo);
            _replacementBlockchain.Add(blockThree);

            BlockchainUtils.ReplaceChain(ref _blockchain, _replacementBlockchain, false);

            Assert.AreEqual(_replacementBlockchain, _blockchain);
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithLongerInvalidBlockchain()
        {
            var lastBlock = _replacementBlockchain[_replacementBlockchain.Count - 1];

            var blockOne = new Block
            {
                Timestamp = TimestampUtils.GenerateTimestamp(),
                LastHash = lastBlock.Hash,
                Transactions = new List<Transaction>(),
                Nonce = 0,
                Difficulty = lastBlock.Difficulty
            };

            blockOne.Hash = HashUtils.ComputeSHA256(blockOne);

            var blockTwo = new Block
            {
                Timestamp = TimestampUtils.GenerateTimestamp(),
                LastHash = blockOne.Hash,
                Transactions = new List<Transaction>(),
                Nonce = 0,
                Difficulty = blockOne.Difficulty
            };

            blockTwo.Hash = HashUtils.ComputeSHA256(blockTwo);

            var blockThree = new Block
            {
                Timestamp = TimestampUtils.GenerateTimestamp(),
                LastHash = blockTwo.Hash,
                Transactions = new List<Transaction>(),
                Nonce = 0,
                Difficulty = blockTwo.Difficulty
            };

            _replacementBlockchain.Add(blockOne);
            _replacementBlockchain.Add(blockTwo);
            _replacementBlockchain.Add(blockThree);

            _replacementBlockchain[_replacementBlockchain.Count - 1].LastHash = "fake-lastHash";

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
            var senderKeyPair = KeyPairUtils.GenerateKeyPair();

            var senderWallet = new Wallet
            {
                Balance = Constants.StartBalance,
                PrivateKey = senderKeyPair.Private as ECPrivateKeyParameters,
                PublicKey = senderKeyPair.Public as ECPublicKeyParameters
            };

            var recipientKeyPair = KeyPairUtils.GenerateKeyPair();

            var recipientPublicKey = recipientKeyPair.Public as ECPublicKeyParameters;

           /* var transactionOutputs = TransactionUtils.GenerateTransactionOutput(senderWallet, recipientPublicKey, 100);

            var transactionInput = TransactionUtils.GenerateTransactionInput(senderWallet, transactionOutputs);

            var transaction = new Transaction
            {
                TransactionInput = transactionInput,
                TransactionOutputs = transactionOutputs
            };*/

            var transaction = WalletUtils.GenerateTransaction(senderWallet, recipientPublicKey, 100, _replacementBlockchain);

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(senderWallet);

            var transactions = new List<Transaction>()
             {
                 transaction,
                 minerRewardTransaction
             };

            _replacementBlockchain.AddBlock(transactions);

            Assert.IsTrue(_replacementBlockchain.IsValidTransactionData());
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithMultipleMinerRewards()
        {
            var senderKeyPair = KeyPairUtils.GenerateKeyPair();

            var senderWallet = new Wallet
            {
                Balance = Constants.StartBalance,
                PrivateKey = senderKeyPair.Private as ECPrivateKeyParameters,
                PublicKey = senderKeyPair.Public as ECPublicKeyParameters
            };

            var recipientKeyPair = KeyPairUtils.GenerateKeyPair();

            var recipientPublicKey = recipientKeyPair.Public as ECPublicKeyParameters;

            var transaction = WalletUtils.GenerateTransaction(senderWallet, recipientPublicKey, 100, _replacementBlockchain);

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(senderWallet);

            var transactions = new List<Transaction>()
            {
                transaction,
                minerRewardTransaction,
                minerRewardTransaction
            };

            _replacementBlockchain.AddBlock(transactions);

            Assert.IsFalse(_replacementBlockchain.IsValidTransactionData());
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithDuplicateTransactions()
        {
            var senderKeyPair = KeyPairUtils.GenerateKeyPair();

            var senderWallet = new Wallet
            {
                Balance = Constants.StartBalance,
                PrivateKey = senderKeyPair.Private as ECPrivateKeyParameters,
                PublicKey = senderKeyPair.Public as ECPublicKeyParameters
            };

            var recipientKeyPair = KeyPairUtils.GenerateKeyPair();

            var recipientPublicKey = recipientKeyPair.Public as ECPublicKeyParameters;

            var transaction = WalletUtils.GenerateTransaction(senderWallet, recipientPublicKey, 100, _replacementBlockchain);

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(senderWallet);

            var transactions = new List<Transaction>()
            {
                transaction,
                transaction,
                minerRewardTransaction
            };

            _replacementBlockchain.AddBlock(transactions);

            Assert.IsFalse(_replacementBlockchain.IsValidTransactionData());
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithTransactionWithWrongOutput()
        {
            var senderKeyPair = KeyPairUtils.GenerateKeyPair();

            var senderWallet = new Wallet
            {
                Balance = Constants.StartBalance,
                PrivateKey = senderKeyPair.Private as ECPrivateKeyParameters,
                PublicKey = senderKeyPair.Public as ECPublicKeyParameters
            };

            var recipientKeyPair = KeyPairUtils.GenerateKeyPair();

            var recipientPublicKey = recipientKeyPair.Public as ECPublicKeyParameters;

            var transaction = WalletUtils.GenerateTransaction(senderWallet, recipientPublicKey, 100, _replacementBlockchain);

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(senderWallet);

            transaction.TransactionOutputs[senderWallet.PublicKey] = 9999;

            var transactions = new List<Transaction>()
            {
                transaction,
                minerRewardTransaction
            };

            _replacementBlockchain.AddBlock(transactions);

            Assert.IsFalse(_replacementBlockchain.IsValidTransactionData());
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithTransactionWithWrongOutputMiner()
        {
            var senderKeyPair = KeyPairUtils.GenerateKeyPair();

            var senderWallet = new Wallet
            {
                Balance = Constants.StartBalance,
                PrivateKey = senderKeyPair.Private as ECPrivateKeyParameters,
                PublicKey = senderKeyPair.Public as ECPublicKeyParameters
            };

            var recipientKeyPair = KeyPairUtils.GenerateKeyPair();

            var recipientPublicKey = recipientKeyPair.Public as ECPublicKeyParameters;

            var transaction = WalletUtils.GenerateTransaction(senderWallet, recipientPublicKey, 100, _replacementBlockchain);

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(senderWallet);

            minerRewardTransaction.TransactionOutputs[senderWallet.PublicKey] = 9999;

            var transactions = new List<Transaction>()
            {
                transaction,
                minerRewardTransaction
            };

            _replacementBlockchain.AddBlock(transactions);

            Assert.IsFalse(_replacementBlockchain.IsValidTransactionData());
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithTransactionWithWrongInput()
        {
            var senderKeyPair = KeyPairUtils.GenerateKeyPair();

            var senderWallet = new Wallet
            {
                Balance = 9999,
                PrivateKey = senderKeyPair.Private as ECPrivateKeyParameters,
                PublicKey = senderKeyPair.Public as ECPublicKeyParameters
            };

            var recipientKeyPair = KeyPairUtils.GenerateKeyPair();

            var recipientPublicKey = recipientKeyPair.Public as ECPublicKeyParameters;

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>
            {
                {senderWallet.PublicKey, 9899},
                {recipientPublicKey, 100}
            };

            var senderPrivateKey = senderKeyPair.Private as ECPrivateKeyParameters;

            var transactionInput = new TransactionInput
            {
                Timestamp = TimestampUtils.GenerateTimestamp(),
                Amount = senderWallet.Balance,
                Address = senderWallet.PublicKey,
                Signature = KeyPairUtils.GenerateSignature(senderPrivateKey, transactionOutputs)
            };

            var transaction = new Transaction
            {
                TransactionOutputs = transactionOutputs,
                TransactionInput = transactionInput
            };

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(senderWallet);

            transaction.TransactionOutputs[senderWallet.PublicKey] = 9999;

            var transactions = new List<Transaction>()
            {
                transaction,
                minerRewardTransaction
            };

            _replacementBlockchain.AddBlock(transactions);

            Assert.IsFalse(_replacementBlockchain.IsValidTransactionData());
        }
    }
}