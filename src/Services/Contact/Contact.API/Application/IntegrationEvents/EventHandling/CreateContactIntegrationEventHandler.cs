namespace Contact.IntegrationEvents
{
    using Contact.Commands;
    using Contact.IntegrationEvents;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class CreateContactIntegrationEventHandler
        : IIntegrationEventHandler<CreateContactIntegrationEvent>
    {
        private readonly IMediator mediator;
        private readonly ILoggerFactory logger;

        public CreateContactIntegrationEventHandler(IMediator mediator,
            ILoggerFactory logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(CreateContactIntegrationEvent @event)
        {
            logger.CreateLogger(nameof(@event)).LogTrace($"New contact create request received Owner:{@event.OwnerId}-{@event.Email}.");

            CreateContactCommand createContactCommand = new CreateContactCommand
            {
                Company = @event.Company,
                Country = @event.Country,
                City = @event.City,
                Phone = @event.Phone,
                Address = @event.Address,
                Email = @event.Email,
                Firstname = @event.Firstname,
                Lastname = @event.Lastname,
                Aboutme = @event.Aboutme,
                GroupId = @event.GroupId,
                OwnerId = @event.OwnerId,
                Ownername = @event.Ownername,
                Source = @event.Source,
                AggregateId = @event.AggregateId
            };

            await mediator.Send(createContactCommand);

            logger.CreateLogger(nameof(@event)).LogTrace($"New contact sent for processing Owner:{@event.OwnerId}-{@event.Email}.");
        }
    }
}
