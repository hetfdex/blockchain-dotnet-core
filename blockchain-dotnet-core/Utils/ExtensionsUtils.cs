using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using blockchain_dotnet_core.API.Models;

namespace blockchain_dotnet_core.API.Utils
{
    public static class ExtensionsUtils
    {
        public static string ToHashableString(this IList<Transaction> transactions)
        {
            if (transactions == null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            var result = string.Empty;

            foreach (var transaction in transactions)
            {
                result += transaction.Id;
                result += transaction.TransactionOutputs.ToHashableString();
                result += transaction.TransactionInput.Timestamp;
                result += transaction.TransactionInput.Address;
                result += transaction.TransactionInput.Amount;
                result += transaction.TransactionInput.Signature;
            }

            return result;
        }

        public static string ToHashableString(this IDictionary<ECPublicKeyParameters, decimal> transactionOutputs)
        {
            if (transactionOutputs == null)
            {
                throw new ArgumentNullException(nameof(transactionOutputs));
            }

            return JsonConvert.SerializeObject(transactionOutputs);
        }

        public static byte[] ToHash(this IDictionary<ECPublicKeyParameters, decimal> transactionOutputs)
        {
            if (transactionOutputs == null)
            {
                throw new ArgumentNullException(nameof(transactionOutputs));
            }

            return HashUtils.ComputeHash(transactionOutputs.ToHashableString());
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