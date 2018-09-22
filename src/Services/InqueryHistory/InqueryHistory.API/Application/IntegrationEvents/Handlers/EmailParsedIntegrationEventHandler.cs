namespace InvitationHistory.IntegrationEvents
{
    using InqueryHistory.Domain;
    using InqueryHistory.Domain.Query;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core;
    using LeadsPlus.Core.Extension;
    using System;
    using System.Threading.Tasks;
    using LeadsPlus.Core.Query;
    using System.Collections.Generic;
    using System.Collections;
    using Microsoft.Extensions.Logging;
    using InqueryHistory.Services;
    using MediatR;
    using MongoDB.Driver;

    public class EmailParsedIntegrationEventHandler
        : IIntegrationEventHandler<EmailParsedIntegrationEvent>
    {
        private readonly IEventBus eventBus;
        private readonly IMediator mediator;
        private readonly IIdentityService identityService;
        private readonly IQueryExecutor queryExecutor;
        private readonly IRepository<InqueryHistory> inqueryHistoryRepository;
        private readonly ILoggerFactory logger;

        public EmailParsedIntegrationEventHandler(IMediator mediator,
            IRepository<InqueryHistory> inqueryHistoryRepository,
            IEventBus eventBus,
            ILoggerFactory logger,
            IIdentityService identityService,
            IQueryExecutor queryExecutor)
        {
            this.inqueryHistoryRepository = inqueryHistoryRepository ?? throw new ArgumentNullException(nameof(inqueryHistoryRepository));
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(EmailParsedIntegrationEvent @event)
        {
            logger.CreateLogger(nameof(@event)).LogTrace($"Email parsed  {@event.AggregateId}.");

            //get the inquery history and update parsed token
            var inqueryHistoryToUpdate = await queryExecutor.Execute<GetInqueryHistoryQuery, InqueryHistory>(
                new GetInqueryHistoryQuery { InqueryHistoryId = @event.AggregateId });

            inqueryHistoryToUpdate.SetParsedStatus();
            inqueryHistoryToUpdate.ExtractedFields = @event.ExtractedFields;

            //confused about this saving. could be better way of handling this
            var filter = Builders<InqueryHistory>.Filter.Eq("Id", @event.AggregateId);
            var update = Builders<InqueryHistory>.Update
                .Set("InqueryStatus", inqueryHistoryToUpdate.InqueryStatus)
                .Set("ExtractedFields", inqueryHistoryToUpdate.ExtractedFields)
                .CurrentDate("UpdatedDate");

            await inqueryHistoryRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            await mediator.DispatchDomainEventsAsync(inqueryHistoryToUpdate);
        }
    }
}
