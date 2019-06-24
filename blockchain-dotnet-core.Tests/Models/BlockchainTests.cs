using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class BlockchainTests
    {
        private readonly Blockchain _blockchain = new Blockchain();

        [TestMethod]
        public void ConstructBlockchain()
        {
            var genesisBlock = Block.GetGenesisBlock();

            Assert.IsNotNull(_blockchain);
            Assert.IsNotNull(_blockchain.Chain);
            Assert.IsTrue(_blockchain.Chain.Count == 1);
            Assert.IsNotNull(_blockchain.Chain[_blockchain.Chain.Count - 1]);
            Assert.AreEqual(genesisBlock, _blockchain.Chain[_blockchain.Chain.Count - 1]);
        }

        [TestMethod]
        public void AddBlock()
        {
            var transactions = new List<Transaction>();

            _blockchain.AddBlock(transactions);

            Assert.IsTrue(_blockchain.Chain.Count == 2);
            Assert.IsNotNull(_blockchain.Chain[_blockchain.Chain.Count - 1]);
            Assert.AreEqual(transactions, _blockchain.Chain[_blockchain.Chain.Count - 1].Transactions);
        }

        [TestMethod]
        public void AddBlockNullTransactionThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _blockchain.AddBlock(null));
        }

        [TestMethod]
        public void AddBlockNullLastBlockThrowsException()
        {
            var transactions = new List<Transaction>();

            _blockchain.Chain[_blockchain.Chain.Count - 1] = null;

            Assert.ThrowsException<ArgumentNullException>(() => _blockchain.AddBlock(transactions));
        }

        /*[TestMethod]
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
            var firstKeyPair = CryptoUtils.GenerateKeyPair();

            var secondKeyPair = CryptoUtils.GenerateKeyPair();

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
            var firstKeyPair = CryptoUtils.GenerateKeyPair();

            var secondKeyPair = CryptoUtils.GenerateKeyPair();

            var minerKeyPair = CryptoUtils.GenerateKeyPair();

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
            var firstKeyPair = CryptoUtils.GenerateKeyPair();

            var secondKeyPair = CryptoUtils.GenerateKeyPair();

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
            var firstKeyPair = CryptoUtils.GenerateKeyPair();

            var secondKeyPair = CryptoUtils.GenerateKeyPair();

            var firstWallet = new Wallet(firstKeyPair.Private as ECPrivateKeyParameters,
                firstKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var secondWallet = new Wallet(secondKeyPair.Private as ECPrivateKeyParameters,
                secondKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

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
            var firstKeyPair = CryptoUtils.GenerateKeyPair();

            var secondKeyPair = CryptoUtils.GenerateKeyPair();

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
            var senderKeyPair = CryptoUtils.GenerateKeyPair();

            var senderWallet = new Wallet(senderKeyPair.Private as ECPrivateKeyParameters,
                senderKeyPair.Public as ECPublicKeyParameters,
                ConfigurationOptions.StartBalance);

            var recipientKeyPair = CryptoUtils.GenerateKeyPair();

            var recipientPublicKey = recipientKeyPair.Public as ECPublicKeyParameters;

            var transaction =
                WalletUtils.GenerateTransaction(senderWallet, recipientPublicKey, 100, _replacementBlockchain);

            var minerRewardTransaction = TransactionUtils.GetMinerRewardTransaction(senderWallet);

            minerRewardTransaction.TransactionOutputs[senderWallet.PublicKey] = 9999;

            var transactions = new List<Transaction>
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
            var firstKeyPair = CryptoUtils.GenerateKeyPair();

            var secondKeyPair = CryptoUtils.GenerateKeyPair();

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

        [TestMethod]
        public void TransactionIsValid()
        {
            var senderWallet = new Wallet();

            var recipientWallet = new Wallet();

            var transaction = new Transaction(senderWallet, recipientWallet.PublicKey, 10);

            var transactions = new List<Transaction>
            {
                transaction
            };

            _blockchain.AddBlock(transactions);

            Assert.IsTrue(_blockchain.AreValidTransactions());
        }

        [TestMethod]
        public void TransactionHasInvalidOutputs()
        {
            var senderWallet = new Wallet();

            var recipientWallet = new Wallet();

            var transaction = new Transaction(senderWallet, recipientWallet.PublicKey, 10);

            var transactions = new List<Transaction>
            {
                transaction
            };

            _blockchain.AddBlock(transactions);

            _blockchain.Chain[_blockchain.Chain.Count - 1]
                .Transactions[_blockchain.Chain[_blockchain.Chain.Count - 1].Transactions.Count - 1]
                .TransactionOutputs[senderWallet.PublicKey] = 9999;

            Assert.IsFalse(_blockchain.AreValidTransactions());
        }

        [TestMethod]
        public void TransactionHasInvalidInput()
        {
            var senderWallet = new Wallet();

            var recipientWallet = new Wallet();

            var transaction = new Transaction(senderWallet, recipientWallet.PublicKey, 10);

            var transactions = new List<Transaction>
            {
                transaction
            };

            _blockchain.AddBlock(transactions);

            var fakeTransactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>
            {
                {recipientWallet.PublicKey, 9999}
            };

            _blockchain.Chain[_blockchain.Chain.Count - 1]
                    .Transactions[_blockchain.Chain[_blockchain.Chain.Count - 1].Transactions.Count - 1]
                    .TransactionInput.Signature =
                CryptoUtils.GenerateSignature(senderWallet.PrivateKey, fakeTransactionOutputs.ToHash());

            Assert.IsFalse(_blockchain.AreValidTransactions());
        }*/

        [TestMethod]
        public void IsValidChain()
        {
            Assert.IsTrue(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void IsValidChainFakeGenesisBlock()
        {
            _blockchain.Chain[0] = new Block(0, 0, "fake-lastHash", new List<Transaction>(), 0, 0);

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void IsValidChainFakeIndex()
        {
            var transactions = new List<Transaction>();

            _blockchain.AddBlock(transactions);

            _blockchain.Chain[_blockchain.Chain.Count - 1].Index = 99;

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void IsValidChainFakeLastHash()
        {
            var transactions = new List<Transaction>();

            _blockchain.AddBlock(transactions);

            _blockchain.Chain[_blockchain.Chain.Count - 1].LastHash = "fake-lastHash";

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void IsValidChainFakeTransactions()
        {
            var keyPair = CryptoUtils.GenerateKeyPair();

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var transactionInput = new TransactionInput(0,
                keyPair.Public as ECPublicKeyParameters, 0, keyPair.Private as ECPrivateKeyParameters,
                transactionOutputs);

            var transactions = new List<Transaction>
            {
                new Transaction(transactionOutputs, transactionInput)
            };

            _blockchain.AddBlock(transactions);

            _blockchain.Chain[_blockchain.Chain.Count - 1].Transactions.RemoveAt(0);

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void IsValidChainFakeDifficulty()
        {
            var transactions = new List<Transaction>();

            _blockchain.AddBlock(transactions);

            var lastBlock = _blockchain.Chain[_blockchain.Chain.Count - 1];

            var fakeBlock = new Block(lastBlock.Index + 1, TimestampUtils.GenerateTimestamp(), lastBlock.Hash,
                new List<Transaction>(),
                0, 10);

            _blockchain.Chain.Add(fakeBlock);

            Assert.IsFalse(_blockchain.IsValidChain());
        }

        [TestMethod]
        public void BlockchainsAreEqual()
        {
            var sameObject = (object)_blockchain;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_blockchain.Equals(sameObject));
        }

        [TestMethod]
        public void BlockchainsAreNotEqual()
        {
            var differentBlockchain = new Blockchain();

            var block = new Block(0, 0, "test-lastHash", new List<Transaction>(), 0, 1);

            differentBlockchain.Chain.Add(block);

            var differentObject = (object)differentBlockchain;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_blockchain.Equals(differentObject));
        }

        [TestMethod]
        public void BlockchainAndNullAreNotEqual()
        {
            Assert.IsFalse(_blockchain.Equals((object)null));
        }
    }
}