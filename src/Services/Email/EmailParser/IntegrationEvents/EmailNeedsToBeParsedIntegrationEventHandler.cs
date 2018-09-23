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
                Subject = $"{@event.Subject} :[agg-{@event.AggregateId}]",
                FromEmail = "adfenixemailparser@adfeixleads.com",
                FromName = "adfenixemailparser",
                To = new[] { @event.ToEmail },
                AggregateId = @event.AggregateId
            };

            eventBus.Publish(emailNeedsToBeSent);
        }
    }
}
