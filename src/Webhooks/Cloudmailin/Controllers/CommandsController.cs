namespace Cloudmailin.Webhook.Controllers
{
    using Cloudmailin.IntegrationEvents;
    using Cloudmailin.Webhook.Command;
    using Cloudmailin.Webhook.IntegrationEvents;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    [Route("api/v1/[controller]")]
    //[Authorize]
    public class CommandsController : ControllerBase
    {
        private readonly IEventBus _eventBus;
        private readonly ILoggerFactory logger;

        public CommandsController(IEventBus eventBus, ILoggerFactory logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        [Route("hello")]
        [HttpGet]
        public OkObjectResult SayHello()
        {
            return Ok("Say Hello");
        }

        [Route("tracked")]
        [HttpPost]
        public async Task<IActionResult> Tracked(CreateInboundEmailCommand createInboundEmailCommand)
        {

            var @event = new AgentInboundEmailTrackedIntegrationEvent()
            {
                Body = createInboundEmailCommand.Text,
                PlainText = createInboundEmailCommand.Plain,
                OrganizationEmail = createInboundEmailCommand.From,
                AgentEmail = createInboundEmailCommand.To,
                Subject = createInboundEmailCommand.Subject,
                AggregateId = Guid.NewGuid().ToString()
            };

            //This  will trigger event in Agent Api to send a autorespondar
            _eventBus.Publish(@event);

            return (IActionResult)Ok(@event);
        }
    }
}
