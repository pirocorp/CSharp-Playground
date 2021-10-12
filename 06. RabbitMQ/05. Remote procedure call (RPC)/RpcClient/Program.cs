namespace RpcClient
{
    using System;
    using System.Collections.Concurrent;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("RPC Client");

            var n = args.Length > 0 ? args[0] : "30";
            await InvokeAsync(n);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static async Task InvokeAsync(string n)
        {
            var rpcClient = new RpcClient();

            Console.WriteLine(" [x] Requesting fib({0})", n);
            var response = await rpcClient.CallAsync(n);
            Console.WriteLine(" [.] Got '{0}'", response);

            rpcClient.Close();
        }
    }

    public class RpcClient : IDisposable
    {
        private const string QUEUE_NAME = "rpc_queue";

        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;

        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper;

        public RpcClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            this.connection = factory.CreateConnection();
            this.channel = this.connection.CreateModel();
            this.replyQueueName = this.channel.QueueDeclare().QueueName;

            this.callbackMapper = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

            this.ConfigureConsumer();
        }

        public Task<string> CallAsync(string message, CancellationToken cancellationToken = default)
        {
            var props = this.channel.CreateBasicProperties();

            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = this.replyQueueName;

            var messageBytes = Encoding.UTF8.GetBytes(message);

            var tcs = new TaskCompletionSource<string>();
            this.callbackMapper.TryAdd(correlationId, tcs);

            this.channel.BasicPublish(
                exchange: "",
                routingKey: QUEUE_NAME,
                basicProperties: props,
                body: messageBytes);

            cancellationToken.Register(() => this.callbackMapper.TryRemove(correlationId, out var tmp));
            return tcs.Task;
        }

        public void Close()
        {
            this.channel.Close();
            this.connection.Close();
        }

        public void Dispose()
        {
            this.channel.Dispose();
            this.connection.Dispose();
        }

        private void ConfigureConsumer()
        {
            var consumer = new EventingBasicConsumer(this.channel);

            consumer.Received += (model, ea) =>
            {
                if (!this.callbackMapper.TryRemove(
                    ea.BasicProperties.CorrelationId, 
                    out var tcs))
                {
                    return;
                }

                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);

                tcs.TrySetResult(response);
            };

            this.channel.BasicConsume(
                consumer: consumer,
                queue: this.replyQueueName,
                autoAck: true);
        }
    }
}
