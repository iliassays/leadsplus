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
            inqueryHistoryToUpdate.CustomerInfo = PrepareCustomerInfoFromExtractedFields(inqueryHistoryToUpdate.ExtractedFields);
            inqueryHistoryToUpdate.OrganizationInfo = PrepareOrganizationInfoFromExtractedFields(inqueryHistoryToUpdate.ExtractedFields, inqueryHistoryToUpdate.OrganizationInfo);
            inqueryHistoryToUpdate.PropertyInfo = PreparePropertyInfoFromExtractedFields(inqueryHistoryToUpdate.ExtractedFields);
            
            var filter = Builders<InqueryHistory>.Filter.Eq("Id", @event.AggregateId);
            var update = Builders<InqueryHistory>.Update
                .Set("InqueryStatus", inqueryHistoryToUpdate.InqueryStatus)
                .Set("ExtractedFields", inqueryHistoryToUpdate.ExtractedFields)
                .Set("CustomerInfo", inqueryHistoryToUpdate.CustomerInfo)
                .Set("OrganizationInfo", inqueryHistoryToUpdate.OrganizationInfo)
                .Set("PropertyInfo", inqueryHistoryToUpdate.PropertyInfo)
                .CurrentDate("UpdatedDate");

            await inqueryHistoryRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            await mediator.DispatchDomainEventsAsync(inqueryHistoryToUpdate);
        }

        private CustomerInfo PrepareCustomerInfoFromExtractedFields(Dictionary<string, string> extractedFields)
        {
            var customerEmail = extractedFields.ContainsKey("customeremail") ? extractedFields["customeremail"] : "";
            customerEmail = customerEmail?.Split(" ")[0];

            return new CustomerInfo()
            {
                Email = customerEmail,
                Phone = extractedFields.ContainsKey("customerphone") ? extractedFields["customerphone"] : "",
                City = extractedFields.ContainsKey("customercity") ? extractedFields["customercity"] : "",
                Company = extractedFields.ContainsKey("customercompany") ? extractedFields["customercompany"] : "",
                Address = extractedFields.ContainsKey("customeraddress") ? extractedFields["customeraddress"] : "",
                Country = extractedFields.ContainsKey("customercountry") ? extractedFields["customercountry"] : "",
                Firstname = extractedFields.ContainsKey("customerfirstname") ? extractedFields["customerfirstname"] : "",
                Lastname = extractedFields.ContainsKey("customerlastname") ? extractedFields["customerlastname"] : "",
                Aboutme = extractedFields.ContainsKey("customeraboutme") ? extractedFields["customeraboutme"] : "",
                
            };
        }

        private OrganizationInfo PrepareOrganizationInfoFromExtractedFields(Dictionary<string, string> extractedFields, OrganizationInfo organizationInfo)
        {
            organizationInfo.OrganizationDomain = extractedFields.ContainsKey("organizationdomain") ? extractedFields["organizationdomain"] : "";
            organizationInfo.OrganizationName = extractedFields.ContainsKey("organizationname") ? extractedFields["organizationname"] : "";

            return organizationInfo;
        }

        private PropertyInfo PreparePropertyInfoFromExtractedFields(Dictionary<string, string> extractedFields)
        {
            return new PropertyInfo()
            {
                ReferenceNo = extractedFields.ContainsKey("propertyreferenceno") ? extractedFields["propertyreferenceno"] : "",
                PropertyAddress = extractedFields.ContainsKey("propertyaddress") ? extractedFields["propertyaddress"] : "",
                PropertyUrl = extractedFields.ContainsKey("propertyurl") ? extractedFields["propertyurl"] : "",
                Message = extractedFields.ContainsKey("proerptymessage") ? extractedFields["proerptymessage"] : "",
            };
        }
    }
}
