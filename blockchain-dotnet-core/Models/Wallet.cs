using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.API.Models
{
    public class Wallet
    {
        public decimal Balance { get; set; }

        public AsymmetricCipherKeyPair KeyPair { get; set; }

        public ECPublicKeyParameters PublicKey { get; set; }
    }
}
