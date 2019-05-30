using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Services
{
    public class WalletService : IWalletService
    {
        private readonly Wallet _wallet;

        public WalletService()
        {
            _wallet = new Wallet
            {
                Balance = Constants.StartBalance,
                KeyPair = KeyPairUtils.GenerateKeyPair(),
                PublicKey = _wallet.KeyPair.Public as ECPublicKeyParameters
            };
        }

        public string Sign(Dictionary<ECPublicKeyParameters, decimal> transactionOutputs)
        {
            var ecPrivateKeyParameters = _wallet.KeyPair.Private as ECPrivateKeyParameters;

            return KeyPairUtils.GenerateSignature(ecPrivateKeyParameters, transactionOutputs);
        }

        public Transaction GenerateTransaction(ECPublicKeyParameters recipient, decimal amount, List<Block> blockchain)
        {
            if (blockchain != null)
            {
                _wallet.Balance = CalculateBalance(blockchain, _wallet.PublicKey);
            }

            if (amount > _wallet.Balance)
            {
                return null;
            }

            return new Transaction
            {
                Sender = _wallet.PublicKey,
                Recipient = recipient,
                Amount = amount
            };
        }

        public decimal CalculateBalance(List<Block> blockchain, ECPublicKeyParameters address)
        {
            var hasConductedTransaction = false;

            decimal outputsTotal = 0;

            for (int i = blockchain.Count - 1; i > 0; i--)
            {
                var block = blockchain[i];

                foreach (var transaction in block.Transactions)
                {
                    if (transaction.TransactionInput.Address.Equals(address))
                    {
                        hasConductedTransaction = true;
                    }

                    var addressOutput = transaction.TransactionOutputs[address];

                    if (addressOutput != 0)
                    {
                        outputsTotal -= addressOutput;
                    }

                    if (hasConductedTransaction)
                    {
                        break;
                    }
                }
            }
            return hasConductedTransaction ? outputsTotal : Constants.StartBalance - outputsTotal;
        }
    }
}