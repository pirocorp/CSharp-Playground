namespace RabbitMQ.Contracts
{
    using System;

    public interface ISubmitOrder
    {
        Guid OrderId { get; }

        DateTime TimeStamp { get; }

        string CustomerNumber { get; }
    }
}
