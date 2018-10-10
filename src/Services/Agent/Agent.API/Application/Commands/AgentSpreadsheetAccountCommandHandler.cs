namespace Agent.Commands
{
    using Agent.Command;
    using Agent.Domain;
    using Agent.Domain.Query;
    using Agent.Services;
    using Agent.TypeFormIntegration;
    using Google.Apis.Sheets.v4.Data;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core;
    using LeadsPlus.Core.Extension;
    using LeadsPlus.Core.Query;
    using LeadsPlus.GoogleApis.Command;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using MongoDB.Driver;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    public class AgentSpreadsheetAccountCommandHandler
        : IRequestHandler<CreateAgentSpreadsheetAccountForBuyInquiryCommand, bool>,
        IRequestHandler<CreateAgentSpreadsheetAccountForRentInquiryCommand, bool>,
        IRequestHandler<UpdateAgentSpreadsheetForBuyInquiryCommand, bool>,
        IRequestHandler<UpdateAgentMortgageSpreadsheetForBuyInquiryCommand, bool>,
        IRequestHandler<UpdateAgentSpreadsheetForRentInquiryCommand, bool>,
        IRequestHandler<UpdateAgentLandlordSpreadsheetForRentInquiryCommand, bool>
    {
        private readonly IEventBus eventBus;
        private readonly IMediator mediator;
        private readonly IIdentityService identityService;
        private readonly IQueryExecutor queryExecutor;
        private readonly IRepository<Agent> agentRepository;
        private readonly ITypeForm typeForm;
        private readonly IConfiguration configuration;

        private List<string> customerHeaders = new List<string>()
        {
            { "Id" }, { "Date"}, { "Customer Name"}, { "Customer Email"}, { "Customer Phone"}, { "Customer Address"}, { "About Customer"},
            { "Enquiry Source"}, { "Enquiry Kind"}, { "Customer Message"}, { "Property Address"},
            { "Property Reference"}, { "Property Url"},
        };

        private List<string> buyInquiryHeaders = new List<string>()
        {
            { "Customer Current Position"}, { "Expected Date Of Moving"}, { "Perpose Of Search"}, { "Preffered Location"},
            { "Property Of Interest"}, { "Number Of Bedroom Required"}, { "Budget"}, { "Preffered Time of Contacting"},
            { "Other Important Feature"}, { "Want Best Mortgage Rate?"},
        };

        private List<string> rentInquiryHeaders = new List<string>()
        {
            { "Expected Date Of Moving"}, { "Customer Occupation"}, { "Currently Rent From"}, { "Need Internet Ready?"}, { "Have Property To Sell?"},
            { "Have Property To Let?"}, { "Preffered Time of Contacting"}
        };

        private List<string> motgageInquiryHeaders = new List<string>()
        {
            { "Id"}, { "Date"}, { "Enquiry Kind"}, { "Enquiry Source"}, { "Property Address"},
            { "Property Reference"}, { "Customer Name"},{ "Customer Email"}, { "Customer Phone"}, { "Customer Address"}
        };

        List<string> landLoardInquiryHeaders = new List<string>()
        {
            { "Id"}, { "Date"}, { "Landlord's name"}, { "Landlord's phone"}, { "Landlord's email"}, { "Landlord Detail Provided By"},
            { "Enquiry Kind"}, { "Enquiry Source"}, { "Property Address"},
            { "Property Reference"}, { "Customer Name"},{ "Customer Email"}, { "Customer Phone"}, { "Customer Address"}
        };

        public AgentSpreadsheetAccountCommandHandler(IMediator mediator, 
            IRepository<Agent> agentRepository, 
            IEventBus eventBus, 
            IIdentityService identityService,
            IQueryExecutor queryExecutor,
            ITypeForm typeForm,
            IConfiguration configuration)
        {
            this.agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.typeForm = typeForm ?? throw new ArgumentNullException(nameof(typeForm));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(CreateAgentSpreadsheetAccountForBuyInquiryCommand @command, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = @command.AggregateId });

            var spreadsheet = CreateSpreadsheetForTrackingInquiry(agent, customerHeaders.Concat(buyInquiryHeaders).ToList(), InquiryType.BuyInquiry);

            agent.BuyInquiry.SpreadsheetUrl = spreadsheet.SpreadsheetUrl;
            agent.BuyInquiry.SpreadsheetId = spreadsheet.SpreadsheetId;
            agent.BuyInquiry.SpreadsheetName = agent.GetSpreadsheetName(Enum.GetName(typeof(InquiryType), InquiryType.BuyInquiry));

            var mortgageSpreadsheet = CreateSpreadsheetForTrackingInquiry(agent, motgageInquiryHeaders, InquiryType.MortgageInquiry);

            agent.BuyInquiry.MortgageSpreadsheetUrl = mortgageSpreadsheet.SpreadsheetUrl;
            agent.BuyInquiry.MortgageSpreadsheetId = mortgageSpreadsheet.SpreadsheetId;
            agent.BuyInquiry.MortgageSpreadsheetName = agent.GetSpreadsheetName(Enum.GetName(typeof(InquiryType), InquiryType.MortgageInquiry));
            
            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);
            var update = Builders<Agent>.Update
                .Set("BuyInquiry", agent.BuyInquiry)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            agent.CreateSpreadsheet(InquiryType.BuyInquiry);
            await mediator.DispatchDomainEventsAsync(agent);

            return true;
        }

        public async Task<bool> Handle(CreateAgentSpreadsheetAccountForRentInquiryCommand @command, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = @command.AggregateId });

            var spreadsheet = CreateSpreadsheetForTrackingInquiry(agent, customerHeaders.Concat(rentInquiryHeaders).ToList(), InquiryType.RentInquiry);

            agent.RentInquiry.SpreadsheetUrl = spreadsheet.SpreadsheetUrl;
            agent.RentInquiry.SpreadsheetId = spreadsheet.SpreadsheetId;
            agent.RentInquiry.SpreadsheetName = agent.GetSpreadsheetName(Enum.GetName(typeof(InquiryType), InquiryType.RentInquiry));
            
            var mortgageSpreadsheet = CreateSpreadsheetForTrackingInquiry(agent, landLoardInquiryHeaders, InquiryType.LandlordInquiry);

            agent.RentInquiry.LandlordSpreadsheetUrl = mortgageSpreadsheet.SpreadsheetUrl;
            agent.RentInquiry.LandlordSpreadsheetId = mortgageSpreadsheet.SpreadsheetId;
            agent.RentInquiry.LandlordSpreadsheetName = agent.GetSpreadsheetName(Enum.GetName(typeof(InquiryType), InquiryType.LandlordInquiry));

            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);
            var update = Builders<Agent>.Update
                .Set("RentInquiry", agent.RentInquiry)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            agent.CreateSpreadsheet(InquiryType.RentInquiry);
            await mediator.DispatchDomainEventsAsync(agent);

            return true;
        }

        public async Task<bool> Handle(UpdateAgentSpreadsheetForBuyInquiryCommand @command, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = @command.AggregateId });
            agent.BuyInquiry.UpdateSpreadsheet(@command.SpreadsheetId, @command.SpreadsheetName, @command.SpreadsheetUrl, @command.SpreadsheetShareableUrl);

            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);
            var update = Builders<Agent>.Update
                .Set("BuyInquiry", agent.BuyInquiry)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            return true;
        }

        public async Task<bool> Handle(UpdateAgentMortgageSpreadsheetForBuyInquiryCommand @command, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = @command.AggregateId });
            agent.BuyInquiry.UpdateMortgageSpreadsheet(@command.SpreadsheetId, @command.SpreadsheetName, @command.SpreadsheetUrl, @command.SpreadsheetShareableUrl);

            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);
            var update = Builders<Agent>.Update
                .Set("BuyInquiry", agent.BuyInquiry)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            return true;
        }

        public async Task<bool> Handle(UpdateAgentSpreadsheetForRentInquiryCommand @command, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = @command.AggregateId });
            agent.RentInquiry.UpdateSpreadsheet(@command.SpreadsheetId, @command.SpreadsheetName, @command.SpreadsheetUrl, @command.SpreadsheetShareableUrl);

            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);
            var update = Builders<Agent>.Update
                .Set("RentInquiry", agent.RentInquiry)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            return true;
        }

        public async Task<bool> Handle(UpdateAgentLandlordSpreadsheetForRentInquiryCommand @command, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = @command.AggregateId });
            agent.RentInquiry.UpdateLandlordSpreadsheet(@command.SpreadsheetId, @command.SpreadsheetName, @command.SpreadsheetUrl, @command.SpreadsheetShareableUrl);

            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);
            var update = Builders<Agent>.Update
                .Set("RentInquiry", agent.RentInquiry)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            return true;
        }

        private async Task<string> CreateTypeFormUrl(Agent agent, InquiryType typeformType, string typeFormTemplateJson)
        {
            Regex reg = new Regex(@"(\""title\"":[ ]?\"")([\d\s\w]*)");

            typeFormTemplateJson = reg.Replace(typeFormTemplateJson, delegate (Match m)
            {
                return m.Groups[1].Value + agent.GetTypeformName(Enum.GetName(typeof(InquiryType), typeformType));
            }, 1);

            var cretedTypeFormJson = await typeForm.CreateTypeFormAsync(typeFormTemplateJson);
            dynamic cretedTypeForm = JObject.Parse(cretedTypeFormJson);

            return cretedTypeForm._links.display;
        }

        private Spreadsheet CreateSpreadsheetForTrackingInquiry(Agent agent, List<string> fields, InquiryType typeFormType)
        {
            CreateSpreadsheetCommand createSpreadsheetCommand = new CreateSpreadsheetCommand
            {
                SpreadSheetName = agent.GetSpreadsheetName(Enum.GetName(typeof(InquiryType), typeFormType)),
                WorkSheetName = Enum.GetName(typeof(InquiryType), typeFormType),
                ApplicationName = "LeadsPlus"
            };

            foreach (var item in fields)
            {
                createSpreadsheetCommand.HeaderValues.Add(item);
                createSpreadsheetCommand.InitialValues.Add("test"); // this is required for Zapier to function correctly
            }

            var spreadsheet = mediator.Send(createSpreadsheetCommand).Result;

            //no longer necessary. we will create a sharable link and share.

            //AssigSpreadsheetPermissionCommand assigSpreadsheetPermissionCommand = new AssigSpreadsheetPermissionCommand
            //{
            //    Email = agent.Email,
            //    SpreadsheetId = spreadsheet.SpreadsheetId,
            //    ApplicationName = "LeadsPlus"
            //};

            //var assigSpreadsheetPermissionCommandResult = mediator.Send(assigSpreadsheetPermissionCommand).Result;

            AssigSpreadsheetPermissionCommand assigSpreadsheetPermissionToOrganizationCommand = new AssigSpreadsheetPermissionCommand
            {
                Email = "adfenixleads@gmail.com",
                SpreadsheetId = spreadsheet.SpreadsheetId,
                ApplicationName = "LeadsPlus"
            };

            var assigSpreadsheetPermissionToOrganizationCommandResult = mediator.Send(assigSpreadsheetPermissionToOrganizationCommand).Result;

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