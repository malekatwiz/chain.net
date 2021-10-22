using Serilog;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Node
{
    public class BlockchainBus : IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private const string Exchange = "chain";
        private ILogger _logger;

        public BlockchainBus(ILogger logger)
        {
            _logger = logger;
            _connectionFactory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "chain",
                HostName = "localhost",
                DispatchConsumersAsync = true
            };

            CreateConnection();
        }

        private void CreateConnection()
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Subscribe(string nodeId, NodeCore node)
        {
            var queueName = $"node-{nodeId}";
            _channel.QueueDeclare(queueName, false, true, true);
            _channel.QueueBind(queueName, Exchange, string.Empty);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (obj, eventArgs) =>
            {
                var msgBody = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                var block = JsonConvert.DeserializeObject<Core.Block>(msgBody);
                _logger.Information($"received new block: {msgBody}");

                node.SyncChain(block);
                //_channel.BasicAck(eventArgs.DeliveryTag, false);

                await Task.Yield();
            };

            _channel.BasicConsume(queueName, true, consumer);
            _logger.Information("message consumed.");
        }

        public void Publish(string nodeId, Core.Block block)
        {
            CreateExchange();
            var msgProperties = _channel.CreateBasicProperties();
            var serializedBlock = JsonConvert.SerializeObject(block);
            var msgBody = Encoding.UTF8.GetBytes(serializedBlock);

            _channel.BasicPublish(Exchange, $"node-{nodeId}", msgProperties, msgBody);
            _logger.Information($"published new mined block: {serializedBlock}");
        }

        private void CreateExchange()
        {
            if (_connection?.CloseReason != null)
            {
                CreateConnection();
            }
            _channel.ExchangeDeclare(Exchange, "fanout", true);
            _logger.Information($"created exchange '{Exchange}'");
        }

        public void Dispose()
        {
            if (_connection?.IsOpen == true)
            {
                _channel.Close();
                _connection.Close();
                _logger.Information("Service bus connection closed.");
            }
        }
    }
}
