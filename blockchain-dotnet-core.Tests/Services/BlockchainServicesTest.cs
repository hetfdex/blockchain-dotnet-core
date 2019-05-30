using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Services;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Services
{
    [TestClass]
    public class BlockchainServicesTest
    {
        private IBlockchainService _blockchainService;

        private IBlockService _blockService;

        private List<Block> _originalBlockchain = new List<Block>();

        private List<Block> _replacementBlockchain = new List<Block>();

        [TestInitialize]
        public void BlockchainServiceTestsSetup()
        {
            _blockService = new BlockService();

            _blockchainService = new BlockchainService(_blockService);

            var transactionsOne = new List<Transaction>
            {
                new Transaction()
            };

            var transactionsTwo = new List<Transaction>
            {
                new Transaction(),
                new Transaction()
            };

            _blockchainService.AddBlock(transactionsOne);
            _blockchainService.AddBlock(transactionsTwo);

            _replacementBlockchain.Add(_blockService.GetGenesisBlock());

            _originalBlockchain = _blockchainService.Blockchain;
        }

        [TestMethod]
        public void ConstructBlockchain()
        {
            Assert.IsNotNull(_blockchainService.Blockchain);
            Assert.IsInstanceOfType(_blockchainService.Blockchain, typeof(List<Block>));
            Assert.AreEqual(_blockService.GetGenesisBlock(), _blockchainService.Blockchain[0]);
        }

        [TestMethod]
        public void AddsToBlockchain()
        {
            var transactions = new List<Transaction>();

            _blockchainService.AddBlock(transactions);

            Assert.AreEqual(transactions, _blockchainService.Blockchain[_blockchainService.Blockchain.Count - 1].Transactions);
        }

        [TestMethod]
        public void BlockchainIsValid()
        {
            Assert.IsTrue(_blockchainService.IsValidChain(_blockchainService.Blockchain));
        }

        [TestMethod]
        public void BlockchainIsNotValidNoGenesisBlock()
        {
            _blockchainService.Blockchain[0] = new Block();

            Assert.IsFalse(_blockchainService.IsValidChain(_blockchainService.Blockchain));
        }

        [TestMethod]
        public void BlockchainIsNotValidFakeLastHash()
        {
            _blockchainService.Blockchain[_blockchainService.Blockchain.Count - 1].LastHash = "fake-lastHash";

            Assert.IsFalse(_blockchainService.IsValidChain(_blockchainService.Blockchain));
        }

        [TestMethod]
        public void BlockchainIsNotValidFakeTransactions()
        {
            _blockchainService.Blockchain[_blockchainService.Blockchain.Count - 1].Transactions = null;

            Assert.IsFalse(_blockchainService.IsValidChain(_blockchainService.Blockchain));
        }

        [TestMethod]
        public void BlockchainIsNotValidFakeDifficulty()
        {
            var lastBlock = _blockchainService.Blockchain[_blockchainService.Blockchain.Count - 1];

            var timestamp = TimestampUtils.GetTimestamp();

            var lastHash = lastBlock.Hash;

            var transactions = new List<Transaction>();

            var nonce = 0;

            var difficulty = lastBlock.Difficulty - 2;

            var hash = HashUtils.ComputeSHA256(timestamp, lastHash, transactions, nonce, difficulty);

            var fakeBlock = new Block
            {
                Timestamp = timestamp,
                LastHash = lastHash,
                Hash = hash,
                Transactions = transactions,
                Nonce = nonce,
                Difficulty = difficulty
            };

            _blockchainService.Blockchain.Add(fakeBlock);

            Assert.IsFalse(_blockchainService.IsValidChain(_blockchainService.Blockchain));
        }

        [TestMethod]
        public void ReplacesBlockchainWithLongerValidBlockchain()
        {
            var lastBlock = _replacementBlockchain[_replacementBlockchain.Count - 1];

            var timestamp = TimestampUtils.GetTimestamp();

            var lastHash = lastBlock.Hash;

            var transactions = new List<Transaction>();

            var nonce = 0;

            var difficulty = lastBlock.Difficulty;

            var hash = HashUtils.ComputeSHA256(timestamp, lastHash, transactions, nonce, difficulty);

            var blockOne = new Block
            {
                Timestamp = timestamp,
                LastHash = lastHash,
                Hash = hash,
                Transactions = transactions,
                Nonce = nonce,
                Difficulty = difficulty
            };

            timestamp = TimestampUtils.GetTimestamp();

            lastHash = blockOne.Hash;

            transactions = new List<Transaction>();

            nonce = 0;

            difficulty = blockOne.Difficulty;

            hash = HashUtils.ComputeSHA256(timestamp, lastHash, transactions, nonce, difficulty);

            var blockTwo = new Block
            {
                Timestamp = timestamp,
                LastHash = lastHash,
                Hash = hash,
                Transactions = transactions,
                Nonce = nonce,
                Difficulty = difficulty
            };

            timestamp = TimestampUtils.GetTimestamp();

            lastHash = blockTwo.Hash;

            transactions = new List<Transaction>();

            nonce = 0;

            difficulty = blockTwo.Difficulty;

            hash = HashUtils.ComputeSHA256(timestamp, lastHash, transactions, nonce, difficulty);

            var blockThree = new Block
            {
                Timestamp = timestamp,
                LastHash = lastHash,
                Hash = hash,
                Transactions = transactions,
                Nonce = nonce,
                Difficulty = difficulty
            };

            _replacementBlockchain.Add(blockOne);
            _replacementBlockchain.Add(blockTwo);
            _replacementBlockchain.Add(blockThree);

            _blockchainService.ReplaceChain(_replacementBlockchain, false);

            Assert.AreEqual(_replacementBlockchain, _blockchainService.Blockchain);
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithLongerInvalidBlockchain()
        {
            var lastBlock = _replacementBlockchain[_replacementBlockchain.Count - 1];

            var timestamp = TimestampUtils.GetTimestamp();

            var lastHash = lastBlock.Hash;

            var transactions = new List<Transaction>();

            var nonce = 0;

            var difficulty = lastBlock.Difficulty;

            var hash = HashUtils.ComputeSHA256(timestamp, lastHash, transactions, nonce, difficulty);

            var blockOne = new Block
            {
                Timestamp = timestamp,
                LastHash = lastHash,
                Hash = hash,
                Transactions = transactions,
                Nonce = nonce,
                Difficulty = difficulty
            };

            timestamp = TimestampUtils.GetTimestamp();

            lastHash = blockOne.Hash;

            transactions = new List<Transaction>();

            nonce = 0;

            difficulty = blockOne.Difficulty;

            hash = HashUtils.ComputeSHA256(timestamp, lastHash, transactions, nonce, difficulty);

            var blockTwo = new Block
            {
                Timestamp = timestamp,
                LastHash = lastHash,
                Hash = hash,
                Transactions = transactions,
                Nonce = nonce,
                Difficulty = difficulty
            };

            timestamp = TimestampUtils.GetTimestamp();

            lastHash = blockTwo.Hash;

            transactions = new List<Transaction>();

            nonce = 0;

            difficulty = blockTwo.Difficulty;

            hash = HashUtils.ComputeSHA256(timestamp, lastHash, transactions, nonce, difficulty);

            var blockThree = new Block
            {
                Timestamp = timestamp,
                LastHash = lastHash,
                Hash = hash,
                Transactions = transactions,
                Nonce = nonce,
                Difficulty = difficulty
            };

            _replacementBlockchain.Add(blockOne);
            _replacementBlockchain.Add(blockTwo);
            _replacementBlockchain.Add(blockThree);

            _replacementBlockchain[_replacementBlockchain.Count - 1].LastHash = "fake-lastHash";

            _blockchainService.ReplaceChain(_replacementBlockchain, false);

            Assert.AreEqual(_originalBlockchain, _blockchainService.Blockchain);
        }

        [TestMethod]
        public void DoesNotReplaceBlockchainWithShorterBlockchain()
        {
            _blockchainService.ReplaceChain(_replacementBlockchain, false);

            Assert.AreEqual(_originalBlockchain, _blockchainService.Blockchain);
        }
    }
}