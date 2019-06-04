using blockchain_dotnet_core.API.Models;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Utils
{
    public static class WalletUtils
    {
        public static Transaction GenerateTransaction(Wallet wallet, ECPublicKeyParameters recipient, decimal amount, List<Block> blockchain)
        {
            var result = new Transaction();

            if (blockchain != null)
            {
                wallet.Balance = CalculateBalance(blockchain, wallet.PublicKey);
            }

            if (amount > wallet.Balance)
            {
                return null;
            }

            result.TransactionOutputs = TransactionUtils.GenerateTransactionOutput(wallet, recipient, amount);
            result.TransactionInput = TransactionUtils.GenerateTransactionInput(wallet, result.TransactionOutputs);

            return result;
        }

        public static decimal CalculateBalance(List<Block> blockchain, ECPublicKeyParameters address)
        {
            var hasConductedTransaction = false;

            decimal outputsTotal = 0;

            for (int i = blockchain.Count - 1; i > 0; i--)
            {
                var block = blockchain[i];

                foreach (var transaction in block.Transactions)
                {
                    if (transaction.TransactionInput != null && transaction.TransactionInput.Address.Equals(address))
                    {
                        hasConductedTransaction = true;
                    }

                    if (transaction.TransactionOutputs != null)
                    {
                        var addressOutput = transaction.TransactionOutputs[address];

                        if (addressOutput != 0)
                        {
                            outputsTotal += addressOutput;
                        }
                    }

                    if (hasConductedTransaction)
                    {
                        break;
                    }
                }
            }
            return hasConductedTransaction ? outputsTotal : Constants.StartBalance + outputsTotal;
        }
    }
}