namespace Cloudmailin.Webhook.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Cloudmailin.Webhook.Command;
    using Cloudmailin.Webhook.IntegrationEvents;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using System.Net.Http.Formatting;

    [Route("api/v1/[controller]")]
    //[Authorize]
    public class CommandsController : ControllerBase
    {
        private readonly IEventBus _eventBus;

        public CommandsController(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        [Route("hello")]
        [HttpGet]
        public OkObjectResult SayHello()
        {
            return Ok("Say Hello");
        }

        [Route("parse")]
        [HttpPost]
        public async Task<IActionResult> Parse(FormDataCollection formCollection)
        {
            var @event = new AgentInboundEmailTrackedIntegrationEvent()
            {
                Body = formCollection["Text"],
                PlainText = formCollection["Plain"],
                CustomerEmail = formCollection["From"],
                AgentEmail = formCollection["To"],
                Subject = formCollection["Subject"],
            };

            //This  will trigger event in Agent Api to send a autorespondar
            _eventBus.Publish(@event);

            return (IActionResult) Ok();
        }
    }
}
