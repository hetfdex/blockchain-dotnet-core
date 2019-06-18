using blockchain_dotnet_core.API.Extensions;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Extensions
{
    [TestClass]
    public class TransactionPoolExtensionsTests
    {
        private Wallet _wallet;

        private Transaction _transaction;

        private TransactionPool _transactionPool;

        [TestInitialize]
        public void TransactionPoolUtilsTestsSetup()
        {
            _wallet = new Wallet();

            var keyPair = KeyPairUtils.GenerateKeyPair();

            var recipient = keyPair.Public as ECPublicKeyParameters;

            var transactionOutputs = TransactionUtils.GenerateTransactionOutput(_wallet, recipient, 100);

            var transactionInput = TransactionUtils.GenerateTransactionInput(_wallet, transactionOutputs);

            _transaction = new Transaction(transactionOutputs, transactionInput);

            _transactionPool = new TransactionPool();
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
            var keyPair = KeyPairUtils.GenerateKeyPair();

            var recipient = keyPair.Public as ECPublicKeyParameters;

            var transactionOutputs = TransactionUtils.GenerateTransactionOutput(_wallet, recipient, 100);

            var transactionInput = TransactionUtils.GenerateTransactionInput(_wallet, transactionOutputs);

            var transaction = new Transaction(transactionOutputs, transactionInput)
            {
                TransactionOutputs = { [recipient] = 9999 }
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

            var keyPair = KeyPairUtils.GenerateKeyPair();

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
    }
}