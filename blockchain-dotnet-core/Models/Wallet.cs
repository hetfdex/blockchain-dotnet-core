using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace blockchain_dotnet_core.API.Models
{
    public class Wallet
    {
        public ECPrivateKeyParameters PrivateKey { get; set; }

        public ECPublicKeyParameters PublicKey { get; set; }

        public decimal Balance { get; set; }

        public Wallet(ECPrivateKeyParameters privateKey, ECPublicKeyParameters publicKey, decimal balance)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
            Balance = balance;
        }

        public override string ToString()
        {
            return Balance + PrivateKey.ToString() + PublicKey;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Balance, PrivateKey, PublicKey);

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