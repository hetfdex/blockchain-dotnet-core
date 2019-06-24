using blockchain_dotnet_core.API;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.Tests.Models
{
    [TestClass]
    public class TransactionInputTests
    {
        private readonly long _timestamp = TimestampUtils.GenerateTimestamp();

        private readonly decimal _amount = 0;

        private readonly string _signature = "test-signature";

        private ECPublicKeyParameters _address;

        private TransactionInput _transactionInput;

        [TestInitialize]
        public void TransactionInputTestsSetup()
        {
            var keyPair = CryptoUtils.GenerateKeyPair();

            _address = keyPair.Public as ECPublicKeyParameters;

            _transactionInput = new TransactionInput(_timestamp, _address, _amount, _signature);
        }

        [TestMethod]
        public void ConstructTransactionInputWithoutSignature()
        {
            var wallet = new Wallet();

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            var transactionInput = new TransactionInput(_timestamp, wallet.PublicKey, _amount, wallet.PrivateKey,
                transactionOutputs);

            var signature = CryptoUtils.GenerateSignature(wallet.PrivateKey, transactionOutputs.ToHash());

            Assert.IsNotNull(transactionInput);
            Assert.AreEqual(_timestamp, transactionInput.Timestamp);
            Assert.AreEqual(wallet.PublicKey, transactionInput.Address);
            Assert.AreEqual(_amount, transactionInput.Amount);
            Assert.IsTrue(CryptoUtils.VerifySignature(wallet.PublicKey, transactionOutputs.ToHash(), signature));
        }

        [TestMethod]
        public void ConstructTransactionInputWithoutSignatureNullAddressThrowsException()
        {
            var wallet = new Wallet();

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>();

            Assert.ThrowsException<ArgumentNullException>(() =>
                new TransactionInput(_timestamp, null, _amount, wallet.PrivateKey, transactionOutputs));
        }

        [TestMethod]
        public void ConstructTransactionInputWithSignature()
        {
            Assert.IsNotNull(_transactionInput);
            Assert.AreEqual(_timestamp, _transactionInput.Timestamp);
            Assert.AreEqual(_address, _transactionInput.Address);
            Assert.AreEqual(_amount, _transactionInput.Amount);
            Assert.AreEqual(_signature, _transactionInput.Signature);
        }

        [TestMethod]
        public void ConstructTransactionInputWithSignatureNullAddressThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new TransactionInput(_timestamp, null, _amount, _signature));
        }

        [TestMethod]
        public void ConstructTransactionInputWithSignatureNullSignatureThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new TransactionInput(_timestamp, _address, _amount, null));
        }

        [TestMethod]
        public void GetsMinerTransactionInput()
        {
            var minerTransactionInput = TransactionInput.GetMinerTransactionInput();

            var expectedTimestamp = 0L;

            var expectedMinerAddress = Constants.MinerAddress;

            var expectedMinerReward = Constants.MinerReward;

            var expectedSignature = "miner-reward";

            Assert.IsNotNull(minerTransactionInput);
            Assert.AreEqual(expectedTimestamp, minerTransactionInput.Timestamp);
            Assert.AreEqual(expectedMinerAddress, minerTransactionInput.Address);
            Assert.AreEqual(expectedMinerReward, minerTransactionInput.Amount);
            Assert.AreEqual(expectedSignature, minerTransactionInput.Signature);
        }

        [TestMethod]
        public void TransactionInputsAreEqual()
        {
            var sameObject = (object)_transactionInput;

            Assert.IsNotNull(sameObject);
            Assert.IsTrue(_transactionInput.Equals(sameObject));
        }

        [TestMethod]
        public void TransactionInputsAreNotEqual()
        {
            var keyPair = CryptoUtils.GenerateKeyPair();

            var differentTransactionInput =
                new TransactionInput(0, keyPair.Public as ECPublicKeyParameters, 10, string.Empty);

            var differentObject = (object)differentTransactionInput;

            Assert.IsNotNull(differentObject);
            Assert.IsFalse(_transactionInput.Equals(differentObject));
        }

        [TestMethod]
        public void TransactionInputAndNullAreNotEqual()
        {
            Assert.IsFalse(_transactionInput.Equals((object)null));
        }
    }
}