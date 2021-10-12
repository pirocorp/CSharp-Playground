namespace EmitLog
{
    using System;
    using System.Text;
    using RabbitMQ.Client;

    public static class EmitLog
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
            using (var channel = connection.CreateModel())
            {
                // Create an exchange of this type Fanout Exchange.
                // It just broadcasts all the messages it receives to all the queues it knows
                channel.ExchangeDeclare("logs", ExchangeType.Fanout);

                // The message content is a byte array, so you can encode whatever you like there.
                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);

                // Publish messages to our logs exchange. 
                // We need to supply a routingKey when sending, but its value is ignored for fanout exchanges.
                channel.BasicPublish(
                    exchange: "logs",
                    routingKey: "",
                    basicProperties: null,
                    body: body);

                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

            // When the code above finishes running, the channel and the connection will be disposed.
            // That's it for our publisher.
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "info: Hello World!");
        }
    }
}
