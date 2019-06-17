using System;
using System.Collections.Generic;
using System.Linq;

namespace blockchain_dotnet_core.API.Models
{
    public class TransactionPool
    {
        public Dictionary<Guid, Transaction> Pool { get; set; }

        public TransactionPool()
        {
            Pool = new Dictionary<Guid, Transaction>();
        }

        public TransactionPool(Dictionary<Guid, Transaction> pool)
        {
            Pool = pool;
        }

        public override string ToString()
        {
            return Pool.ToString();
        }

        public override int GetHashCode() => HashCode.Combine(Pool);

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