namespace Mediator.Contracts
{
    using System;

    public interface IOrderSubmissionRejected
    {
        Guid OrderId { get; }

        DateTime TimeStamp { get; }

        string CustomerNumber { get; }

        string Reason { get; }
    }
}
