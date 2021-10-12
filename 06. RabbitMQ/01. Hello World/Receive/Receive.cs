namespace Receive
{
    using System;
    using System.Text;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    // As for the consumer, it is listening for messages from RabbitMQ.
    // So unlike the publisher which publishes a single message,
    // we'll keep the consumer running continuously to listen for messages and print them out.
    public static class Receive
    {
        public static void Main()
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
            using (var channel = connection.CreateModel())
            {
                // To send, we must declare a queue for us to send to; then we can publish a message to the queue:
                // Declaring a queue is idempotent - it will only be created if it doesn't exist already.
                // Because we might start the consumer before the publisher, we want to make sure the queue
                // exists before we try to consume messages from it.
                channel.QueueDeclare(queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                // We're about to tell the server to deliver us the messages from the queue.
                var consumer = new EventingBasicConsumer(channel);

                // Since it will push us messages asynchronously, we provide a callback.
                // That is what EventingBasicConsumer.Received event handler does.
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };

                // Tell the server to deliver us the messages from the queue.
                channel.BasicConsume(queue: "hello",
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
