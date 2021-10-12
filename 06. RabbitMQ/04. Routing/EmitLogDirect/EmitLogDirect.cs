namespace EmitLogDirect
{
    using System;
    using System.Linq;
    using System.Text;
    using RabbitMQ.Client;

    public static class EmitLogDirect
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
                channel.ExchangeDeclare("direct_logs", ExchangeType.Direct);

                // The message content is a byte array, so you can encode whatever you like there.
                var message = GetMessage(args);
                var severity = (args.Length > 1) ? args[0] : "info";
                var body = Encoding.UTF8.GetBytes(message);

                // Publish messages to our logs exchange. 
                // We need to supply a routingKey when sending, but its value is ignored for fanout exchanges.
                channel.BasicPublish(
                    exchange: "direct_logs",
                    routingKey: severity,
                    basicProperties: null,
                    body: body);

                Console.WriteLine(" [x] Sent '{0}':'{1}'", severity, message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

            // When the code above finishes running, the channel and the connection will be disposed.
            // That's it for our publisher.
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 1) ? string.Join(" ", args.Skip(1)) : "Hello World!");
        }
    }
}
