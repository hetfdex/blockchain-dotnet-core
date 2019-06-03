using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IWalletService _walletService;

        public TransactionService(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public Dictionary<ECPublicKeyParameters, decimal> GenerateTransactionOutput(Wallet senderWallet, ECPublicKeyParameters recipient,
            decimal amount)
        {
            return new Dictionary<ECPublicKeyParameters, decimal>
            {
                {recipient, amount}, {senderWallet.PublicKey, senderWallet.Balance - amount}
            };
        }

        public TransactionInput GenerateTransactionInput(Wallet senderWallet, Dictionary<ECPublicKeyParameters, decimal> transactionOutputs)
        {
            return new TransactionInput
            {
                Timestamp = TimestampUtils.GetTimestamp(),
                Address = senderWallet.PublicKey,
                Amount = senderWallet.Balance,
                Signature = _walletService.Sign(transactionOutputs)
            };
        }

        public void Update(Transaction transaction, Wallet senderWallet, ECPublicKeyParameters recipient, decimal amount)
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

        public bool IsValidTransaction(Transaction transaction)
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

        public Transaction GetMinerRewardTransaction(Wallet minerWallet)
        {
            var transactionInput = new TransactionInput
            {
                Address = null
            };

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>
            {
                { minerWallet.PublicKey, Constants.MinerRewardAmmount }
            };

            return new Transaction
            {
                TransactionInput = transactionInput,
                TransactionOutputs = transactionOutputs
            };
        }
    }
}