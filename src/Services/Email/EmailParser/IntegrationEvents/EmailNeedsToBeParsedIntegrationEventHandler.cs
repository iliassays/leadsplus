namespace Email.EmailParser.IntegrationEvents
{
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class EmailNeedsToBeParsedIntegrationEventHandler
        : IIntegrationEventHandler<EmailNeedsToBeParsedIntegrationEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IEventBus eventBus;

        public EmailNeedsToBeParsedIntegrationEventHandler(
            ILoggerFactory logger,
            IEventBus eventBus)
        {
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));            
        }

        public async Task Handle(EmailNeedsToBeParsedIntegrationEvent @event)
        {
            //For now, we just send a plain mail to a mailbox and get it parsed. 
            //Lot of architectural things will come later

            var emailNeedsToBeSent = new EmailNeedsToBeSentIntegrationEvent
            {
                Body = @event.Body,
                IsBodyHtml = true,
                Subject = $"{@event.Subject}_{@event.AggregateId}", //later we will push it to header
                FromEmail = "adfenixemailparser@adfeixleads.com",
                FromName = "adfenixemailparser",
                To = new[] { "" }, //decide mailbox to be sent for parsing
                AggregateId = @event.AggregateId
            };

            eventBus.Publish(emailNeedsToBeSent);
        }
    }
}
