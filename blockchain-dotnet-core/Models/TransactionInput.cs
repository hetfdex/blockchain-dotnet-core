using blockchain_dotnet_core.API.Utils;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Models
{
    public class TransactionInput
    {
        public long Timestamp { get; set; }

        public ECPublicKeyParameters Address { get; set; }

        public decimal Amount { get; set; }

        public string Signature { get; set; }

        public TransactionInput(long timestamp, ECPublicKeyParameters address, decimal amount,
            ECPrivateKeyParameters privateKey, IDictionary<ECPublicKeyParameters, decimal> transactionOutputs)
        {
            Timestamp = timestamp;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Amount = amount;
            Signature = CryptoUtils.GenerateSignature(privateKey, transactionOutputs.ToHash());
        }

        public TransactionInput(long timestamp, ECPublicKeyParameters address, decimal amount, string signature)
        {
            Timestamp = timestamp;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Amount = amount;
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));
        }

        public static TransactionInput GetMinerTransactionInput()
        {
            return new TransactionInput(0, Constants.MinerAddress,
                Constants.MinerReward, "miner-reward");
        }

        public override bool Equals(object obj)
        {
            var transactionInput = obj as TransactionInput;

            if (transactionInput == null)
            {
                return false;
            }

            return Equals(transactionInput);
        }

        public bool Equals(TransactionInput other)
        {
            return Timestamp.Equals(other.Timestamp) && Address.Equals(other.Address) && Amount.Equals(other.Amount) &&
                   Signature.Equals(other.Signature);
        }
    }
}