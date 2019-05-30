using System;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.API.Models
{
    public class Transaction
    {
        public Guid Id { get; } = Guid.NewGuid();

        public ECPublicKeyParameters Sender { get; set; }

        public ECPublicKeyParameters Recipient { get; set; }

        public decimal Amount { get; set; }

        public TransactionOutput TransactionOutput { get; set; }

        public TransactionInput TransactionInput { get; set; }

    }
}