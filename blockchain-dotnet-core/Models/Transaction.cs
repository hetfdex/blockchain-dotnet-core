using blockchain_dotnet_core.API.Utils;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blockchain_dotnet_core.API.Models
{
    public class Transaction
    {
        public Guid Id { get; } = Guid.NewGuid();

        public IDictionary<ECPublicKeyParameters, decimal> TransactionOutputs { get; set; }

        public TransactionInput TransactionInput { get; set; }

        public Transaction(IDictionary<ECPublicKeyParameters, decimal> transactionOutputs,
            TransactionInput transactionInput)
        {
            TransactionOutputs = transactionOutputs ?? throw new ArgumentNullException(nameof(transactionOutputs));
            TransactionInput = transactionInput ?? throw new ArgumentNullException(nameof(transactionInput));
        }

        public Transaction(Wallet senderWallet, ECPublicKeyParameters recipient, decimal amount)
        {
            if (senderWallet == null)
            {
                throw new ArgumentNullException(nameof(senderWallet));
            }

            if (recipient == null)
            {
                throw new ArgumentNullException(nameof(recipient));
            }

            TransactionOutputs = GenerateTransactionOutput(senderWallet, recipient, amount);
            TransactionInput = GenerateTransactionInput(senderWallet, TransactionOutputs);
        }

        public bool IsValidTransaction()
        {
            decimal outputTotal = 0;

            foreach (var output in TransactionOutputs)
            {
                outputTotal += output.Value;
            }

            if (outputTotal != TransactionInput.Amount)
            {
                return false;
            }

            if (!CryptoUtils.VerifySignature(TransactionInput.Address, TransactionOutputs.ToHash(),
                TransactionInput.Signature))
            {
                return false;
            }

            return true;
        }

        public void UpdateTransaction(Wallet senderWallet,
            ECPublicKeyParameters recipient, decimal amount)
        {
            if (amount > TransactionOutputs[senderWallet.PublicKey])
            {
                return;
            }

            if (!TransactionOutputs.ContainsKey(recipient))
            {
                TransactionOutputs[recipient] = amount;
            }
            else
            {
                TransactionOutputs[recipient] += amount;
            }

            TransactionOutputs[senderWallet.PublicKey] -= amount;

            TransactionInput =
                GenerateTransactionInput(senderWallet, TransactionOutputs);
        }

        public static IDictionary<ECPublicKeyParameters, decimal> GenerateTransactionOutput(Wallet senderWallet,
            ECPublicKeyParameters recipient,
            decimal amount)
        {
            if (senderWallet == null)
            {
                throw new ArgumentNullException(nameof(senderWallet));
            }

            if (recipient == null)
            {
                throw new ArgumentNullException(nameof(recipient));
            }

            return new Dictionary<ECPublicKeyParameters, decimal>
            {
                {recipient, amount}, {senderWallet.PublicKey, senderWallet.Balance - amount}
            };
        }

        public static TransactionInput GenerateTransactionInput(Wallet senderWallet,
            IDictionary<ECPublicKeyParameters, decimal> transactionOutputs)
        {
            if (senderWallet == null)
            {
                throw new ArgumentNullException(nameof(senderWallet));
            }

            if (transactionOutputs == null)
            {
                throw new ArgumentNullException(nameof(transactionOutputs));
            }

            return new TransactionInput(TimestampUtils.GenerateTimestamp(), senderWallet.PublicKey,
                senderWallet.Balance,
                CryptoUtils.GenerateSignature(senderWallet.PrivateKey, transactionOutputs.ToHash()));
        }

        public static Transaction GetMinerRewardTransaction(Wallet minerWallet)
        {
            if (minerWallet == null)
            {
                throw new ArgumentNullException(nameof(minerWallet));
            }

            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>
            {
                {minerWallet.PublicKey, Constants.MinerReward}
            };

            return new Transaction(transactionOutputs, TransactionInput.GetMinerTransactionInput());
        }

        public override bool Equals(object obj)
        {
            var transaction = obj as Transaction;

            if (transaction == null)
            {
                return false;
            }

            return Equals(transaction);
        }

        public bool Equals(Transaction other)
        {
            return Id.Equals(other.Id) && TransactionOutputs.SequenceEqual(other.TransactionOutputs) &&
                   TransactionInput.Equals(other.TransactionInput);
        }
    }
}