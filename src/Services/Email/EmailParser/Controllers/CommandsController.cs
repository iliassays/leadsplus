namespace Email.EmailParser.Controllers
{
    using Email.EmailParser.IntegrationEvents;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
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

        [Route("parsed")]
        [HttpPost]
        public async Task<IActionResult> GetParsed()
        {
            var @event = new EmailParsedIntegrationEvent()
            {
                ExtractedField = null,
                AggregateId = "", //parse it from subject last part after split by _ or header
            };

            //This  will trigger event in Agent Api to send a autorespondar
            _eventBus.Publish(@event);

            return (IActionResult)Ok(@event);
        }
    }
}
