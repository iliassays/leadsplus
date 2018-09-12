﻿using Agent.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Agent.API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class CommandsController : ControllerBase
    {
        private readonly IMediator mediator;
        
        public CommandsController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
         
        //POST api/v1/[controller]/create
        [Route("create")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAgent([FromBody] CreateAgentCommand newAgentRequest)
        {
            var result = await mediator.Send(newAgentRequest);
           
            return result ? 
                (IActionResult)Ok(result) : 
                (IActionResult)BadRequest();
        }

        //POST api/v1/[controller]/update
        [Route("update")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAgent([FromBody] UpdateAgentCommand updategentCommand)
        {
            var result = await mediator.Send(updategentCommand);

            return result ?
                (IActionResult) Ok() :
                (IActionResult) BadRequest();
        }

        //POST api/v1/[controller]/CreateAgentIntigrationEmail
        [Route("createagentintigrationemail")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAgentIntigrationEmail([FromBody] CreateAgentIntigrationEmailAccountCommand createAgentIntigrationEmailAccountCommand)
        {
            var result = await mediator.Send(createAgentIntigrationEmailAccountCommand);
            
            return result ?
                (IActionResult) Ok(result) :
                (IActionResult) BadRequest();
        }

        //POST api/v1/[controller]/CreateAgentCloudMallinAccount
        [Route("createagenttypeformaccount")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAgentTypeFormAccount([FromBody] CreateAgentTypeFormAccountCommand createAgentTypeFormAccount)
        {
            var result = await mediator.Send(createAgentTypeFormAccount);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
        }

        [Route("Delete")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(DeleteAgentCommand deleteAgentCommand)
        {
            var result = await mediator.Send(deleteAgentCommand);

            return NoContent();
        }
    }
}
