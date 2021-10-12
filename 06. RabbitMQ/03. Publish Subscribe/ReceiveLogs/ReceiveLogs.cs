namespace ReceiveLogs
{
    using System;
    using System.Text;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public static class ReceiveLogs
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
            using var channel = connection.CreateModel();

            // Create an exchange of this type Fanout Exchange.
            // It just broadcasts all the messages it receives to all the queues it knows
            channel.ExchangeDeclare("logs", ExchangeType.Fanout);

            // Firstly, whenever we connect to Rabbit we need a fresh, empty queue.
            // To do this we could create a queue with a random name, or, even better -
            // let the server choose a random queue name for us. 
            // Secondly, once we disconnect the consumer the queue should be automatically deleted.
            // In the .NET client, when we supply no parameters to QueueDeclare() we create a
            // non-durable, exclusive, auto delete queue with a generated name.
            // At that point queueName contains a random queue name.
            // For example it may look like amq.gen-JzTY20BRgKO-HjmUJj0wLg
            var queueName = channel.QueueDeclare().QueueName;

            // Bindings
            // Relationship between exchange and a queue is called a binding.
            channel.QueueBind(
                queue: queueName,
                exchange: "logs",
                routingKey: "");

            Console.WriteLine(" [*] Waiting for logs.");

            // We're about to tell the server to deliver us the messages from the queue.
            var consumer = new EventingBasicConsumer(channel);

            // Since it will push us messages asynchronously, we provide a callback.
            // That is what EventingBasicConsumer.Received event handler does.
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0}", message);
            };

            // Tell the server to deliver us the messages from the queue.
            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
