using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Org.BouncyCastle.Crypto.Parameters;

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

            if (!KeyPairUtils.VerifySignature(transactionInput.Address, transaction.TransactionOutputs, transactionInput.Signature))
            {
                return false;
            }

            return true;
        }

        public static void UpdateTransaction(this Transaction transaction, Wallet senderWallet, ECPublicKeyParameters recipient, decimal amount)
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

            transaction.TransactionInput = TransactionUtils.GenerateTransactionInput(senderWallet, transaction.TransactionOutputs);
        }
    }
}