namespace Agent.API.Controllers
{
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

        public QueriesController(IQueryExecutor queryExecutor, IMediator mediator, IHostingEnvironment env)
        {
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        //GET api/v1/[controller]/1
        [Route("sayhello")]
        [HttpGet]
        public async Task<IActionResult> SayHello()
        {
            return Ok("Hello");
        }
    }
}
