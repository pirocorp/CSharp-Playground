namespace Mediator.Components.Consumers
{
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;

    public class SubmitOrderConsumer : IConsumer<ISubmitOrder>
    {
        public async Task Consume(ConsumeContext<ISubmitOrder> context)
        {
            await context.RespondAsync<IOrderSubmissionAccepted>(new
            {
                TimeStamp = InVar.Timestamp,
                context.Message.OrderId,
                context.Message.CustomerNumber
            });
        }
    }
}
