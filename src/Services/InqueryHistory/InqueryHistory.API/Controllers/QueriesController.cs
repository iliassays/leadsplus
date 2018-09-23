namespace Agent.API.Controllers
{
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

        //GET api/v1/[controller]/1
        [Route("sayhello")]
        [HttpGet]
        public async Task<IActionResult> SayHello()
        {

            //EmailParsedIntegrationEvent newInqueryRequestReceivedIntegrationEvent = new EmailParsedIntegrationEvent()
            //{
                
            //};

            //eventBus.Publish(newInqueryRequestReceivedIntegrationEvent);

            return Ok("Hello");
        }
    }
}
