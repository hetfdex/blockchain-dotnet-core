using blockchain_dotnet_core.API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blockchain_dotnet_core.API.Models
{
    public class Blockchain
    {
        public List<Block> Chain { get; set; }

        public Blockchain()
        {
            Chain = new List<Block>
            {
                BlockUtils.GetGenesisBlock()
            };
        }

        public override string ToString()
        {
            return Chain.ToString();
        }

        public override int GetHashCode() => HashCode.Combine(Chain);

        public override bool Equals(object obj)
        {
            var blockchain = obj as Blockchain;

            if (blockchain == null)
            {
                return false;
            }

            return Equals(blockchain);
        }

        public bool Equals(Blockchain other)
        {
            return Chain.SequenceEqual(other.Chain);
        }
    }
}