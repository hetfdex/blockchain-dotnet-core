using blockchain_dotnet_core.API.Models;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blockchain_dotnet_core.API.Extensions
{
    public static class TransactionPoolExtensions
    {
        public static void AddTransaction(this TransactionPool transactionPool, Transaction transaction)
        {
            transactionPool.Pool.Add(transaction.Id, transaction);
        }

        public static List<Transaction> GetValidTransactions(this TransactionPool transactionPool)
        {
            return transactionPool.Pool.Where(t => t.Value.IsValidTransaction())
                .Select(t => t.Value).ToList();
        }

        public static bool IsExistingTransaction(this TransactionPool transactionPool, ECPublicKeyParameters publicKey)
        {
            return transactionPool.Pool.Any(t => t.Value.TransactionInput.Address.Equals(publicKey));
        }

        public static bool IsExistingTransaction(this TransactionPool transactionPool, Guid id)
        {
            return transactionPool.Pool.Any(t => t.Value.Id.Equals(id));
        }

        public static void ClearTransactions(this TransactionPool transactionPool)
        {
            transactionPool.Pool.Clear();
        }

        public static void ClearBlockchainTransactions(this TransactionPool transactionPool, Blockchain blockchain)
        {
            foreach (var block in blockchain.Chain)
            {
                foreach (var transaction in block.Transactions)
                {
                    if (transactionPool.IsExistingTransaction(transaction.Id))
                    {
                        transactionPool.Pool.Remove(transaction.Id);
                    }
                }
            }
        }
    }
}