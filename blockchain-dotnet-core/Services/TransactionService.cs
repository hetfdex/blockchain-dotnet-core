using System;
using System.Collections.Generic;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IWalletService _walletService;

        public TransactionService(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public TransactionOutput GenerateTransactionOutput(Wallet senderWallet, ECPublicKeyParameters recipient,
            decimal amount)
        {
            TransactionOutput transactionOutput = new TransactionOutput();

            transactionOutput.Output.Add(recipient, amount);
            transactionOutput.Output.Add(senderWallet.PublicKey, senderWallet.Balance - amount);

            return transactionOutput;
        }

        public TransactionInput GenerateTransactionInput(Wallet senderWallet, TransactionOutput transactionOutput)
        {
            return new TransactionInput
            {
                Timestamp = TimestampUtils.GetTimestamp(),
                Address = senderWallet.PublicKey,
                Amount = senderWallet.Balance,
                Signature = _walletService.Sign(transactionOutput)
            };
        }

        public void Update(Transaction transaction, Wallet senderWallet, ECPublicKeyParameters recipient, decimal amount)
        {
            if (amount > transaction.TransactionOutput.Output[senderWallet.PublicKey])
            {
                return;
            }

            if (transaction.TransactionOutput.Output[recipient] == 0)
            {
                transaction.TransactionOutput.Output[recipient] = amount;
            }
            else
            {
                transaction.TransactionOutput.Output[recipient] += amount;
            }

            transaction.TransactionOutput.Output[senderWallet.PublicKey] -= amount;

            transaction.TransactionInput = GenerateTransactionInput(senderWallet, transaction.TransactionOutput);
        }

        public bool IsValidTransaction(Transaction transaction)
        {
            var transactionOutput = transaction.TransactionOutput;

            var transactionInput = transaction.TransactionInput;

            decimal outputTotal = 0;

            foreach (var output in transactionOutput.Output)
            {
                outputTotal += output.Value;
            }

            if (outputTotal != transactionInput.Amount)
            {
                return false;
            }

            if (!KeyPairUtils.VerifySignature(transactionInput.Address, transaction, transactionInput.Signature))
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

            var output = new Dictionary<ECPublicKeyParameters, decimal>();

            output.Add(minerWallet.PublicKey, Constants.MinerRewardAmmount);

            var transactionOutput = new TransactionOutput
            {
                Output = output
            };

            return new Transaction
            {
                TransactionInput = transactionInput,
                TransactionOutput = transactionOutput
            };
        }
    }
}