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

        public override string ToString()
        {
            return Timestamp + Address.ToString() + Amount + Signature;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Timestamp, Address, Amount, Signature);

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is TransactionInput))
            {
                return false;
            }

            return Equals((TransactionInput)obj);
        }

        public bool Equals(TransactionInput other)
        {
            return Timestamp == other.Timestamp && Address.Equals(other.Address) && Amount == other.Amount &&
                   Signature == other.Signature;
        }
    }
}