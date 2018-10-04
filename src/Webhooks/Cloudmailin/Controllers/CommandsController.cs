namespace Cloudmailin.Webhook.Controllers
{
    using Cloudmailin.IntegrationEvents;
    using Cloudmailin.Webhook.IntegrationEvents;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;

    [Route("api/v1/[controller]")]
    //[Authorize]
    public class CommandsController : ControllerBase
    {
        private readonly IEventBus _eventBus;
        private readonly ILoggerFactory _logger;

        public CommandsController(IEventBus eventBus, ILoggerFactory logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        public IActionResult Tracked()
        {

            var aggregateId = Guid.NewGuid().ToString();

            var @event = new AgentInboundEmailTrackedIntegrationEvent()
            {
                Body = Request.Form["html"],
                PlainText = Request.Form["plain"],
                OrganizationEmail = Request.Form["envelope[from]"],
                AgentEmail = Request.Form["envelope[to]"],
                Subject = $"{Request.Form["headers[Subject]"]}",
                AggregateId = aggregateId
            };

            if (@event.Subject.Contains(":::::"))
            {
                var usePlainText = @event.Subject.Split(":::::")[2];
                var emailNeedsToBeSent = new EmailNeedsToBeSentIntegrationEvent
                {
                    Body = usePlainText.Equals("useplaintext", StringComparison.InvariantCultureIgnoreCase) ? @event.PlainText : @event.Body,
                    IsBodyHtml = true,
                    Subject = @event.Subject,
                    FromEmail = "shimulsays@gmail.com",
                    FromName = "Ilias Hossain",
                    To = new[] { @event.Subject.Split(":::::")[1] },
                    AggregateId = Guid.NewGuid().ToString(),
                    DisableClickTracking = true,
                    DisableOpenTracking = true
                };

                _eventBus.Publish(emailNeedsToBeSent);

            }
            else
            {
                //This  will trigger event in Agent Api to send a autorespondar
                _eventBus.Publish(@event);
            }

            return (IActionResult)Ok(@event);
        }

    }
}
