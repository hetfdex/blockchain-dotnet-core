using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Models
{
    public class Transaction
    {
        public Guid Id { get; } = Guid.NewGuid();

        public Dictionary<ECPublicKeyParameters, decimal> TransactionOutputs { get; set; }

        public TransactionInput TransactionInput { get; set; }

        public override string ToString()
        {
            return Id.ToString() + TransactionOutputs + TransactionInput;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Id, TransactionOutputs, TransactionInput);

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
            return Id.Equals(other.Id) && TransactionOutputs == other.TransactionOutputs &&
                   TransactionInput.Equals(other.TransactionInput);
        }
    }
}