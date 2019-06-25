using blockchain_dotnet_core.API;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace blockchain_dotnet_core.Tests
{
    [TestClass]
    public class ConstantsTests
    {
        [TestMethod]
        public void ConstantsHaveCorrectValues()
        {
            var expectedInitialDifficulty = 2;

            var expectedMiningRate = 10;

            var expectedStartBalance = 1000;

            var expectedMinerAddress = CryptoUtils.LoadPublicKey(
                "MIIBMzCB7AYHKoZIzj0CATCB4AIBATAsBgcqhkjOPQEBAiEA/////////////////////////////////////v///C8wRAQgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHBEEEeb5mfvncu6xVoGKVzocLBwKb/NstzijZWfKBWxb4F5hIOtp3JqPEZV2k+/wOEQio/Re0SKaFVBmcR9CP+xDUuAIhAP////////////////////66rtzmr0igO7/SXozQNkFBAgEBA0IABG+Iu+O3FJGQhZHBUVn+4/EEw41r13myLyTRqZfeklWN/VIiUjE5WC574vIV9tYErJf/tE2h51rH/5KB246NRfg=");

            var expectedMinerReward = 50;

            Assert.AreEqual(expectedInitialDifficulty, Constants.InitialDifficulty);
            Assert.AreEqual(expectedMiningRate, Constants.MiningRate);
            Assert.AreEqual(expectedStartBalance, Constants.StartBalance);
            Assert.AreEqual(expectedMinerAddress, Constants.MinerAddress);
            Assert.AreEqual(expectedMinerReward, Constants.MinerReward);
        }
    }
}