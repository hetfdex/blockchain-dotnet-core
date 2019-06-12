using blockchain_dotnet_core.API.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace blockchain_dotnet_core.Tests.Options
{
    [TestClass]
    public class ConfigurationOptionsTests
    {
        [TestMethod]
        public void ConfigurationOptionsHasCorrectValues()
        {
            Assert.AreEqual(1, ConfigurationOptions.InitialDifficulty);
            Assert.AreEqual(10, ConfigurationOptions.MiningRate);
            Assert.AreEqual(1000, ConfigurationOptions.StartBalance);
            //todo: add real miner address
            Assert.AreEqual(null, ConfigurationOptions.MinerAddress);
            Assert.AreEqual(50, ConfigurationOptions.MinerReward);
        }
    }
}