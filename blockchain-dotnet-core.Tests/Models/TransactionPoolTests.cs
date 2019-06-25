﻿/*using blockchain_dotnet_core.API.Models;
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
        public void ConstructsNewTransactionPool()
        {
            Assert.IsNotNull(_transactionPool);
            Assert.IsNotNull(_transactionPool.Pool);
            Assert.IsInstanceOfType(_transactionPool, typeof(TransactionPool));
        }

        [TestMethod]
        public void ConstructsTransactionPoolFromPool()
        {
            var pool = new Dictionary<Guid, Transaction>();

            _transactionPool = new TransactionPool(pool);

            Assert.IsNotNull(_transactionPool);
            Assert.IsNotNull(_transactionPool.Pool);
            Assert.AreEqual(pool, _transactionPool.Pool);
        }

        [TestMethod]
        public void TransactionPoolHasPool()
        {
            Assert.IsInstanceOfType(_transactionPool.Pool, typeof(Dictionary<Guid, Transaction>));
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
}*/