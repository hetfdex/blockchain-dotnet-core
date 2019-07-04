using blockchain_dotnet_core.API.Models;
using blockchain_dotnet_core.API.Services;

namespace blockchain_dotnet_core.API.Controllers
{
    public class BlockchainController
    {
        private readonly IMessageProducer _messageProducer;

        private readonly IMessageConsumer _messageConsumer;

        private Blockchain _blockchain = new Blockchain();

        private TransactionPool _transactionPool = new TransactionPool();

        public BlockchainController(IMessageProducer messageProducer, IMessageConsumer messageConsumer)
        {
            _messageProducer = messageProducer;
            _messageConsumer = messageConsumer;
        }
    }
}