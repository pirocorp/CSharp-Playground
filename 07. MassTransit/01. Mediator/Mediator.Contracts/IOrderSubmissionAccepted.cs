namespace Mediator.Contracts
{
    using System;

    public interface IOrderSubmissionAccepted
    {
        Guid OrderId { get; }

        DateTime TimeStamp { get; }

        string CustomerNumber { get; }
    }
}
