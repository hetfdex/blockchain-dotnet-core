using System.Collections.Generic;
using blockchain_dotnet_core.API.Models;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.API.Services
{
    public interface IWalletService
    {
        string Sign(TransactionOutput transactionOutput);

        Transaction GenerateTransaction(ECPublicKeyParameters recipient, decimal amount,
            List<Block> blockchain);

        decimal CalculateBalance(List<Block> blockchain, ECPublicKeyParameters address);
    }
}
