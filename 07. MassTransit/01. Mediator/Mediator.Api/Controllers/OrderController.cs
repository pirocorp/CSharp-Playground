namespace Mediator.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;
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
        public async Task<IActionResult> Post(Guid id, string customerNumber)
        {
            // Acts like RPC
            var response = await this.submitOrderRequestClient.GetResponse<IOrderSubmissionAccepted>(new
            {
                OrderId = id,
                TimeStamp = InVar.Timestamp,
                CustomerNumber = customerNumber
            });

            return this.Ok(response.Message);
        }
    }
}
