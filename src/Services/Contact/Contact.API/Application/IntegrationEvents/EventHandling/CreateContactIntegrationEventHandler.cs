namespace Contact.IntegrationEvents
{
    using Contact.Commands;
    using Contact.IntegrationEvents;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using MediatR;
    using System;
    using System.Threading.Tasks;

    public class CreateContactIntegrationEventHandler
        : IIntegrationEventHandler<CreateContactIntegrationEvent>
    {
        private readonly IMediator mediator;

        public CreateContactIntegrationEventHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(CreateContactIntegrationEvent @event)
        {
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
                Source = @event.Source,
                AggregateId = @event.AggregateId
            };

            await mediator.Send(createContactCommand);
        }
    }
}
