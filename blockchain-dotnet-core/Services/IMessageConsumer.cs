using blockchain_dotnet_core.API.Models;

namespace blockchain_dotnet_core.API.Services
{
    public interface IMessageConsumer
    {
        void ConsumeTransaction();

        void ConsumeBlockchain();
    }
}