using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Extensions
{
    public static class TransactionExtensions
    {
        public static Dictionary<ECPublicKeyParameters, decimal> GenerateTransactionOutput(Wallet senderWallet, ECPublicKeyParameters recipient,
            decimal amount)
        {
            return new Dictionary<ECPublicKeyParameters, decimal>
            {
                {recipient, amount}, {senderWallet.PublicKey, senderWallet.Balance - amount}
            };
        }

        public static TransactionInput GenerateTransactionInput(Wallet senderWallet, Dictionary<ECPublicKeyParameters, decimal> transactionOutputs)
        {
            var privateKey = senderWallet.KeyPair.Private as ECPrivateKeyParameters;

            return new TransactionInput
            {
                Timestamp = TimestampUtils.GenerateTimestamp(),
                Address = senderWallet.PublicKey,
                Amount = senderWallet.Balance,
                Signature = Utils.KeyPairUtils.GenerateSignature(privateKey, transactionOutputs)
            };
        }

        public static void UpdateTransacions(this Transaction transaction, Wallet senderWallet, ECPublicKeyParameters recipient, decimal amount)
        {
            if (amount > transaction.TransactionOutputs[senderWallet.PublicKey])
            {
                return;
            }

            if (transaction.TransactionOutputs[recipient] == 0)
            {
                transaction.TransactionOutputs[recipient] = amount;
            }
            else
            {
                transaction.TransactionOutputs[recipient] += amount;
            }

            transaction.TransactionOutputs[senderWallet.PublicKey] -= amount;

            transaction.TransactionInput = GenerateTransactionInput(senderWallet, transaction.TransactionOutputs);
        }

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

            if (!KeyPairUtils.VerifySignature(transactionInput.Address, transaction.TransactionOutputs, transactionInput.Signature))
            {
                return false;
            }

            return true;
        }

        public static Transaction GetMinerRewardTransaction(Wallet minerWallet)
        {
            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>
            {
                { minerWallet.PublicKey, Constants.MinerRewardAmount }
            };

            return new Transaction
            {
                TransactionInput = Constants.MinerTransactionInput,
                TransactionOutputs = transactionOutputs
            };
        }
    }
}