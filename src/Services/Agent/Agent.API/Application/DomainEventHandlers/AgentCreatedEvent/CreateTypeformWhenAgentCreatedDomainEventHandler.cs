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

    public class CreateTypeformWhenAgentCreatedDomainEventHandler
                        : INotificationHandler<AgentCreatedDomainEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;
        private readonly IRepository<Agent> agentRepository;
        private readonly IMediator madiator;

        public CreateTypeformWhenAgentCreatedDomainEventHandler(
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

        public async Task Handle(AgentCreatedDomainEvent agentCreatedDomainEvent, CancellationToken cancellationToken)
        {
            string typeFormTemplateJson = await TypeFormCreator.GetTemplateFormAsync();
            var typeFormUrl = await CreateTypeformUrl(agentCreatedDomainEvent.Agent, typeFormTemplateJson);

            var spreadsheet = CreateSpreadsheetForTypeform(agentCreatedDomainEvent.Agent, typeFormTemplateJson);

            if (agentCreatedDomainEvent.Agent.AgentTypeForm == null)
            {
                agentCreatedDomainEvent.Agent.AgentTypeForm = new AgentTypeForm();
            }

            agentCreatedDomainEvent.Agent.AgentTypeForm.TypeFormUrl = typeFormUrl;
            agentCreatedDomainEvent.Agent.AgentTypeForm.SpreadsheetUrl = spreadsheet.SpreadsheetUrl;
            agentCreatedDomainEvent.Agent.AgentTypeForm.SpreadsheetId = spreadsheet.SpreadsheetId;

            var filter = Builders<Agent>.Filter.Eq("Id", agentCreatedDomainEvent.Agent.Id);
            var update = Builders<Agent>.Update
                .Set("AgentTypeForm", agentCreatedDomainEvent.Agent.AgentTypeForm)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
            //Do create typeform
            logger.CreateLogger(nameof(agentCreatedDomainEvent)).LogTrace($"Agent typefor created {agentCreatedDomainEvent.Agent.Id}.");
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



        private Spreadsheet CreateSpreadsheetForTypeform(Agent agent, string typeFormTemplateJson)
        {
            dynamic typeForm = JObject.Parse(typeFormTemplateJson);

            //spreadSheetCreator.CreateSpreadSheetPermission(spreadsheet.SpreadsheetId, "bintusays@gmail.com");
            //return spreadsheet.SpreadsheetUrl;

            CreateSpreadsheetCommand createSpreadsheetCommand = new CreateSpreadsheetCommand
            {
                SpreadSheetName = $"{agent.Email}_{agent.Firstname}_{agent.Id}",
                WorkSheetName = "Inquiries",
                ApplicationName = "LeadsPlus"
            };

            foreach (dynamic item in typeForm.fields)
            {
                createSpreadsheetCommand.HeaderValues.Add(item.title);
                createSpreadsheetCommand.InitialValues.Add("-");
            }

            var spreadsheet = madiator.Send(createSpreadsheetCommand).Result;

            AssigSpreadsheetPermissionCommand assigSpreadsheetPermissionCommand = new AssigSpreadsheetPermissionCommand
            {
                Email = agent.Email,
                SpreadsheetId = spreadsheet.SpreadsheetId,
                ApplicationName = "LeadsPlus"
            };

            var result = madiator.Send(assigSpreadsheetPermissionCommand).Result;

            //var createContactIntegrationEvent = new CreateContactIntegrationEvent()
            //{
            //    AggregateId = agent.Id,
            //    Source = "AdfenixLeads",
            //    Email = agent.Email,
            //    OwnerId = agent.Id,
            //    Ownername = $"{agent.Firstname} {agent.Lastname}"
            //};

            //eventBus.Publish(createContactIntegrationEvent);

            return spreadsheet;
        }
    }
}
