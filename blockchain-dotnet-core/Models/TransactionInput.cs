using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.API.Models
{
    public class TransactionInput
    {
        public long Timestamp { get; set; }

        public ECPublicKeyParameters Address { get; set; }

        public decimal Amount { get; set; }

        public string Signature { get; set; }
    }
}
