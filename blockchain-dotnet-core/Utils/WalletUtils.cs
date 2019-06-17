using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Options;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.API.Utils
{
    public static class WalletUtils
    {
        public static Transaction GenerateTransaction(Wallet wallet, ECPublicKeyParameters recipient, decimal amount,
            Blockchain blockchain)
        {
            if (blockchain != null)
            {
                wallet.Balance = CalculateBalance(blockchain, wallet.PublicKey);
            }

            if (amount > wallet.Balance)
            {
                return null;
            }

            var transactionOutputs = TransactionUtils.GenerateTransactionOutput(wallet, recipient, amount);
            var transactionInput = TransactionUtils.GenerateTransactionInput(wallet, transactionOutputs);

            return new Transaction(transactionOutputs, transactionInput);
        }

        public static decimal CalculateBalance(Blockchain blockchain, ECPublicKeyParameters address)
        {
            var hasConductedTransaction = false;

            decimal outputsTotal = 0;

            for (int i = blockchain.Chain.Count - 1; i > 0; i--)
            {
                var block = blockchain.Chain[i];

                foreach (var transaction in block.Transactions)
                {
                    if (transaction.TransactionInput?.Address != null &&
                        transaction.TransactionInput.Address.Equals(address))
                    {
                        hasConductedTransaction = true;
                    }

                    if (transaction.TransactionOutputs != null && transaction.TransactionOutputs.ContainsKey(address))
                    {
                        var addressOutput = transaction.TransactionOutputs[address];

                        if (addressOutput != 0)
                        {
                            outputsTotal += addressOutput;
                        }
                    }
                }

                if (hasConductedTransaction)
                {
                    break;
                }
            }

            return hasConductedTransaction ? outputsTotal : ConfigurationOptions.StartBalance + outputsTotal;
        }
    }
}