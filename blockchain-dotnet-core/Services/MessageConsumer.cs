using blockchain_dotnet_core.API.Models;
using Confluent.Kafka;
using Newtonsoft.Json;
using System.Threading;

namespace blockchain_dotnet_core.API.Services
{
    public class MessageConsumer : IMessageConsumer
    {
        private static readonly string _groupId = "test-consumer-group";

        private static readonly string _bootstrapServers = "localhost:9092";

        private static readonly string _transactionTopic = "transaction";

        private static readonly string _blockchainTopic = "blockchain";

        private readonly ConsumerConfig _consumerConfig = new ConsumerConfig
        {
            GroupId = _groupId,
            BootstrapServers = _bootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        public void ConsumeTransaction()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                consumer.Subscribe(_transactionTopic);

                var cancellationTokenSource = new CancellationTokenSource();

                while (true)
                {
                    var consumeResult = consumer.Consume(cancellationTokenSource.Token);

                    HandleMessage(_transactionTopic, consumeResult.Value);
                }
            }
        }

        public void ConsumeBlockchain()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                consumer.Subscribe(_blockchainTopic);

                var cancellationTokenSource = new CancellationTokenSource();

                while (true)
                {
                    var consumeResult = consumer.Consume(cancellationTokenSource.Token);

                    HandleMessage(_blockchainTopic, consumeResult.Value);
                }
            }
        }

        private void HandleMessage(string topic, string message)
        {
            if (topic == _transactionTopic)
            {
                var transaction = JsonConvert.DeserializeObject<Transaction>(message);
            }
            else if (topic == _blockchainTopic)
            {
                var blockchain = JsonConvert.DeserializeObject<Blockchain>(message);
            }
        }
    }
}