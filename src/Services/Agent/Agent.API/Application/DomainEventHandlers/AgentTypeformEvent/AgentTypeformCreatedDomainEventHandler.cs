namespace Contact.DomainEventHandlers.ContactCreatedEvent
{
    using Agent.Command;
    using Agent.Domain;
    using Agent.Domain.Events;
    using Agent.IntegrationEvents;
    using Agent.Services;
    using Agent.TypeFormIntegration;
    using Google.Apis.Sheets.v4.Data;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core;
    using LeadsPlus.GoogleApis.Command;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AgentTypeformCreatedDomainEventHandler
                        : INotificationHandler<AgentTypeformCreatedEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;
        private readonly IRepository<Agent> agentRepository;
        private readonly IMediator madiator;

        public AgentTypeformCreatedDomainEventHandler(
            ILoggerFactory logger,
            IIdentityService identityService,
            IEventBus eventBus,
            IRepository<Agent> agentRepository,
            IMediator madiator)
        {
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
            this.madiator = madiator ?? throw new ArgumentNullException(nameof(madiator));
        }

        public async Task Handle(AgentTypeformCreatedEvent agentTypeformCreatedEvent, CancellationToken cancellationToken)
        {
            string typeFormTemplateJson = await TypeFormCreator.GetTemplateFormAsync();
            var typeFormUrl = await CreateTypeformUrl(agentTypeformCreatedEvent.Agent, typeFormTemplateJson);

            if (agentTypeformCreatedEvent.Agent.AgentTypeForm == null)
            {
                agentTypeformCreatedEvent.Agent.AgentTypeForm = new AgentTypeForm();
            }

            agentTypeformCreatedEvent.Agent.AgentTypeForm.TypeFormUrl = typeFormUrl;

            var filter = Builders<Agent>.Filter.Eq("Id", agentTypeformCreatedEvent.Agent.Id);
            var update = Builders<Agent>.Update
                .Set("AgentTypeForm", agentTypeformCreatedEvent.Agent.AgentTypeForm)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
            //Do create typeform
            logger.CreateLogger(nameof(agentTypeformCreatedEvent)).LogTrace($"Agent typefor created {agentTypeformCreatedEvent.Agent.Id}.");
        }

        private async Task<string> CreateTypeformUrl(Agent agent, string typeFormTemplateJson)
        {
            //Regex reg = new Regex(@"\""(id\"":[ ]?\""[\d\s\w]*\"",)");
            //Regex reg = new Regex(@"id\"":[ ]?\""([\d\s\w]*)\"",");
            //typeformJson = reg.Replace(typeformJson, delegate (Match m)
            //{
            //    return string.Empty;
            //});
            //typeform.title = $"{agent.Firstname}_{agent.Lastname}_{agent.Email}_{agent.Id}";
            //typeform.id = "";

            var cretedTypeFormJson = await TypeFormCreator.CreateTypeFormAsync(typeFormTemplateJson);
            dynamic cretedTypeForm = JObject.Parse(cretedTypeFormJson);

            return cretedTypeForm._links.display;
        }
    }
}
