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
        public async Task<IActionResult> Tracked([FromBody] CreateInboundEmailCommand createInboundEmailCommand)
        {

            var requestForm = Request.Form;
            var form = JsonConvert.SerializeObject(Request.Form);
            var logr = logger.CreateLogger("Tracked");

            logr.Log(LogLevel.Critical, $"Full email  : { form }");

            var from = requestForm["envelope[from]"];
            logr.Log(LogLevel.Critical, $"from: { from }");

            var to = requestForm["envelope[to]"];
            logr.Log(LogLevel.Critical, $"to: { to }");

            var body = requestForm["html"];
            logr.Log(LogLevel.Critical, $"body: { body }");

            var subject = requestForm["headers[Subject]"];
            subject = $"{subject} :[{to}]";
            logr.Log(LogLevel.Critical, $"subject: { subject }");

            var zapierMailbox = "rightmove0co0uk0customerinquery@robot.zapier.com";

            var emailNeedsToBeSent = new EmailNeedsToBeSentIntegrationEvent
            {
                Body = body,
                IsBodyHtml = true,
                Subject = subject,
                FromEmail = from,
                To = new[] { zapierMailbox },
                AggregateId = Guid.NewGuid().ToString(),
            };

            _eventBus.Publish(emailNeedsToBeSent);

            logr.Log(LogLevel.Critical, "Email need to be sent published !");


            return (IActionResult)Ok();
        }

        [Route("parsed")]
        [HttpPost]
        public async Task<IActionResult> Parsed(string trackedData)
        {
            var logr = logger.CreateLogger("Parsed");
            logr.Log(LogLevel.Critical, "Parsed....");

            try
            {
                logr.Log(LogLevel.Critical, $"body : {trackedData}");
                logr.Log(LogLevel.Critical, $"from : {JsonConvert.SerializeObject(Request.Form)}");
                var dynamic = JsonConvert.DeserializeObject<dynamic>(trackedData);
                logr.Log(LogLevel.Critical, $"dynamic : {JsonConvert.SerializeObject(dynamic)}");

            }
            catch (Exception ex)
            {
                logr.Log(LogLevel.Critical, $"exception : {ex.ToString()}");
            }

            return (IActionResult)Ok();
        }

    }
}
