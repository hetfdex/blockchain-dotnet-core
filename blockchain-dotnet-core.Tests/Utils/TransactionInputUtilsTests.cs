using System;
using System.Collections.Generic;
using System.Text;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Options;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class TransactionInputUtilsTests
    {
        [TestMethod]
        public void GetsMinerTransactionInput()
        {
            var expectedTimestamp = TimestampUtils.GenerateTimestamp();

            var expectedMinerAddress = ConfigurationOptions.MinerAddress;

            var expectedMinerReward = ConfigurationOptions.MinerReward;

            var expectedSignature = "miner-reward";

            var minerTransactionInput = TransactionInputUtils.GetMinerTransactionInput();

            Assert.IsNotNull(minerTransactionInput);
            Assert.IsInstanceOfType(minerTransactionInput, typeof(TransactionInput));
            Assert.AreEqual(expectedTimestamp, minerTransactionInput.Timestamp);
            Assert.AreEqual(expectedMinerAddress, minerTransactionInput.Address);
            Assert.AreEqual(expectedMinerReward, minerTransactionInput.Amount);
            Assert.AreEqual(expectedSignature, minerTransactionInput.Signature);
        }
    }
}
