using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace blockchain_dotnet_core.API.Models
{
    public class Wallet
    {
        public decimal Balance { get; set; }

        public AsymmetricCipherKeyPair KeyPair { get; set; }

        public ECPublicKeyParameters PublicKey { get; set; }

        public override string ToString()
        {
            return Balance + KeyPair.ToString() + PublicKey;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Balance, KeyPair, PublicKey);

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
            return Balance.Equals(other.Balance) && KeyPair.Equals(other.KeyPair) && PublicKey.Equals(other.PublicKey);
        }
    }
}