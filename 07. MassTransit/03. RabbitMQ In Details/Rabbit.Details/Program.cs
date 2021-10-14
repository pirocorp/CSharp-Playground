namespace Rabbit.Details
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Sample.Components;
    using Sample.Contracts;

    namespace Sample.Contracts
    {
        // For each message MassTransit will generate different exchange
        // Based on Full Name of the message (including namespace)
        // In this case Exchange: Rabbit.Details.Sample.Contracts:IUpdateAccount
        // Fanout Exchange - Durable 
        public interface IUpdateAccount
        {
            public string AccountNumber { get; set; }
        }
    }

    namespace Sample.Components
    {
        using Contracts;

        public class AccountConsumer : IConsumer<IUpdateAccount>
        {
            public Task Consume(ConsumeContext<IUpdateAccount> context)
            {
                Console.WriteLine("Command received: {0}", context.Message.AccountNumber);
                return Task.CompletedTask;
            }
        }

        public class AnotherAccountConsumer : IConsumer<IUpdateAccount>
        {
            public Task Consume(ConsumeContext<IUpdateAccount> context)
            {
                Console.WriteLine("Another Command received: {0}", context.Message.AccountNumber);
                return Task.CompletedTask;
            }
        }
    }

    public static class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("RabbitMQ Details");

            var busControl = Bus.Factory.CreateUsingRabbitMq(bus =>
            {
                bus.Host("localhost", virtualHost =>
                {
                    virtualHost.Username("guest");
                    virtualHost.Password("guest");
                });

                // For each queue will be created exchange with the same name
                // Will generate "account-service" Exchange and "account-service" Queue
                // And will create binding "account-service" (Exchange) => "account-service" (Queue) (queue binding)
                bus.ReceiveEndpoint("account-service", endpoint =>
                {
                    // Avoid memory overloading
                    endpoint.Lazy = true;

                    // How many messages to be send to que before load balancing kick in.
                    // Approximation value 2 * task that can be performed per second
                    endpoint.PrefetchCount = 20;

                    // Configure consumer for the endpoint and will bound "account-service" exchange to the 
                    // AccountConsumer's message exchange. 
                    // In this case Rabbit.Details.Sample.Contracts:IUpdateAccount => account-service (exchange binding)
                    endpoint.Consumer<AccountConsumer>();
                });

                // Will generate "another-account-service" Exchange and "another-account-service" Queue (queue binding)
                bus.ReceiveEndpoint("another-account-service", endpoint =>
                {
                    // Disables automatic binding (exchange binding)
                    endpoint.ConfigureConsumeTopology = false;
                    endpoint.PrefetchCount = 10;

                    // Explicit binding for
                    // Rabbit.Details.Sample.Contracts:IUpdateAccount => another-account-service (exchange binding)
                    endpoint.Bind<IUpdateAccount>();

                    endpoint.Consumer<AnotherAccountConsumer>();
                });
            });

            using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await busControl.StartAsync(cancellation.Token);

            try
            {
                Console.WriteLine("Bus was started.");

                //var endpoint = await busControl.GetSendEndpoint(new Uri("exchange:account-service"));
                //await endpoint.Send<IUpdateAccount>(new
                //{
                //    AccountNumber = "123456",
                //});

                // Publish uses message type to determine destination. So publish send message to
                // Message exchange. And anything bound to that exchange will get a copy of that message.
                await busControl.Publish<IUpdateAccount>(new
                {
                    AccountNumber = "5555",
                });
            }
            finally
            {
                await busControl.StopAsync(CancellationToken.None);
            }
        }
    }
}
