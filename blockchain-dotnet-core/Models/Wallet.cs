using blockchain_dotnet_core.API.Utils;
using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace blockchain_dotnet_core.API.Models
{
    public class Wallet
    {
        public ECPrivateKeyParameters PrivateKey { get; set; }

        public ECPublicKeyParameters PublicKey { get; set; }

        public decimal Balance { get; set; }

        public Wallet()
        {
            var keyPair = CryptoUtils.GenerateKeyPair();

            PrivateKey = keyPair.Private as ECPrivateKeyParameters;

            PublicKey = keyPair.Public as ECPublicKeyParameters;

            Balance = Constants.StartBalance;
        }

        public Wallet(ECPrivateKeyParameters privateKey, ECPublicKeyParameters publicKey, decimal balance)
        {
            PrivateKey = privateKey ?? throw new ArgumentNullException(nameof(privateKey));
            PublicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
            Balance = balance;
        }

        public Transaction GenerateTransaction(ECPublicKeyParameters recipient, decimal amount,
            Blockchain blockchain)
        {
            if (recipient == null)
            {
                throw new ArgumentNullException(nameof(recipient));
            }

            if (blockchain != null)
            {
                Balance = CalculateBalance(blockchain, PublicKey);
            }

            if (amount > Balance)
            {
                return null;
            }

            var transactionOutputs = Transaction.GenerateTransactionOutput(this, recipient, amount);
            var transactionInput = Transaction.GenerateTransactionInput(this, transactionOutputs);

            return new Transaction(transactionOutputs, transactionInput);
        }

        public static decimal CalculateBalance(Blockchain blockchain, ECPublicKeyParameters address)
        {
            if (blockchain == null)
            {
                throw new ArgumentNullException(nameof(blockchain));
            }

            var hasConductedTransaction = false;

            var outputsTotal = 0m;

            for (int i = blockchain.Chain.Count - 1; i > 0; i--)
            {
                var block = blockchain.Chain[i];

                foreach (var transaction in block.Transactions)
                {
                    if (transaction.TransactionInput.Address.Equals(address))
                    {
                        hasConductedTransaction = true;
                    }

                    if (transaction.TransactionOutputs.ContainsKey(address))
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

            return hasConductedTransaction ? outputsTotal : Constants.StartBalance + outputsTotal;
        }

        public override bool Equals(object obj)
        {
            var wallet = obj as Wallet;

            if (wallet == null)
            {
                return false;
            }

            return Equals(wallet);
        }

        public bool Equals(Wallet other)
        {
            return Balance.Equals(other.Balance) && PrivateKey.Equals(other.PrivateKey) &&
                   PublicKey.Equals(other.PublicKey);
        }
    }
}