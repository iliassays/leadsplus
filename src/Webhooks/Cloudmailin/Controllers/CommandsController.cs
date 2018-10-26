namespace Cloudmailin.Webhook.Controllers
{
    using Cloudmailin.IntegrationEvents;
    using Cloudmailin.Webhook.Command;
    using Cloudmailin.Webhook.IntegrationEvents;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;

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
                    IsBodyHtml = usePlainText.Equals("useplaintext", StringComparison.InvariantCultureIgnoreCase) ? true : false,
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

        [Route("sendbuydemoemail")]
        [HttpPost]
        public IActionResult SendBuyDemoEmail([FromBody] CreateDemoEnquiryEmailCommand @command)
        {
            var emailNeedsToBeSent = new EmailNeedsToBeSentIntegrationEvent
            {
                IsBodyHtml = @command.IsBodyhtml,
                FromEmail = @command.From,
                FromName = "Demo",
                To = new[] { @command.To },
                AggregateId = Guid.NewGuid().ToString(),
                DisableClickTracking = true,
                DisableOpenTracking = true,
                MergeFields = GetMergeField(@command),
                TemplateId = "770b116f-fe82-410d-b675-198f45361d5a"
            };

            _eventBus.Publish(emailNeedsToBeSent);

            return (IActionResult) Ok(emailNeedsToBeSent);
        }

        [Route("sendrentdemoemail")]
        [HttpPost]
        public IActionResult SendRentDemoEmail([FromBody] CreateDemoEnquiryEmailCommand @command)
        {
            var emailNeedsToBeSent = new EmailNeedsToBeSentIntegrationEvent
            {
                IsBodyHtml = @command.IsBodyhtml,
                FromEmail = @command.From,
                FromName = "Demo",
                To = new[] { @command.To },
                AggregateId = Guid.NewGuid().ToString(),
                DisableClickTracking = true,
                DisableOpenTracking = true,
                MergeFields = GetMergeField(@command),
                TemplateId = "1797b49e-4c40-43ff-9220-c95816dadd9e"
            };

            _eventBus.Publish(emailNeedsToBeSent);

            return (IActionResult) Ok(emailNeedsToBeSent);
        }

        private Dictionary<string, string> GetMergeField(CreateDemoEnquiryEmailCommand @command)
        {
            var mergedFields = new Dictionary<string, string>()
                {
                    { "[customerfirstname]", @command.CustomerFirstname },
                    { "[customerlastname]", @command.CustomerLastname },
                    { "[customeremail]", @command.CustomerEmail },
                    { "[customerphone]", @command.CustomerPhone },
                    { "[customeraddress]", @command.CustomerAddress },
                    { "[customermessage]", @command.CustomerMessage },
                };

            return mergedFields;
        }
    }
}
