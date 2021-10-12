namespace RPCServer
{
    using System;
    using System.Text;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public static class RPCServer
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

            // To send, we must declare a queue for us to send to; then we can publish a message to the queue:
            // Declaring a queue is idempotent - it will only be created if it doesn't exist already.
            // Because we might start the consumer before the publisher, we want to make sure the queue
            // exists before we try to consume messages from it.
            channel.QueueDeclare(
                queue: "rpc_queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // This tells RabbitMQ not to give more than one message to a worker at a time.
            // Or, in other words, don't dispatch a new message to a worker until it
            // has processed and acknowledged the previous one. Instead, it will dispatch
            // it to the next worker that is not still busy.
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, false);

            // We're about to tell the server to deliver us the messages from the queue.
            var consumer = new EventingBasicConsumer(channel);

            
            // Tell the server to deliver us the messages from the queue.
            // Manual message acknowledgments are turned on by default. (autoAck: false)
            channel.BasicConsume(
                queue: "rpc_queue",
                autoAck: false,
                consumer: consumer);

            Console.WriteLine(" [x] Awaiting RPC requests");

            // Since it will push us messages asynchronously, we provide a callback.
            // That is what EventingBasicConsumer.Received event handler does.
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();

                // Coping CorrelationId from incoming message to outgoing
                var props = ea.BasicProperties;
                var replyProps = consumer.Model.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                var response = string.Empty;

                try
                {
                    // Incoming message
                    var message = Encoding.UTF8.GetString(body);
                    var n = int.Parse(message);

                    Console.WriteLine(" [.] fib({0})", message);
                    response = fib(n).ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                    response = "";
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);

                    // Publish messages to our default exchange. 
                    // We need to supply a routingKey when sending.
                    consumer.Model.BasicPublish(
                        exchange: "", 
                        routingKey: props.ReplyTo,
                        basicProperties: replyProps, 
                        body: responseBytes);

                    // It is possible to access the channel via Model property of consumer.
                    // Using this code we can be sure that even if you kill a worker using
                    // CTRL+C while it was processing a message, nothing will be lost.
                    // Soon after the worker dies all unacknowledged messages will be redelivered.
                    // Acknowledgement must be sent on the same channel that received the delivery.
                    // Attempts to acknowledge using a different channel will result in a channel-level protocol exception.
                    consumer.Model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static long fib(int n)
        {
            long f1 = 1;
            long f2 = 1;
            long f_result = 0;

            long my_counter;

            for (my_counter=2; my_counter < n; my_counter++)
            {
                f_result = f1 + f2;
                f1 = f2;
                f2 = f_result;
            }

            return f_result;
        }
    }
}
