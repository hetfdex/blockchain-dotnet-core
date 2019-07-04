using blockchain_dotnet_core.API.Models;
using System.Threading.Tasks;

namespace blockchain_dotnet_core.API.Services
{
    public interface IMessageProducer
    {
        Task ProduceTransaction(Transaction transaction);

        Task ProduceBlockchain(Blockchain blockchain);
    }
}