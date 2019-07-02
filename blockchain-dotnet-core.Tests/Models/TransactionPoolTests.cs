using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class TransactionPoolTests
    {
        private TransactionPool _transactionPool = new TransactionPool();

        private Wallet _senderWallet;

        private Wallet _recipientWallet;

        private Transaction _transaction;

        [TestInitialize]
        public void TransactionPoolUtilsTestsSetup()
        {
            _senderWallet = new Wallet();

            _recipientWallet = new Wallet();

            var transactionOutputs =
                Transaction.GenerateTransactionOutputs(_senderWallet, _recipientWallet.PublicKey, 100);

            var transactionInput = Transaction.GenerateTransactionInput(_senderWallet, transactionOutputs);

            _transaction = new Transaction(transactionOutputs, transactionInput);

            _transactionPool = new TransactionPool();
        }

        [TestMethod]
        public void ConstructTransactionPoolNoParameters()
        {
            Assert.IsNotNull(_transactionPool);
            Assert.IsNotNull(_transactionPool.Pool);
            Assert.IsTrue(_transactionPool.Pool.Count == 0);
        }

        [TestMethod]
        public void ConstructTransactionPool()
        {
            var pool = new Dictionary<Guid, Transaction>();

            _transactionPool = new TransactionPool(pool);

            Assert.IsNotNull(_transactionPool);
            Assert.IsNotNull(_transactionPool.Pool);
            Assert.AreEqual(pool, _transactionPool.Pool);
        }

        [TestMethod]
        public void ConstructTransactionPoolNullPoolThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new TransactionPool(null));
        }

        [TestMethod]
        public void TransactionPoolHasPool()
        {
            Assert.IsInstanceOfType(_transactionPool.Pool, typeof(Dictionary<Guid, Transaction>));
        }

        [TestMethod]
        public void AddTransaction()
        {
            _transactionPool.AddTransaction(_transaction);

            Assert.IsTrue(_transactionPool.Pool.Count == 1);
            Assert.AreEqual(_transaction, _transactionPool.Pool[_transaction.Id]);
        }

        [TestMethod]
        public void AddTransactionPoolNullTransactionThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _transactionPool.AddTransaction(null));
        }

        [TestMethod]
        public void IsExistingTransactionId()
        {
            _transactionPool.AddTransaction(_transaction);

            Assert.IsTrue(_transactionPool.IsExistingTransaction(_transaction.Id));
        }

        [TestMethod]
        public void IsExistingTransactionIdEmptyIdThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _transactionPool.IsExistingTransaction(Guid.Empty));
        }

        [TestMethod]
        public void IsExistingTransactionPublicKey()
        {
            _transactionPool.AddTransaction(_transaction);

            Assert.IsTrue(_transactionPool.IsExistingTransaction(_transaction.TransactionInput.Address));
        }

        [TestMethod]
        public void IsExistingTransactionPublicKeyNullPublicKeyThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _transactionPool.IsExistingTransaction(null));
        }

        [TestMethod]
        public void DoesNotFindMissingTransactionById()
        {
            Assert.IsFalse(_transactionPool.IsExistingTransaction(_transaction.Id));
        }

        [TestMethod]
        public void DoesNotFindMissingTransactionByPublicKey()
        {
            Assert.IsFalse(_transactionPool.IsExistingTransaction(_transaction.TransactionInput.Address));
        }

        [TestMethod]
        public void GetsValidTransactions()
        {
            var transactionOutputs =
                Transaction.GenerateTransactionOutputs(_senderWallet, _recipientWallet.PublicKey, 100);

            var transactionInput = Transaction.GenerateTransactionInput(_senderWallet, transactionOutputs);

            var transaction = new Transaction(transactionOutputs, transactionInput)
            {
                TransactionOutputs = { [_recipientWallet.PublicKey] = 9999 }
            };

            _transactionPool.AddTransaction(_transaction);
            _transactionPool.AddTransaction(transaction);

            var result = _transactionPool.GetValidTransactions();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
            Assert.AreEqual(_transaction, result[0]);
        }

        [TestMethod]
        public void ClearsTransactionPool()
        {
            _transactionPool.AddTransaction(_transaction);

            _transactionPool.ClearTransactions();

            Assert.IsNotNull(_transactionPool.Pool);
            Assert.IsTrue(_transactionPool.Pool.Count == 0);
        }

        [TestMethod]
        public void ClearsBlockchainTransactions()
        {
            var blockchain = new Blockchain();

            var transactions = new List<Transaction>
            {
                _transaction
            };

            var transactionOutputs =
                Transaction.GenerateTransactionOutputs(_senderWallet, _recipientWallet.PublicKey, 100);

            var transactionInput = Transaction.GenerateTransactionInput(_senderWallet, transactionOutputs);

            var transaction = new Transaction(transactionOutputs, transactionInput);

            blockchain.AddBlock(transactions);

            _transactionPool.AddTransaction(_transaction);
            _transactionPool.AddTransaction(transaction);

            _transactionPool.ClearBlockchainTransactions(blockchain);

            Assert.IsTrue(_transactionPool.Pool.Count == 1);
            Assert.AreEqual(transaction, _transactionPool.Pool[transaction.Id]);
        }

        [TestMethod]
        public void ClearsBlockchainTransactionsNullBlockchainThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _transactionPool.ClearBlockchainTransactions(null));
        }

        [TestMethod]
        public void TransactionPoolsAreEqual()
        {
            var sameObject = (object)_transactionPool;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_transactionPool.Equals(sameObject));
        }

        [TestMethod]
        public void TransactionPoolAreNotEqual()
        {
            var differentTransactionPool = new TransactionPool();

            var keyPair = CryptoUtils.GenerateKeyPair();

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var transactionInput = new TransactionInput(0,
                keyPair.Public as ECPublicKeyParameters, 0, keyPair.Private as ECPrivateKeyParameters,
                transactionOutputs);

            differentTransactionPool.Pool.Add(Guid.NewGuid(), new Transaction(transactionOutputs, transactionInput));

            var differentObject = (object)differentTransactionPool;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_transactionPool.Equals(differentObject));
        }

        [TestMethod]
        public void TransactionPoolAndNullAreNotEqual()
        {
            Assert.IsFalse(_transactionPool.Equals((object)null));
        }
    }
}