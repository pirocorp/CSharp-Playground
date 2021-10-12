namespace Send
{
    using System;
    using System.Text;
    using RabbitMQ.Client;

    public static class Send
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
                channel.QueueDeclare(queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                // The message content is a byte array, so you can encode whatever you like there.
                const string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "hello",
                    basicProperties: null,
                    body: body);

                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

            // When the code above finishes running, the channel and the connection will be disposed.
            // That's it for our publisher.
        }
    }
}
