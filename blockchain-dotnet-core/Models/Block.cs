using System;
using blockchain_dotnet_core.API.Utils;

namespace blockchain_dotnet_core.API.Models
{
    public class Block
    {
        public string Hash { get; set; }

        public string LastHash { get; set; }

        public long TimeStamp => (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        public string Data { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Hash) ? SHA256Util.ComputeSHA256(this) : Hash;
        }
    }
}
