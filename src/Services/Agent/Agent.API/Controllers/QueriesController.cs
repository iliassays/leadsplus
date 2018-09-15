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

    [Route("api/v1/[controller]")]
    //[Authorize]
    public class QueriesController : ControllerBase
    {
        private readonly IQueryExecutor queryExecutor;
        private readonly IMediator mediator;

        public QueriesController(IQueryExecutor queryExecutor, IMediator mediator, IHostingEnvironment env)
        {
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
