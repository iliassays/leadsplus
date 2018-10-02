namespace Agent.API.Controllers
{
    using Agent.Command;
    using Agent.Domain;
    using Agent.Domain.Query;
    using Agent.TypeFormIntegration;
    using Google.Apis.Sheets.v4.Data;
    using LeadsPlus.Core.Query;
    using LeadsPlus.GoogleApis.Command;
    using MediatR;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using System.Linq;

    [Route("api/v1/[controller]")]
    //[Authorize]
    public class QueriesController : ControllerBase
    {
        private readonly IQueryExecutor queryExecutor;
        private readonly IMediator mediator;
        private List<string> customerHeaders = new List<string>()
        {
            { "Id" }, { "Date"}, { "Customer Name"}, { "Customer Email"}, { "Customer Phone"}, { "Customer Address"}, { "About Customer"},
            { "Customer Message"}, { "Enquiry Source"}, { "Enquiry Kind"}, { "Property Address"},
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

        public QueriesController(IQueryExecutor queryExecutor, IMediator mediator, IHostingEnvironment env)
        {
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            CreateSpreadsheetForTrackingInquiryTest(InquiryType.BuyInquiry);
        }

        public Spreadsheet CreateSpreadsheetForTrackingInquiryTest(InquiryType typeFormType)
        {
            CreateSpreadsheetCommand createSpreadsheetCommand = new CreateSpreadsheetCommand
            {
                SpreadSheetName = "Hello World",
                WorkSheetName = Enum.GetName(typeof(InquiryType), typeFormType),
                ApplicationName = "LeadsPlus"
            };

            var fields = customerHeaders.Concat(buyInquiryHeaders).ToList();
            foreach (var item in fields)
            {
                createSpreadsheetCommand.HeaderValues.Add(item);
                createSpreadsheetCommand.InitialValues.Add("test"); // this is required for Zapier to function correctly
            }

            var spreadsheet = mediator.Send(createSpreadsheetCommand).Result;

            AssigSpreadsheetPermissionCommand assigSpreadsheetPermissionCommand = new AssigSpreadsheetPermissionCommand
            {
                Email = "shimulsays@gmail.com",
                SpreadsheetId = spreadsheet.SpreadsheetId,
                ApplicationName = "LeadsPlus"
            };

            var assigSpreadsheetPermissionToOrganizationCommandResult = mediator.Send(assigSpreadsheetPermissionCommand).Result;

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

        //GET api/v1/[controller]/1
        [Route("getagentbyid")]
        [HttpGet]
        [ProducesResponseType(typeof(Agent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAgent(GetAgentQuery query)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(query);

            if (agent is null)
            {
                return NotFound();
            }

            return Ok(agent);
        }

        //GET api/v1/[controller]/
        [Route("getallagent")]
        [HttpGet]
        //[ProducesResponseType(typeof(List<Agent>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAgents()
        {
            var agents = await queryExecutor.Execute<GetAllAgentQuery, List<Agent>>(new GetAllAgentQuery());
            return Ok(agents);
        }
    }
}
