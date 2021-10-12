namespace ReceiveLogsDirect
{
    using System;
    using System.Text;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public static class ReceiveLogsDirect
    {
        public static void Main(string[] args)
        {
            // Factory creates connections to a RabbitMQ node on the local machine - hence the localhost.
            // If we wanted to connect to a node on a different machine we'd
            // simply specify its hostname or IP address here.
            var factory = new ConnectionFactory() { HostName = "localhost" };

            //The connection abstracts the socket connection,
            //and takes care of protocol version negotiation
            //and authentication and so on for us.
            using var connection = factory.CreateConnection();

            // Next we create a channel, which is where most of the API for getting things done resides.
            using var channel = connection.CreateModel();

            // Create an exchange of this type Fanout Exchange.
            // It just broadcasts all the messages it receives to all the queues it knows
            channel.ExchangeDeclare("direct_logs", ExchangeType.Direct);

            // Firstly, whenever we connect to Rabbit we need a fresh, empty queue.
            // To do this we could create a queue with a random name, or, even better -
            // let the server choose a random queue name for us. 
            // Secondly, once we disconnect the consumer the queue should be automatically deleted.
            // In the .NET client, when we supply no parameters to QueueDeclare() we create a
            // non-durable, exclusive, auto delete queue with a generated name.
            // At that point queueName contains a random queue name.
            // For example it may look like amq.gen-JzTY20BRgKO-HjmUJj0wLg
            var queueName = channel.QueueDeclare().QueueName;

            if(args.Length < 1)
            {
                EndProgram();

                return;
            }

            // Bindings
            // Relationship between exchange and a queue is called a binding.
            // This can be simply read as: the queue is interested in messages from this exchange.
            foreach (var severity in args)
            {
                // We're going to create a new binding for each severity we're interested in.
                channel.QueueBind(
                    queue: queueName,
                    exchange: "direct_logs",
                    routingKey: severity);
            }

            Console.WriteLine(" [*] Waiting for logs.");

            // We're about to tell the server to deliver us the messages from the queue.
            var consumer = new EventingBasicConsumer(channel);

            // Since it will push us messages asynchronously, we provide a callback.
            // That is what EventingBasicConsumer.Received event handler does.
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;

                Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);
            };

            // Tell the server to deliver us the messages from the queue.
            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static void EndProgram()
        {
            Console.Error.WriteLine("Usage: {0} [info] [warning] [error]", Environment.GetCommandLineArgs()[0]);
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
            Environment.ExitCode = 1;
        }
    }
}
