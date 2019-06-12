/*using blockchain_dotnet_core.API.Extensions;
using blockchain_dotnet_core.API.Models;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blockchain_dotnet_core.API.Utils
{
    public static class TransactionPoolUtils
    {
        public static Dictionary<Guid, Transaction> TransactionPool { get; set; } = new Dictionary<Guid, Transaction>();

        public static void AddTransaction(Transaction transaction)
        {
            TransactionPool.Add(transaction.Id, transaction);
        }

        public static List<Transaction> GetValidTransactions()
        {
            return TransactionPool.Where(t => t.Value.IsValidTransaction()).Select(t => t.Value).ToList();
        }

        public static bool IsExistingTransaction(ECPublicKeyParameters publicKey)
        {
            return TransactionPool.Any(t => t.Value.TransactionInput.Address.Equals(publicKey));
        }

        public static bool IsExistingTransaction(Guid id)
        {
            return TransactionPool.Any(t => t.Value.Id.Equals(id));
        }

        public static void ClearTransactions()
        {
            TransactionPool.Clear();
        }

        public static void ClearBlockchainTransactions(List<Block> blockchain)
        {
            foreach (var block in blockchain)
            {
                foreach (var transaction in block.Transactions)
                {
                    if (IsExistingTransaction(transaction.Id))
                    {
                        TransactionPool.Remove(transaction.Id);
                    }
                }
            }
        }
    }
}*/