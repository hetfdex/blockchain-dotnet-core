using blockchain_dotnet_core.API.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class TransactionPoolTests
    {
        private TransactionPool _transactionPool = new TransactionPool();

        [TestMethod]
        public void ConstructsNewTransactionPool()
        {
            Assert.IsNotNull(_transactionPool);
            Assert.IsInstanceOfType(_transactionPool, typeof(TransactionPool));
        }

        [TestMethod]
        public void ConstructsTransactionPoolFromPool()
        {
            var pool = new Dictionary<Guid, Transaction>
            {
                {Guid.NewGuid(), new Transaction(null, null)}
            };

            _transactionPool = new TransactionPool(pool);

            Assert.IsNotNull(_transactionPool);
            Assert.IsInstanceOfType(_transactionPool, typeof(TransactionPool));
            Assert.AreEqual(pool, _transactionPool.Pool);
        }

        [TestMethod]
        public void TransactionPoolHasPool()
        {
            Assert.IsNotNull(_transactionPool.Pool);
            Assert.IsInstanceOfType(_transactionPool.Pool, typeof(Dictionary<Guid, Transaction>));
        }

        [TestMethod]
        public void TransactionPoolToStringReturnsValid()
        {
            var result = _transactionPool.ToString();

            var expectedResult = _transactionPool.Pool.ToString();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
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
            var differenTransactionPool = new TransactionPool();

            differenTransactionPool.Pool.Add(Guid.NewGuid(), new Transaction(null, null));

            var differentObject = (object)differenTransactionPool;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_transactionPool.Equals(differentObject));
        }

        [TestMethod]
        public void TransactionPoolAndObjectAreNotEqual()
        {
            var differentObject = new object();

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_transactionPool.Equals(differentObject));
        }

        [TestMethod]
        public void TransactionPoolAndNullAreNotEqual()
        {
            Assert.IsFalse(_transactionPool.Equals((object)null));
        }

        [TestMethod]
        public void TransactionPoolsHaveSameHashCode()
        {
            var sameTransactionPool = _transactionPool;

            Assert.IsNotNull(sameTransactionPool);
            Assert.IsTrue(_transactionPool.GetHashCode() == sameTransactionPool.GetHashCode());
        }

        [TestMethod]
        public void TransactionPoolsDoNotHaveSameHashCode()
        {
            var differentTransactionPool = new TransactionPool();

            Assert.IsNotNull(differentTransactionPool);
            Assert.IsFalse(_transactionPool.GetHashCode() == differentTransactionPool.GetHashCode());
        }
    }
}