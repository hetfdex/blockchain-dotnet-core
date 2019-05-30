using System.Collections.Generic;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Utils;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.API.Services
{
    public class WalletService : IWalletService
    {
        private Wallet _wallet;

        public WalletService()
        {
            _wallet = new Wallet
            {
                Balance = Constants.StartBalance,
                KeyPair = KeyPairUtils.GenerateKeyPair(),
                PublicKey = _wallet.KeyPair.Public as ECPublicKeyParameters
            };
        }

        public string Sign(List<Transaction> transactions)
        {
            var ecPrivateKeyParameters = _wallet.KeyPair.Private as ECPrivateKeyParameters;

            return KeyPairUtils.GenerateSignature(ecPrivateKeyParameters, transactions);
        }
    }
}
