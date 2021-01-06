using Confluent.Kafka;
using MDA.Infrastructure.Serialization;
using MDA.Infrastructure.Typing;
using MDA.Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus.Kafka
{
    public class KafkaAsyncMessageQueueService : IAsyncMessageQueueService
    {
        private readonly IKafkaProducerPool _connectionPool;
        private readonly IBinarySerializer _serializer;
        private readonly ILogger _logger;
        private readonly IKafkaConsumer _consumer;
        private readonly IServiceProvider _serviceProvider;

        public KafkaAsyncMessageQueueService(
            IKafkaProducerPool connectionPool,
            ILogger<KafkaAsyncMessageQueueService> logger,
            IBinarySerializer serializer,
            IKafkaConsumer consumer,
            IServiceProvider serviceProvider)
        {
            _connectionPool = connectionPool;
            _logger = logger;
            _serializer = serializer;
            _consumer = consumer;
            _serviceProvider = serviceProvider;
        }

        public MessageBusClientNames Name => MessageBusClientNames.Kafka;

        public async Task StartAsync(CancellationToken token = default)
        {
            _consumer.OnConsumeReceived += Consumer_Received;

            await _consumer.ConsumeAsync(token);
        }

        public async Task EnqueueAsync(IMessage message, CancellationToken token = default)
        {
            var topic = message.Topic;
            var producer = _connectionPool.Rent();

            try
            {
                var headers = new Headers
                {
                    new Header("MessageType", Encoding.UTF8.GetBytes(message.GetType().FullName ?? throw new InvalidOperationException()))
                };

                var result = await producer.ProduceAsync(topic, new Message<string, byte[]>
                {
                    Headers = headers,
                    Key = message.Id,
                    Value = _serializer.Serialize(message)
                }, token);

                if (result.Status == PersistenceStatus.Persisted ||
                    result.Status == PersistenceStatus.PossiblyPersisted)
                {
                    if (_logger.IsEnabled(LogLevel.Debug))
                        _logger.LogDebug($"The kafka message was published successfully, Topic: [{topic}], Message: [{message.Id}, {message.GetType().FullName}].");

                    return;
                }

                var jsonSerializer = _serviceProvider.GetService<IJsonSerializer>();

                _logger.LogError($"The kafka message publish failed, Topic: [{topic}], Message: {jsonSerializer.Serialize(message)}");
            }
            catch (Exception ex)
            {
                var jsonSerializer = _serviceProvider.GetService<IJsonSerializer>();

                _logger.LogError($"Publishing the kafka message, Topic: [{topic}], Message: {jsonSerializer.Serialize(message)}, has a unknown exception: {ex}.");
            }
            finally
            {
                var returned = _connectionPool.Return(producer);

                if (!returned)
                {
                    producer.Dispose();
                }
            }
        }

        public async Task StopAsync(CancellationToken token = default)
            => await Task.CompletedTask;

        private async void Consumer_Received(object sender, ConsumeResult<string, byte[]> eventArgs)
        {
            var kafkaMessage = eventArgs.Message;
            var messageTypeFullName = string.Empty;

            foreach (var header in kafkaMessage.Headers)
            {
                if (header.Key != "MessageType") continue;

                messageTypeFullName = Encoding.UTF8.GetString(header.GetValueBytes());
                break;
            }

            var typeResolver = _serviceProvider.GetService<ITypeResolver>();
            if (!typeResolver.TryResolveType(messageTypeFullName, out var messageType))
            {
                _logger.LogError($"Received kafka message, cannot resolve type: {messageTypeFullName}.");

                return;
            };

            var payload = _serializer.Deserialize(eventArgs.Message.Value, messageType);
            if (!(payload is IMessage message))
            {
                _logger.LogError($"Deserialized kafka message, incorrect message type,expected: {messageTypeFullName}, actual: {payload.GetType().FullName}.");

                return;
            };

            var handlerProxyTypeDefinition = typeof(IMessageHandlerProxy<>);
            var asyncHandlerProxyTypeDefinition = typeof(IAsyncMessageHandlerProxy<>);
            using (var scope = _serviceProvider.CreateScope())
            {
                var hasHandler = false;
                var scopeServiceProvider = scope.ServiceProvider;

                var handlerProxies = scopeServiceProvider.GetServices(handlerProxyTypeDefinition.MakeGenericType(messageType));
                if (handlerProxies.IsNotEmpty())
                {
                    hasHandler = true;

                    MessageHandlerUtils.DynamicInvokeHandle(handlerProxies, message, _logger);
                }

                var asyncHandlerProxies =
                    scopeServiceProvider.GetServices(asyncHandlerProxyTypeDefinition.MakeGenericType(messageType));
                if (asyncHandlerProxies.IsNotEmpty())
                {
                    hasHandler = true;

                    await MessageHandlerUtils.DynamicInvokeAsyncHandle(asyncHandlerProxies, message, _logger);
                }

                if (!hasHandler)
                {
                    _logger.LogError($"No message handler found: {messageTypeFullName}.");
                }
            }
        }
    }
}
