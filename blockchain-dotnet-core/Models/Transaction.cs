using System;

namespace blockchain_dotnet_core.API.Models
{
    public class Transaction
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string OutputMap { get; set; }

        public string Input { get; set; }

    }
}