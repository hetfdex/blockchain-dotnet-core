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
        public void SetsTransaction()
        {
            _transactionPool.AddTransaction(_transaction);

            Assert.IsNotNull(_transactionPool.Pool);
            Assert.AreEqual(_transaction, _transactionPool.Pool[_transaction.Id]);
        }

        [TestMethod]
        public void FindsExistingTransactionById()
        {
            _transactionPool.AddTransaction(_transaction);

            Assert.IsTrue(_transactionPool.IsExistingTransaction(_transaction.Id));
        }

        [TestMethod]
        public void FindsExistingTransactionByPublicKey()
        {
            _transactionPool.AddTransaction(_transaction);

            Assert.IsTrue(_transactionPool.IsExistingTransaction(_transaction.TransactionInput.Address));
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
            var keyPair = CryptoUtils.GenerateKeyPair();

            var recipient = keyPair.Public as ECPublicKeyParameters;

            var transactionOutputs = TransactionUtils.GenerateTransactionOutput(_wallet, recipient, 100);

            var transactionInput = TransactionUtils.GenerateTransactionInput(_wallet, transactionOutputs);

            var transaction = new Transaction(transactionOutputs, transactionInput)
            {
                TransactionOutputs = {[recipient] = 9999}
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

            var keyPair = CryptoUtils.GenerateKeyPair();

            var recipient = keyPair.Public as ECPublicKeyParameters;

            var transactionOutputs = TransactionUtils.GenerateTransactionOutput(_wallet, recipient, 100);

            var transactionInput = TransactionUtils.GenerateTransactionInput(_wallet, transactionOutputs);

            var transaction = new Transaction(transactionOutputs, transactionInput);

            blockchain.AddBlock(transactions);

            _transactionPool.AddTransaction(_transaction);
            _transactionPool.AddTransaction(transaction);

            _transactionPool.ClearBlockchainTransactions(blockchain);

            Assert.IsTrue(_transactionPool.Pool.Count == 1);
            Assert.AreEqual(transaction, _transactionPool.Pool[transaction.Id]);
        }

        //

        [TestMethod]
        public void TransactionPoolsAreEqual()
        {
            var sameObject = (object) _transactionPool;

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

            var differentObject = (object) differentTransactionPool;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_transactionPool.Equals(differentObject));
        }

        [TestMethod]
        public void TransactionPoolAndNullAreNotEqual()
        {
            Assert.IsFalse(_transactionPool.Equals((object) null));
        }
    }
}