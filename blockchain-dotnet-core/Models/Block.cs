using blockchain_dotnet_core.API.Utils;

namespace blockchain_dotnet_core.API.Models
{
    public class Block
    {
        public long Timestamp { get; set; }

        public string LastHash { get; set; }

        public string Hash { get; set; }

        public string Data { get; set; }

        public int Nonce { get; set; }

        public int Difficulty { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Hash) ? SHA256Util.ComputeSHA256(this) : Hash;
        }
    }
}