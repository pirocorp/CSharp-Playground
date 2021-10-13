namespace RabbitMQ.Service
{
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Hosting;

    public class MassTransitConsoleHostedService : IHostedService
    {
        private readonly IBusControl bus;

        public MassTransitConsoleHostedService(IBusControl bus)
        {
            this.bus = bus;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.bus.StartAsync(cancellationToken).ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return this.bus.StopAsync(cancellationToken);
        }
    }
}
