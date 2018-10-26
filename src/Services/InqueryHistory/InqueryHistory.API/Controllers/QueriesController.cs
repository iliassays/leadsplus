namespace InqueryHistory.API.Controllers
{
    using InqueryHistory.Domain;
    using InqueryHistory.Domain.Query;
    using InqueryHistory.IntegrationEvents;
    using InvitationHistory.IntegrationEvents;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core.Query;
    using MediatR;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using System.Linq;

    [Route("api/v1/[controller]")]
    //[Authorize]
    public class QueriesController : ControllerBase
    {
        private readonly IQueryExecutor queryExecutor;
        private readonly IMediator mediator;
        private readonly IEventBus eventBus;

        public QueriesController(IQueryExecutor queryExecutor, IMediator mediator, IEventBus eventBus, IHostingEnvironment env)
        {
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        [Route("getall")]
        [HttpGet]
        //[ProducesResponseType(typeof(List<Agent>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await queryExecutor.Execute<GetAllInqueryHistoryQuery, List<InqueryHistory>>(new GetAllInqueryHistoryQuery());
            return Ok(contacts.Select(c => new
            {
                organizationDomain = c.OrganizationInfo?.OrganizationDomain,
                organizationName = c.OrganizationInfo?.OrganizationName,
                customerEmail = c.CustomerInfo?.Email,
                agentEmail = c.AgentEmail,
                inquiryStatus = Enum.GetName(typeof(InqueryStatus), c.InqueryStatus),
                inquiryType = Enum.GetName(typeof(InquiryType), c.InquiryType),
                createdDate = c.CreatedDate
            }).OrderByDescending(c => c.createdDate));
        }
    }
}
