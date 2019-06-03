using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace blockchain_dotnet_core.Tests.Extensions
{
    [TestClass]
    public class BlockchainServicesTests
    {
        /* private IBlockchainService _blockchainService;

         private IBlockchainService _replacementBlockchainService;

         private IBlockService _blockService;

         private ITransactionService _transactionService;

         private IWalletService _walletService;

         private List<Block> _originalBlockchain = new List<Block>();

         public BlockchainServicesTests(IWalletService walletService, ITransactionService transactionService, IBlockService blockService, IBlockchainService replacementBlockchainService, IBlockchainService blockchainService)
         {
             _walletService = walletService;

             _transactionService = transactionService;

             _blockService = blockService;

             _replacementBlockchainService = replacementBlockchainService;

             _blockchainService = blockchainService;
         }

         [TestInitialize]
         public void BlockchainServiceTestsSetup()
         {
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

             var timestamp = TimestampUtils.GenerateTimestamp();

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
             var lastBlock = _replacementBlockchainService.Blockchain[_replacementBlockchainService.Blockchain.Count - 1];

             var timestamp = TimestampUtils.GenerateTimestamp();

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

             timestamp = TimestampUtils.GenerateTimestamp();

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

             timestamp = TimestampUtils.GenerateTimestamp();

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

             _replacementBlockchainService.Blockchain.Add(blockOne);
             _replacementBlockchainService.Blockchain.Add(blockTwo);
             _replacementBlockchainService.Blockchain.Add(blockThree);

             _blockchainService.ReplaceChain(_replacementBlockchainService.Blockchain, false);

             Assert.AreEqual(_replacementBlockchainService.Blockchain, _blockchainService.Blockchain);
         }

         [TestMethod]
         public void DoesNotReplaceBlockchainWithLongerInvalidBlockchain()
         {
             var lastBlock = _replacementBlockchainService.Blockchain[_replacementBlockchainService.Blockchain.Count - 1];

             var timestamp = TimestampUtils.GenerateTimestamp();

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

             timestamp = TimestampUtils.GenerateTimestamp();

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

             timestamp = TimestampUtils.GenerateTimestamp();

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

             _replacementBlockchainService.Blockchain.Add(blockOne);
             _replacementBlockchainService.Blockchain.Add(blockTwo);
             _replacementBlockchainService.Blockchain.Add(blockThree);

             _replacementBlockchainService.Blockchain[_replacementBlockchainService.Blockchain.Count - 1].LastHash = "fake-lastHash";

             _blockchainService.ReplaceChain(_replacementBlockchainService.Blockchain, false);

             Assert.AreEqual(_originalBlockchain, _blockchainService.Blockchain);
         }

         [TestMethod]
         public void DoesNotReplaceBlockchainWithShorterBlockchain()
         {
             _blockchainService.ReplaceChain(_replacementBlockchainService.Blockchain, false);

             Assert.AreEqual(_originalBlockchain, _blockchainService.Blockchain);
         }

         [TestMethod]
         public void ReplacesBlockchainWithValidTransactionData()
         {
             var keyPair = KeyPairUtils.GenerateKeyPair();

             var publicKey = keyPair.Public as ECPublicKeyParameters;

             var transaction = _walletService.GenerateTransaction(publicKey, 100, _blockchainService.Blockchain);

             var minerRewardTransaction = _transactionService.GetMinerRewardTransaction(_walletService.Wallet);

             var transactions = new List<Transaction>()
             {
                 transaction,
                 minerRewardTransaction
             };

             _replacementBlockchainService.AddBlock(transactions);

             Assert.IsTrue(_replacementBlockchainService.IsValidTransactionData(_replacementBlockchainService.Blockchain));
         }*/
    }
}