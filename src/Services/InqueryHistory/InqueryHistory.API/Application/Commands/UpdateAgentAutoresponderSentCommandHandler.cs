namespace InqueryHistory.Commands
{
    using InqueryHistory.Command;
    using InqueryHistory.Domain;
    using InqueryHistory.Domain.Query;
    using InqueryHistory.Services;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core;
    using LeadsPlus.Core.Extension;
    using LeadsPlus.Core.Query;
    using MediatR;
    using MongoDB.Driver;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateAgentAutoresponderSentCommandHandler
        : IRequestHandler<UpdateAgentAutoresponderSentCommand, bool>
    {
        private readonly IEventBus eventBus;
        private readonly IMediator mediator;
        private readonly IIdentityService identityService;
        private readonly IQueryExecutor queryExecutor;
        private readonly IRepository<InqueryHistory> inqueryHistoryRepository;

        public UpdateAgentAutoresponderSentCommandHandler(IMediator mediator, 
            IRepository<InqueryHistory> inqueryHistoryRepository, 
            IEventBus eventBus, 
            IIdentityService identityService,
            IQueryExecutor queryExecutor)
        {
            this.inqueryHistoryRepository = inqueryHistoryRepository ?? throw new ArgumentNullException(nameof(inqueryHistoryRepository));
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(UpdateAgentAutoresponderSentCommand @command, CancellationToken cancellationToken)
        {
            var inqueryHistoryToUpdate = await queryExecutor.Execute<GetInqueryHistoryQuery, InqueryHistory>(
                new GetInqueryHistoryQuery { InqueryHistoryId = @command.AggregateId });

            inqueryHistoryToUpdate.AgentAutoresponderSent = true;
            inqueryHistoryToUpdate.SetAutoresponderSendingStatus();

            var filter = Builders<InqueryHistory>.Filter.Eq("Id", @command.AggregateId);
            var update = Builders<InqueryHistory>.Update
                .Set("InqueryStatus", inqueryHistoryToUpdate.InqueryStatus)
                .Set("AgentAutoresponderSent", inqueryHistoryToUpdate.AgentAutoresponderSent)
                .CurrentDate("UpdatedDate");

            await inqueryHistoryRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            await mediator.DispatchDomainEventsAsync(inqueryHistoryToUpdate);

            return true;
        }
    }
}