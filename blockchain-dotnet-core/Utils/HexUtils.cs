using System;

namespace blockchain_dotnet_core.API.Utils
{
    public static class HexUtils
    {
        public static byte[] StringToBytes(string s)
        {
            var length = s.Length;

            var result = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                result[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            }

            return result;
        }

        public static string BytesToString(byte[] bytes) =>
            String.Join(string.Empty, Array.ConvertAll(bytes, b => b.ToString("x2")));
    }
}