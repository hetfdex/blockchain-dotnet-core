using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class TransactionTests
    {
        private readonly decimal _amount = 10;

        private IDictionary<ECPublicKeyParameters, decimal> _transactionOutputs;

        private TransactionInput _transactionInput;

        private Wallet _senderWallet;

        private Wallet _recipientWallet;

        private Transaction _transaction;

        [TestInitialize]
        public void TransactionTestsSetup()
        {
            _senderWallet = new Wallet();

            _recipientWallet = new Wallet();

            _transactionOutputs =
                Transaction.GenerateTransactionOutputs(_senderWallet, _recipientWallet.PublicKey, _amount);

            _transactionInput = Transaction.GenerateTransactionInput(_senderWallet, _transactionOutputs);

            _transaction = new Transaction(_transactionOutputs, _transactionInput);
        }

        [TestMethod]
        public void ConstructTransactionWithOutputAndInput()
        {
            Assert.IsNotNull(_transaction);
            Assert.AreEqual(_transactionOutputs, _transaction.TransactionOutputs);
            Assert.AreEqual(_transactionInput, _transaction.TransactionInput);
        }

        [TestMethod]
        public void ConstructTransactionWithOutputAndInputNullOutputThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Transaction(null, _transactionInput));
        }

        [TestMethod]
        public void ConstructTransactionWithOutputAndInputNullInputThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Transaction(_transactionOutputs, null));
        }

        [TestMethod]
        public void ConstructTransactionWithoutOutputAndInput()
        {
            var transaction = new Transaction(_senderWallet, _recipientWallet.PublicKey, _amount);

            Assert.IsNotNull(transaction);
            Assert.IsNotNull(transaction.TransactionOutputs);
            Assert.IsNotNull(transaction.TransactionInput);
        }

        [TestMethod]
        public void ConstructTransactionWithoutOutputAndInputNullWalletThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new Transaction(null, _recipientWallet.PublicKey, _amount));
        }

        [TestMethod]
        public void ConstructTransactionWithoutOutputAndInputNullRecipientThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Transaction(_senderWallet, null, _amount));
        }

        [TestMethod]
        public void TransactionIsValid()
        {
            Assert.IsTrue(_transaction.IsValidTransaction());
        }

        [TestMethod]
        public void TransactionIsValidInvalidOutputs()
        {
            _transaction.TransactionOutputs[_senderWallet.PublicKey] = 9999;

            Assert.IsFalse(_transaction.IsValidTransaction());
        }

        [TestMethod]
        public void TransactionIsValidInvalidInput()
        {
            var fakeWallet = new Wallet();

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>
            {
                {fakeWallet.PublicKey, 9999}
            };

            _transaction.TransactionInput.Signature = CryptoUtils.GenerateSignature(_senderWallet.PrivateKey,
                transactionOutputs.ToHash());

            Assert.IsFalse(_transaction.IsValidTransaction());
        }

        [TestMethod]
        public void UpdateTransactionTwice()
        {
            var originalSignature = _transaction.TransactionInput.Signature;

            var originalSenderOutput = _transaction.TransactionOutputs[_senderWallet.PublicKey];

            var nextRecipientWallet = new Wallet();

            var nextAmount = 50m;

            _transaction.UpdateTransaction(_senderWallet, nextRecipientWallet.PublicKey, nextAmount);

            Assert.AreEqual(nextAmount, _transaction.TransactionOutputs[nextRecipientWallet.PublicKey]);
            Assert.AreEqual(originalSenderOutput - nextAmount,
                _transaction.TransactionOutputs[_senderWallet.PublicKey]);
            Assert.AreNotEqual(originalSignature, _transaction.TransactionInput.Signature);

            originalSignature = _transaction.TransactionInput.Signature;

            _transaction.UpdateTransaction(_senderWallet, nextRecipientWallet.PublicKey, nextAmount);

            Assert.AreEqual(nextAmount * 2, _transaction.TransactionOutputs[nextRecipientWallet.PublicKey]);
            Assert.AreEqual(originalSenderOutput - nextAmount * 2,
                _transaction.TransactionOutputs[_senderWallet.PublicKey]);
            Assert.AreNotEqual(originalSignature, _transaction.TransactionInput.Signature);
        }

        [TestMethod]
        public void UpdateTransactionInvalidTransaction()
        {
            var originalSignature = _transaction.TransactionInput.Signature;

            var originalSenderOutput = _transaction.TransactionOutputs[_senderWallet.PublicKey];

            var nextRecipientWallet = new Wallet();

            var nextAmount = 9999m;

            _transaction.UpdateTransaction(_senderWallet, nextRecipientWallet.PublicKey, nextAmount);

            Assert.IsFalse(_transaction.TransactionOutputs.ContainsKey(nextRecipientWallet.PublicKey));
            Assert.AreNotEqual(originalSenderOutput - nextAmount,
                _transaction.TransactionOutputs[_senderWallet.PublicKey]);
            Assert.AreEqual(originalSignature, _transaction.TransactionInput.Signature);
        }

        [TestMethod]
        public void UpdateTransactionNullWalletThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _transaction.UpdateTransaction(null, _recipientWallet.PublicKey, 50));
        }

        [TestMethod]
        public void UpdateTransactionNullRecipientThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _transaction.UpdateTransaction(_senderWallet, null, 50));
        }

        [TestMethod]
        public void GenerateTransactionOutputs()
        {
            var transactionOutputs =
                Transaction.GenerateTransactionOutputs(_senderWallet, _recipientWallet.PublicKey, _amount);

            Assert.IsNotNull(transactionOutputs);
            Assert.IsTrue(transactionOutputs.Count == 2);
            Assert.AreEqual(_amount, transactionOutputs[_recipientWallet.PublicKey]);
            Assert.AreEqual(_senderWallet.Balance - _amount, transactionOutputs[_senderWallet.PublicKey]);
        }

        [TestMethod]
        public void GenerateTransactionOutputsNullWalletThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                Transaction.GenerateTransactionOutputs(null, _recipientWallet.PublicKey, _amount));
        }

        [TestMethod]
        public void GenerateTransactionOutputsNullRecipientThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                Transaction.GenerateTransactionOutputs(_senderWallet, null, _amount));
        }

        [TestMethod]
        public void GenerateTransactionInput()
        {
            var transactionOutputs =
                Transaction.GenerateTransactionOutputs(_senderWallet, _recipientWallet.PublicKey, _amount);

            var transactionInput =
                Transaction.GenerateTransactionInput(_senderWallet, transactionOutputs);

            Assert.IsNotNull(transactionInput);
            Assert.AreEqual(_senderWallet.PublicKey, transactionInput.Address);
            Assert.AreEqual(_senderWallet.Balance, transactionInput.Amount);
            Assert.IsTrue(CryptoUtils.VerifySignature(_senderWallet.PublicKey,
                transactionOutputs.ToHash(), transactionInput.Signature));
        }

        [TestMethod]
        public void GenerateTransactionInputNullWalletThrowsException()
        {
            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            Assert.ThrowsException<ArgumentNullException>(() =>
                Transaction.GenerateTransactionInput(null, transactionOutputs));
        }

        [TestMethod]
        public void GenerateTransactionInputNullOutputsThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                Transaction.GenerateTransactionInput(_senderWallet, null));
        }

        [TestMethod]
        public void GetMinerRewardTransaction()
        {
            var minerWallet = new Wallet();

            var rewardTransaction = Transaction.GetMinerRewardTransaction(minerWallet);

            Assert.AreEqual(TransactionInput.GetMinerTransactionInput(), rewardTransaction.TransactionInput);
            Assert.AreEqual(TransactionInput.GetMinerTransactionInput().Amount,
                rewardTransaction.TransactionOutputs[minerWallet.PublicKey]);
        }

        [TestMethod]
        public void GetMinerRewardTransactionTransactionNullWalletThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                Transaction.GetMinerRewardTransaction(null));
        }

        [TestMethod]
        public void TransactionsAreEqual()
        {
            var sameObject = (object)_transaction;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_transaction.Equals(sameObject));
        }

        [TestMethod]
        public void TransactionsAreNotEqualDifferentProperties()
        {
            var differentTransaction =
                new Transaction(new Dictionary<ECPublicKeyParameters, decimal>(), _transactionInput);

            var differentObject = (object)differentTransaction;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_transaction.Equals(differentObject));
        }

        [TestMethod]
        public void TransactionAndNullAreNotEqual()
        {
            Assert.IsFalse(_transaction.Equals((object)null));
        }
    }
}