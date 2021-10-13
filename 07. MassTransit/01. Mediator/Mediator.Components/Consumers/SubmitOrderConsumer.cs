namespace Mediator.Components.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;
    using Microsoft.Extensions.Logging;

    public class SubmitOrderConsumer : IConsumer<ISubmitOrder>
    {
        private readonly ILogger<SubmitOrderConsumer> logger;

        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<ISubmitOrder> context)
        {
            this.logger.Log(LogLevel.Debug, $"SubmitOrderConsumer: {context.Message.CustomerNumber}");

            if (context.Message.CustomerNumber.Contains("TEST", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.RespondAsync<IOrderSubmissionRejected>(new
                {
                    TimeStamp = InVar.Timestamp,
                    context.Message.OrderId,
                    context.Message.CustomerNumber,
                    Reason = $"Test customer cannot submit orders: {context.Message.CustomerNumber}"
                });

                return;
            }

            await context.RespondAsync<IOrderSubmissionAccepted>(new
            {
                TimeStamp = InVar.Timestamp,
                context.Message.OrderId,
                context.Message.CustomerNumber
            });
        }
    }
}
