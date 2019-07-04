using blockchain_dotnet_core.API.Models;
using Confluent.Kafka;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace blockchain_dotnet_core.API.Services
{
    public class MessageProducer : IMessageProducer
    {
        private static readonly string _bootstrapServers = "localhost:9092";

        private static readonly string _transactionTopic = "transaction";

        private static readonly string _blockchainTopic = "blockchain";

        private static readonly ProducerConfig _producerConfig = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers
        };

        public async Task ProduceTransaction(Transaction transaction)
        {
            var message = new Message<Null, string>
            {
                Value = JsonConvert.SerializeObject(transaction)
            };

            using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {
                await producer.ProduceAsync(_transactionTopic, message);
            }
        }

        public async Task ProduceBlockchain(Blockchain blockchain)
        {
            var message = new Message<Null, string>
            {
                Value = JsonConvert.SerializeObject(blockchain)
            };

            using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {
                await producer.ProduceAsync(_blockchainTopic, message);
            }
        }
    }
}