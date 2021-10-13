namespace RabbitMQ.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IRequestClient<ISubmitOrder> submitOrderRequestClient;

        public OrderController(IRequestClient<ISubmitOrder> submitOrderRequestClient)
        {
            this.submitOrderRequestClient = submitOrderRequestClient;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IOrderSubmissionAccepted), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(IOrderSubmissionRejected), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(Guid id, string customerNumber)
        {
            // Acts like RPC
            var (accepted, rejected) = await this.submitOrderRequestClient
                .GetResponse<IOrderSubmissionAccepted, IOrderSubmissionRejected>(new
                {
                    OrderId = id,
                    TimeStamp = InVar.Timestamp,
                    CustomerNumber = customerNumber
                });

            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;

                return this.Accepted(response.Message);
            }
            else
            {
                var response = await rejected;

                return this.BadRequest(response.Message);
            }
        }
    }
}
