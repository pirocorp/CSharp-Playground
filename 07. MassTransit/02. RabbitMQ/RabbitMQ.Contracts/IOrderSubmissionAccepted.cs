namespace RabbitMQ.Contracts
{
    using System;

    // Mass transit will declare new exchange for every message type based on message full name (with namespace)
    // When you publish message mass transit will publish this message to this exchange.
    public interface IOrderSubmissionAccepted
    {
        Guid OrderId { get; }

        DateTime TimeStamp { get; }

        string CustomerNumber { get; }
    }
}
