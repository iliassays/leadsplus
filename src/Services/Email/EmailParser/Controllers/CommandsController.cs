namespace Email.EmailParser.Controllers
{
    using Email.EmailParser.IntegrationEvents;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
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
        public IActionResult GetParsed()
        {

            var @event = new EmailParsedIntegrationEvent()
            {
                ExtractedField = FilterExtractedFields(Request.Form),
                AggregateId = ExtractAggregateId(),
            };

            //This  will trigger event in Agent Api to send a autorespondar
            _eventBus.Publish(@event);

            return (IActionResult)Ok(@event);
        }

        private string ExtractAggregateId()
        {
            string aggregateId = "";
            string subject = Request.Form["email__subject"];
            string prefix = ":[agg-";
            string suffix = "]";

            var splited = subject.Split(':');

            foreach (var portion in splited)
            {
                if (portion.StartsWith(prefix) && portion.EndsWith(suffix))
                {
                    //Remove suffix
                    aggregateId = portion.Remove(0, prefix.Length - 1);

                    //Remove preffix
                    aggregateId = portion.Remove((portion.Length - 1) - suffix.Length, (portion.Length - 1));
                }
            }

            return aggregateId;
        }

        private Dictionary<string, string> FilterExtractedFields(IFormCollection form)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            
            foreach (var item in form)
            {
                switch (item.Key)
                {
                    case "hook__target":
                    case "email__cc":
                    case "mailbox__template":
                    case "mailbox__address":
                    case "mailbox__key":
                    case "id":
                    case "parse__payload":
                    case "hook__filters__mailbox_id":
                    case "email__sender__email":
                    case "email__reply_to":
                    case "email__sender__name":
                    case "mailbox__id":
                    case "hook__id":
                    case "date":
                    case "email__body_plain":
                    case "mailbox__date":
                    case "hook__event":
                    case "email__attachment":
                    case "email__recipient":
                    case "email__subject":
                        continue;
                }

                fields.Add(item.Key.Remove(0, ("parse__output__").Length - 1), item.Value);
            }

            return fields;
        }
    }
}
