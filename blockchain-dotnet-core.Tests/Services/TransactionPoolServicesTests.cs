using System.Collections.Generic;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Services;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.Tests.Services
{
    [TestClass]
    public class TransactionPoolServicesTests
    {
        private Wallet _wallet;

        private Transaction _transaction;

        [TestInitialize]
        public void TransactionPoolServiceTestsSetup()
        {
            var keyPair = KeyPairUtils.GenerateKeyPair();

            _wallet = new Wallet
            {
                Balance = 1000,
                KeyPair = keyPair,
                PublicKey = keyPair.Public as ECPublicKeyParameters
            };

            var transactionOutput = new Dictionary<ECPublicKeyParameters, decimal>();

            var transactionInput = new TransactionInput
            {
                Timestamp = TimestampUtils.GenerateTimestamp(),
                Address = keyPair.Public as ECPublicKeyParameters,
                Amount = 100,
                Signature = KeyPairUtils.GenerateSignature(keyPair.Private as ECPrivateKeyParameters, transactionOutput)
            };

            _transaction = new Transaction
            {
                TransactionInput = transactionInput,
                TransactionOutputs = transactionOutput
            };
        }

        [TestMethod]
        public void SetsTransaction()
        {
            TransactionPoolUtils.AddTransaction(_transaction);

            Assert.IsNotNull(TransactionPoolUtils.TransactionPool);
            Assert.AreEqual(_transaction, TransactionPoolUtils.TransactionPool[_transaction.Id]);
        }

        [TestMethod]
        public void FindsExistingTransaction()
        {
            TransactionPoolUtils.AddTransaction(_transaction);

            Assert.IsNotNull(TransactionPoolUtils.TransactionPool);
            Assert.IsTrue(TransactionPoolUtils.IsExistingTransaction(_transaction.Id));
        }

        [TestMethod]
        public void DoesNotFindMissingTransaction()
        {
            Assert.IsNotNull(TransactionPoolUtils.TransactionPool);
            Assert.IsFalse(TransactionPoolUtils.IsExistingTransaction(_transaction.Id));
        }

        [TestMethod]
        public void GetsValidTransactions()
        {
            //TODO
        }

        [TestMethod]
        public void ClearsTransactionPool()
        {
            TransactionPoolUtils.AddTransaction(_transaction);

            TransactionPoolUtils.ClearTransactions();

            Assert.IsNotNull(TransactionPoolUtils.TransactionPool);
            Assert.IsTrue(TransactionPoolUtils.TransactionPool.Count == 0);
        }

        [TestMethod]
        public void ClearsBlockchainTransactions()
        {
            //TODO
        }
    }
}
