using blockchain_dotnet_core.API.Models;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.API.Services
{
    public interface ITransactionService
    {
        TransactionOutput GenerateTransactionOutput(Wallet senderWallet, ECPublicKeyParameters recipient,
            decimal amount);

        TransactionInput GenerateTransactionInput(Wallet senderWallet, TransactionOutput transactionOutput);

        void Update(Transaction transaction, Wallet senderWallet, ECPublicKeyParameters recipient, decimal amount);

        bool IsValidTransaction(Transaction transaction);

        Transaction GetMinerRewardTransaction(Wallet minerWallet);
    }
}
