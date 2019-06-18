using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace blockchain_dotnet_core.API.Extensions
{
    public static class TransactionExtensions
    {
        public static bool IsValidTransaction(this Transaction transaction)
        {
            var transactionOutputs = transaction.TransactionOutputs;

            var transactionInput = transaction.TransactionInput;

            decimal outputTotal = 0;

            foreach (var output in transactionOutputs)
            {
                outputTotal += output.Value;
            }

            if (outputTotal != transactionInput.Amount)
            {
                return false;
            }

            if (!KeyPairUtils.VerifySignature(transactionInput.Address, transaction.TransactionOutputs.ToHash(),
                transactionInput.Signature))
            {
                return false;
            }

            return true;
        }

        public static void UpdateTransaction(this Transaction transaction, Wallet senderWallet,
            ECPublicKeyParameters recipient, decimal amount)
        {
            if (amount > transaction.TransactionOutputs[senderWallet.PublicKey])
            {
                return;
            }

            if (!transaction.TransactionOutputs.ContainsKey(recipient))
            {
                transaction.TransactionOutputs[recipient] = amount;
            }
            else
            {
                transaction.TransactionOutputs[recipient] += amount;
            }

            transaction.TransactionOutputs[senderWallet.PublicKey] -= amount;

            transaction.TransactionInput =
                TransactionUtils.GenerateTransactionInput(senderWallet, transaction.TransactionOutputs);
        }

        public static byte[] ToHash(this Dictionary<ECPublicKeyParameters, decimal> transactionOutputs)
        {
            var serializable = new Dictionary<string, decimal>();

            foreach (var transactionOutput in transactionOutputs)
            {
                serializable[transactionOutput.Key.ToString()] = transactionOutput.Value;
            }

            var binaryFormatter = new BinaryFormatter();

            var memoryStream = new MemoryStream();

            binaryFormatter.Serialize(memoryStream, serializable);

            var bytes = memoryStream.ToArray();

            return HashUtils.ComputeHash(bytes);
        }
    }
}