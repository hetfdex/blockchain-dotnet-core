using blockchain_dotnet_core.API.Models;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Services
{
    public interface IWalletService
    {
        string Sign(Dictionary<ECPublicKeyParameters, decimal> transactionOutputs);

        Transaction GenerateTransaction(ECPublicKeyParameters recipient, decimal amount,
            List<Block> blockchain);

        decimal CalculateBalance(List<Block> blockchain, ECPublicKeyParameters address);
    }
}