using blockchain_dotnet_core.API.Models;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Utils
{
    public static class TransactionUtils
    {
        public static Dictionary<ECPublicKeyParameters, decimal> GenerateTransactionOutput(Wallet senderWallet, ECPublicKeyParameters recipient,
            decimal amount)
        {
            return new Dictionary<ECPublicKeyParameters, decimal>
            {
                {recipient, amount}, {senderWallet.PublicKey, senderWallet.Balance - amount}
            };
        }

        public static TransactionInput GenerateTransactionInput(Wallet senderWallet, Dictionary<ECPublicKeyParameters, decimal> transactionOutputs)
        {
            var privateKey = senderWallet.KeyPair.Private as ECPrivateKeyParameters;

            return new TransactionInput
            {
                Timestamp = TimestampUtils.GenerateTimestamp(),
                Address = senderWallet.PublicKey,
                Amount = senderWallet.Balance,
                Signature = KeyPairUtils.GenerateSignature(privateKey, transactionOutputs)
            };
        }

        public static Transaction GetMinerRewardTransaction(Wallet minerWallet)
        {
            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>
            {
                { minerWallet.PublicKey, Constants.MinerRewardAmount }
            };

            return new Transaction
            {
                TransactionInput = Constants.MinerTransactionInput,
                TransactionOutputs = transactionOutputs
            };
        }
    }
}