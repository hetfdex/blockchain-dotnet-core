using System;
using System.Linq;

namespace blockchain_dotnet_core.API.Utils
{
    public static class HexUtils
    {
        public static byte[] FromHex(string hex)
        {
            var chars = hex.Length;

            var result = new byte[chars / 2];

            for (int i = 0; i < chars; i += 2)
            {
                result[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return result;
        }

        public static string ToHex(byte[] bytes) => String.Concat(bytes.Select(x => x.ToString("x2")));
    }
}