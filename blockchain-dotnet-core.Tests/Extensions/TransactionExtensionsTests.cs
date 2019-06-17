using blockchain_dotnet_core.API.Extensions;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Options;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Extensions
{
    [TestClass]
    public class TransactionExtensionsTests
    {
        private Wallet _senderWallet;

        private ECPublicKeyParameters _recipientPublicKey;

        private Transaction _transaction;

        private readonly decimal _amount = 100;

        [TestInitialize]
        public void TransactionExtensionsTestsSetup()
        {
            var senderKeyPair = KeyPairUtils.GenerateKeyPair();

            _senderWallet = new Wallet(senderKeyPair.Private as ECPrivateKeyParameters,
                senderKeyPair.Public as ECPublicKeyParameters, ConfigurationOptions.StartBalance);

            var recipientKeyPair = KeyPairUtils.GenerateKeyPair();

            _recipientPublicKey = recipientKeyPair.Public as ECPublicKeyParameters;

            var transactionOutputs =
                TransactionUtils.GenerateTransactionOutput(_senderWallet, _recipientPublicKey, _amount);

            var transactionInput = TransactionUtils.GenerateTransactionInput(_senderWallet, transactionOutputs);

            _transaction = new Transaction(transactionOutputs, transactionInput);
        }

        [TestMethod]
        public void ConstructsTransaction()
        {
            Assert.IsNotNull(_transaction.Id);
            Assert.IsInstanceOfType(_transaction.Id, typeof(Guid));
            Assert.IsNotNull(_transaction.TransactionOutputs);
            Assert.IsInstanceOfType(_transaction.TransactionOutputs,
                typeof(Dictionary<ECPublicKeyParameters, decimal>));
            Assert.IsNotNull(_transaction.TransactionInput);
            Assert.IsInstanceOfType(_transaction.TransactionInput, typeof(TransactionInput));
        }

        [TestMethod]
        public void HasValidTransactionOutputs()
        {
            Assert.AreEqual(_amount, _transaction.TransactionOutputs[_recipientPublicKey]);
            Assert.AreEqual(_senderWallet.Balance - _amount, _transaction.TransactionOutputs[_senderWallet.PublicKey]);
        }

        [TestMethod]
        public void HasValidTransactionInput()
        {
            Assert.IsNotNull(_transaction.TransactionInput.Timestamp);
            Assert.IsInstanceOfType(_transaction.TransactionInput.Timestamp, typeof(long));
            Assert.IsNotNull(_transaction.TransactionInput.Address);
            Assert.IsInstanceOfType(_transaction.TransactionInput.Address, typeof(ECPublicKeyParameters));
            Assert.IsNotNull(_transaction.TransactionInput.Amount);
            Assert.IsInstanceOfType(_transaction.TransactionInput.Amount, typeof(decimal));
            Assert.IsNotNull(_transaction.TransactionInput.Signature);
            Assert.IsInstanceOfType(_transaction.TransactionInput.Signature, typeof(string));
            Assert.AreEqual(_senderWallet.PublicKey, _transaction.TransactionInput.Address);
            Assert.AreEqual(_senderWallet.Balance, _transaction.TransactionInput.Amount);
            Assert.IsTrue(KeyPairUtils.VerifySignature(_senderWallet.PublicKey,
                _transaction.TransactionOutputs.ToBytes(), _transaction.TransactionInput.Signature));
        }

        [TestMethod]
        public void TransactionIsValid()
        {
            Assert.IsTrue(_transaction.IsValidTransaction());
        }

        [TestMethod]
        public void TransactionHasInvalidOutputs()
        {
            _transaction.TransactionOutputs[_senderWallet.PublicKey] = 9999;

            Assert.IsFalse(_transaction.IsValidTransaction());
        }

        [TestMethod]
        public void TransactionHasInvalidInput()
        {
            var keyPair = KeyPairUtils.GenerateKeyPair();

            var publicKey = keyPair.Public as ECPublicKeyParameters;

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>()
            {
                {publicKey, 9999}
            };

            _transaction.TransactionInput.Signature = KeyPairUtils.GenerateSignature(_senderWallet.PrivateKey,
                transactionOutputs.ToBytes());

            Assert.IsFalse(_transaction.IsValidTransaction());
        }

        [TestMethod]
        public void UpdatesValidTransactionTwice()
        {
            var originalSignature = _transaction.TransactionInput.Signature;

            var originalSenderOutput = _transaction.TransactionOutputs[_senderWallet.PublicKey];

            var nextRecipientKeyPair = KeyPairUtils.GenerateKeyPair();

            var nextRecipientPublicKey = nextRecipientKeyPair.Public as ECPublicKeyParameters;

            var nextAmount = 50M;

            _transaction.UpdateTransaction(_senderWallet, nextRecipientPublicKey, nextAmount);

            Assert.AreEqual(nextAmount, _transaction.TransactionOutputs[nextRecipientPublicKey]);
            Assert.AreEqual(originalSenderOutput - nextAmount,
                _transaction.TransactionOutputs[_senderWallet.PublicKey]);
            Assert.AreNotEqual(originalSignature, _transaction.TransactionInput.Signature);

            originalSignature = _transaction.TransactionInput.Signature;

            _transaction.UpdateTransaction(_senderWallet, nextRecipientPublicKey, nextAmount);

            Assert.AreEqual(nextAmount * 2, _transaction.TransactionOutputs[nextRecipientPublicKey]);
            Assert.AreEqual(originalSenderOutput - nextAmount * 2,
                _transaction.TransactionOutputs[_senderWallet.PublicKey]);
            Assert.AreNotEqual(originalSignature, _transaction.TransactionInput.Signature);
        }

        [TestMethod]
        public void DoesNotUpdateInvalidTransaction()
        {
            var originalSignature = _transaction.TransactionInput.Signature;

            var originalSenderOutput = _transaction.TransactionOutputs[_senderWallet.PublicKey];

            var nextRecipientKeyPair = KeyPairUtils.GenerateKeyPair();

            var nextRecipientPublicKey = nextRecipientKeyPair.Public as ECPublicKeyParameters;

            var nextAmount = 9999M;

            _transaction.UpdateTransaction(_senderWallet, nextRecipientPublicKey, nextAmount);

            Assert.IsFalse(_transaction.TransactionOutputs.ContainsKey(nextRecipientPublicKey));
            Assert.AreNotEqual(originalSenderOutput - nextAmount,
                _transaction.TransactionOutputs[_senderWallet.PublicKey]);
            Assert.AreEqual(originalSignature, _transaction.TransactionInput.Signature);
        }
    }
}