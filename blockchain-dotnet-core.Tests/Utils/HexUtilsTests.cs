using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class HexUtilsTests
    {
        private readonly byte[] _bytes = Encoding.Default.GetBytes("hetfdex");

        [TestMethod]
        public void ConvertsStringToBytes()
        {
            var result = HexUtils.StringToBytes("68657466646578");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.SequenceEqual(_bytes));
        }

        [TestMethod]
        public void ConvertsBytesToString()
        {
            var result = HexUtils.BytesToString(_bytes);

            var expectedResult = "68657466646578";

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }
    }
}