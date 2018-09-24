namespace Contact.Commands
{
    using Contact.Domain;
    using Contact.Projection.Query;
    using LeadsPlus.Core;
    using LeadsPlus.Core.Query;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    // Regular CommandHandler
    public class ContactCommandHandler
        : IRequestHandler<CreateContactCommand, bool>, 
        IRequestHandler<UpdateContactCommand, bool>,
        IRequestHandler<RemoveContactCommand, bool>
    {
        private readonly IQueryExecutor queryExecutor;
        private readonly IIdentityService identityService;
        private readonly IMediator mediator;
        private readonly IRepository<Contact> contactRepository;
        private readonly ILoggerFactory logger;

        // Using DI to inject infrastructure persistence Repositories
        public ContactCommandHandler(IMediator mediator,
            ILoggerFactory logger,
            IQueryExecutor queryExecutor, 
            IIdentityService identityService,
            IRepository<Contact> contactRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        }

        public async Task<bool> Handle(CreateContactCommand command, CancellationToken cancellationToken)
        {
            //check if cntact for this agent already exist.

            logger.CreateLogger(nameof(command)).LogTrace($"Creating contact {command.AggregateId}-{command.Email}.");

            var email = command.Email?.Split(" ")[0];

            var contact = await queryExecutor.Execute<GetContactByEmailAndOwnerQuery, Contact>(
                new GetContactByEmailAndOwnerQuery { OwnerId = command.OwnerId, Email = email });

            if(contact == null)
            {
                contact = new Contact(command.AggregateId, command.OwnerId, command.Ownername, command.Source, command.Firstname, command.Lastname, email);

                await contactRepository.AddAsync(contact);

                logger.CreateLogger(nameof(command)).LogTrace($"New Contact created {command.AggregateId}-{command.Email}.");
            }

            logger.CreateLogger(nameof(command)).LogTrace($"Contact exists {command.AggregateId}-{command.Email}.");

            return true;
        }

        public async Task<bool> Handle(UpdateContactCommand message, CancellationToken cancellationToken)
        {
            //var contact = new Contact(message.OwnerId, message.Firstname, message.Lastname, message.Email);

            //await _contactRepository.AddAsync(contact);

            return true;
        }

        public async Task<bool> Handle(RemoveContactCommand message, CancellationToken cancellationToken)
        {
            //await _contactRepository.AddAsync(contact);

            return true;
        }
    }
}