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

    public class AgentSpreadsheetCreatedDomainEventHandler
                        : INotificationHandler<AgentSpreadsheetCreatedEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;
        private readonly IRepository<Agent> agentRepository;
        private readonly IMediator madiator;

        public AgentSpreadsheetCreatedDomainEventHandler(
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

        public async Task Handle(AgentSpreadsheetCreatedEvent agentSpreadsheetCreatedEvent, CancellationToken cancellationToken)
        {
            string typeFormTemplateJson = await TypeFormCreator.GetTemplateFormAsync();

            var spreadsheet = CreateSpreadsheetForTypeform(agentSpreadsheetCreatedEvent.Agent, typeFormTemplateJson);

            agentSpreadsheetCreatedEvent.Agent.AgentTypeForm.SpreadsheetUrl = spreadsheet.SpreadsheetUrl;
            agentSpreadsheetCreatedEvent.Agent.AgentTypeForm.SpreadsheetId = spreadsheet.SpreadsheetId;
            agentSpreadsheetCreatedEvent.Agent.AgentTypeForm.SpreadsheetName = GetSpreadsheetName(agentSpreadsheetCreatedEvent.Agent);

            var filter = Builders<Agent>.Filter.Eq("Id", agentSpreadsheetCreatedEvent.Agent.Id);
            var update = Builders<Agent>.Update
                .Set("AgentTypeForm", agentSpreadsheetCreatedEvent.Agent.AgentTypeForm)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
            //Do create typeform
            logger.CreateLogger(nameof(agentSpreadsheetCreatedEvent)).LogTrace($"Agent typefor created {agentSpreadsheetCreatedEvent.Agent.Id}.");
        }

        private Spreadsheet CreateSpreadsheetForTypeform(Agent agent, string typeFormTemplateJson)
        {
            dynamic typeForm = JObject.Parse(typeFormTemplateJson);

            //spreadSheetCreator.CreateSpreadSheetPermission(spreadsheet.SpreadsheetId, "bintusays@gmail.com");
            //return spreadsheet.SpreadsheetUrl;

            CreateSpreadsheetCommand createSpreadsheetCommand = new CreateSpreadsheetCommand
            {
                SpreadSheetName = GetSpreadsheetName(agent),
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

            var assigSpreadsheetPermissionCommandResult = madiator.Send(assigSpreadsheetPermissionCommand).Result;

            AssigSpreadsheetPermissionCommand assigSpreadsheetPermissionToOrganizationCommand = new AssigSpreadsheetPermissionCommand
            {
                Email = "shimulsays@gmail.com",
                SpreadsheetId = spreadsheet.SpreadsheetId,
                ApplicationName = "LeadsPlus"
            };

            var assigSpreadsheetPermissionToOrganizationCommandResult = madiator.Send(assigSpreadsheetPermissionToOrganizationCommand).Result;

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

        private string GetSpreadsheetName(Agent agent)
        {
            return $"{agent.Email}_{agent.Firstname}_{agent.Id}";
        }
    }
}
