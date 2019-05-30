using blockchain_dotnet_core.API.Models;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Services
{
    public interface ITransactionService
    {
        Dictionary<ECPublicKeyParameters, decimal> GenerateTransactionOutput(Wallet senderWallet, ECPublicKeyParameters recipient,
            decimal amount);

        TransactionInput GenerateTransactionInput(Wallet senderWallet, Dictionary<ECPublicKeyParameters, decimal> transactionOutputs);

        void Update(Transaction transaction, Wallet senderWallet, ECPublicKeyParameters recipient, decimal amount);

        bool IsValidTransaction(Transaction transaction);

        Transaction GetMinerRewardTransaction(Wallet minerWallet);
    }
}