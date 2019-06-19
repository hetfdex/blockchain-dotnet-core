using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blockchain_dotnet_core.API.Models
{
    public class TransactionPool
    {
        public IDictionary<Guid, Transaction> Pool { get; set; }

        public TransactionPool()
        {
            Pool = new Dictionary<Guid, Transaction>();
        }

        public TransactionPool(IDictionary<Guid, Transaction> pool)
        {
            Pool = pool ?? throw new ArgumentNullException(nameof(pool));
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            Pool.Add(transaction.Id, transaction);
        }

        public IList<Transaction> GetValidTransactions()
        {
            return Pool.Where(t => t.Value.IsValidTransaction())
                .Select(t => t.Value).ToList();
        }

        public bool IsExistingTransaction(ECPublicKeyParameters publicKey)
        {
            if (publicKey == null)
            {
                throw new ArgumentNullException(nameof(publicKey));
            }

            return Pool.Any(t => t.Value.TransactionInput.Address.Equals(publicKey));
        }

        public bool IsExistingTransaction(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return Pool.Any(t => t.Value.Id.Equals(id));
        }

        public void ClearTransactions()
        {
            Pool.Clear();
        }

        public void ClearBlockchainTransactions(Blockchain blockchain)
        {
            if (blockchain == null)
            {
                throw new ArgumentNullException(nameof(blockchain));
            }

            foreach (var block in blockchain.Chain)
            {
                foreach (var transaction in block.Transactions)
                {
                    if (IsExistingTransaction(transaction.Id))
                    {
                        Pool.Remove(transaction.Id);
                    }
                }
            }
        }

        public override bool Equals(object obj)
        {
            var transactionPool = obj as TransactionPool;

            if (transactionPool == null)
            {
                return false;
            }

            return Equals(transactionPool);
        }

        public bool Equals(TransactionPool other)
        {
            return Pool.SequenceEqual(other.Pool);
        }
    }
}