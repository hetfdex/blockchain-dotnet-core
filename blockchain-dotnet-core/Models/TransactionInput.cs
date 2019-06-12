using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace blockchain_dotnet_core.API.Models
{
    public class TransactionInput
    {
        public long Timestamp { get; set; }

        public ECPublicKeyParameters Address { get; set; }

        public decimal Amount { get; set; }

        public string Signature { get; set; }

        public TransactionInput(long timestamp, ECPublicKeyParameters address, decimal amount, string signature)
        {
            Timestamp = timestamp;
            Address = address;
            Amount = amount;
            Signature = signature;
        }

        public override string ToString()
        {
            return Timestamp + Address.ToString() + Amount + Signature;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Timestamp, Address, Amount, Signature);

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