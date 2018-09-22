﻿namespace InqueryHistory.IntegrationEvents
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

    public class NewInqueryRequestReceivedIntegrationEventHandler
        : IIntegrationEventHandler<NewInqueryRequestReceivedIntegrationEvent>
    {
        private readonly IEventBus eventBus;
        private readonly IMediator mediator;
        private readonly IIdentityService identityService;
        private readonly IQueryExecutor queryExecutor;
        private readonly IRepository<InqueryHistory> inqueryHistoryRepository;
        private readonly ILoggerFactory logger;

        public NewInqueryRequestReceivedIntegrationEventHandler(IMediator mediator,
            IRepository<InqueryHistory> agentRepository,
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

        public async Task Handle(NewInqueryRequestReceivedIntegrationEvent @event)
        {
            //logger.CreateLogger(nameof(@event)).LogTrace($"customer email tracked {@event.From}.");
            //logger.CreateLogger(nameof(@event)).LogTrace($"agent email {@event.To}.");

            //1. Let caller know that we have started the processing
            //2. send the request to zapier to parse the mail,
            //3. send the parsed result to agent for further action

            var newInqueryProcessStartedIntegrationEvent = new NewInqueryProcessStartedIntegrationEvent();
            eventBus.Publish(newInqueryProcessStartedIntegrationEvent);
            //string id, string customerEmail, string message, string subject, string agentEmail, AgentInfo agentInfo
            var inqueryHistory = new InqueryHistory(@event.AggregateId,
                @event.CustomerEmail,
                @event.Body,
                @event.Subject,
                @event.AgentEmail,
                new Domain.AgentInfo()
                 {
                     Address = @event.AgentInfo.Address,
                     City = @event.AgentInfo.City,
                     State = @event.AgentInfo.State,
                     Zip = @event.AgentInfo.Zip,
                     Email = @event.AgentInfo.Email,
                     Firstname = @event.AgentInfo.Firstname,
                     Lastname = @event.AgentInfo.Lastname,
                     Company = @event.AgentInfo.Company,
                     Country = @event.AgentInfo.Country,
                     Phone = @event.AgentInfo.Phone,
                     Id = @event.AgentInfo.Id,
                     IntegrationEmail = @event.AgentInfo.IntegrationEmail,
                     AgentTypeFormInfo = new Domain.AgentTypeFormInfo
                     {
                         SpreadsheetId = @event.AgentInfo.AgentTypeFormInfo?.SpreadsheetId,
                         SpreadsheetName = @event.AgentInfo.AgentTypeFormInfo?.SpreadsheetName,
                         SpreadsheetUrl = @event.AgentInfo.AgentTypeFormInfo?.SpreadsheetUrl,
                         TypeFormUrl = @event.AgentInfo.AgentTypeFormInfo?.TypeFormUrl
                     }
                });

            await inqueryHistoryRepository.AddAsync(inqueryHistory);
            await mediator.DispatchDomainEventsAsync(inqueryHistory);
        }
    }
}
