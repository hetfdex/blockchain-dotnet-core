using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Parameters;

namespace blockchain_dotnet_core.API.Models
{
    public class TransactionOutput
    {
        public Dictionary<ECPublicKeyParameters, decimal> Output { get; set; }

        public Dictionary<ECPublicKeyParameters, decimal> Input { get; set; }
    }
}
