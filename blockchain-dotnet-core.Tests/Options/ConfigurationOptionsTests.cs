using blockchain_dotnet_core.API.Options;
using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace blockchain_dotnet_core.Tests.Options
{
    [TestClass]
    public class ConfigurationOptionsTests
    {
        [TestMethod]
        public void ConfigurationOptionsHasCorrectValues()
        {
            var expectedMinerAddress = CryptoUtils.LoadPublicKey(
                "MIIBMzCB7AYHKoZIzj0CATCB4AIBATAsBgcqhkjOPQEBAiEA/////////////////////////////////////v///C8wRAQgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHBEEEeb5mfvncu6xVoGKVzocLBwKb/NstzijZWfKBWxb4F5hIOtp3JqPEZV2k+/wOEQio/Re0SKaFVBmcR9CP+xDUuAIhAP////////////////////66rtzmr0igO7/SXozQNkFBAgEBA0IABG+Iu+O3FJGQhZHBUVn+4/EEw41r13myLyTRqZfeklWN/VIiUjE5WC574vIV9tYErJf/tE2h51rH/5KB246NRfg=");

            Assert.AreEqual(1, ConfigurationOptions.InitialDifficulty);
            Assert.AreEqual(10, ConfigurationOptions.MiningRate);
            Assert.AreEqual(1000, ConfigurationOptions.StartBalance);
            Assert.AreEqual(expectedMinerAddress, ConfigurationOptions.MinerAddress);
            Assert.AreEqual(50, ConfigurationOptions.MinerReward);
        }
    }
}