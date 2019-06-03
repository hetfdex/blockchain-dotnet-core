using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Models
{
    public class Transaction
    {
        public Guid Id { get; } = Guid.NewGuid();

        public ECPublicKeyParameters Sender { get; set; }

        public ECPublicKeyParameters Recipient { get; set; }

        public decimal Amount { get; set; }

        public Dictionary<ECPublicKeyParameters, decimal> TransactionOutputs { get; set; }

        public TransactionInput TransactionInput { get; set; }

        public override string ToString()
        {
            return Id + Sender.ToString() + Recipient + Amount + TransactionOutputs + TransactionInput;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Id, Sender, Recipient, Amount, TransactionOutputs, TransactionInput);

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Transaction))
            {
                return false;
            }

            return Equals((Transaction)obj);
        }

        public bool Equals(Transaction other)
        {
            return Id == other.Id && Sender.Equals(other.Sender) && Recipient.Equals(other.Recipient) &&
                   Amount == other.Amount &&
                   TransactionOutputs == other.TransactionOutputs && TransactionInput.Equals(other.TransactionInput);
        }
    }
}