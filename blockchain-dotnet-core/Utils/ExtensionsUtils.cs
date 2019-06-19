using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Utils
{
    public static class ExtensionsUtils
    {
        public static byte[] ToHash(this IDictionary<ECPublicKeyParameters, decimal> transactionOutputs)
        {
            if (transactionOutputs == null)
            {
                throw new ArgumentNullException(nameof(transactionOutputs));
            }

            var serialized = JsonConvert.SerializeObject(transactionOutputs);

            return HashUtils.ComputeHash(serialized);
        }

        public static string ToBase64(this byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            return Convert.ToBase64String(bytes);
        }

        public static byte[] FromBase64(this string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException(nameof(s));
            }

            return Convert.FromBase64String(s);
        }
    }
}