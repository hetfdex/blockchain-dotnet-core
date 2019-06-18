﻿using blockchain_dotnet_core.API.Extensions;
using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Options;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;

namespace blockchain_dotnet_core.API.Utils
{
    public static class TransactionUtils
    {
        public static Dictionary<ECPublicKeyParameters, decimal> GenerateTransactionOutput(Wallet senderWallet,
            ECPublicKeyParameters recipient,
            decimal amount)
        {
            return new Dictionary<ECPublicKeyParameters, decimal>
            {
                {recipient, amount}, {senderWallet.PublicKey, senderWallet.Balance - amount}
            };
        }

        public static TransactionInput GenerateTransactionInput(Wallet senderWallet,
            Dictionary<ECPublicKeyParameters, decimal> transactionOutputs)
        {
            return new TransactionInput(TimestampUtils.GenerateTimestamp(), senderWallet.PublicKey,
                senderWallet.Balance,
                KeyPairUtils.GenerateSignature(senderWallet.PrivateKey, transactionOutputs.ToHash()));
        }

        public static Transaction GetMinerRewardTransaction(Wallet minerWallet)
        {
            var transactionOutputs = new Dictionary<ECPublicKeyParameters, decimal>
            {
                {minerWallet.PublicKey, ConfigurationOptions.MinerReward}
            };

            return new Transaction(transactionOutputs, TransactionInputUtils.GetMinerTransactionInput());
        }
    }
}