using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class HexUtilsTests
    {
        private string _testValue = "hetfdex";

        [TestMethod]
        public void StringToBytesIsValid()
        {
            var testHex = "68657466646578";

            var result = HexUtils.StringToBytes(testHex);

            var expectedResult = Encoding.Default.GetBytes(_testValue);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        [TestMethod]
        public void BytesToStringIsValid()
        {
            var testBytes = Encoding.Default.GetBytes(_testValue);

            var result = HexUtils.BytesToString(testBytes);

            var expectedResult = "68657466646578";

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }
    }
}