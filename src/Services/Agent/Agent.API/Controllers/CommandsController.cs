using Agent.Command;
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
        [Route("updateagentintigrationemail")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdategentIntigrationEmail([FromBody] UpdateAgentIntigrationEmailAccountCommand updateAgentIntigrationEmailAccountCommand)
        {
            var result = await mediator.Send(updateAgentIntigrationEmailAccountCommand);
            
            return result ?
                (IActionResult) Ok(result) :
                (IActionResult) BadRequest();
        }

        [Route("updateagentsocialmedia")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateAgentSocialMedia([FromBody] UpdateAgentSocialMediaCommand @command)
        {
            var result = await mediator.Send(@command);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
        }

        [Route("updateagentlogo")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateAgentLogo([FromBody] UpdateAgentLogoCommand @command)
        {
            var result = await mediator.Send(@command);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
            

        }

        [Route("markagentaslaunched")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> MarkAgentAsLaunched([FromBody] MarkAgentAsLaunched @command)
        {
            var result = await mediator.Send(@command);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
        }

        [Route("createagentintigrationemail")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAgentIntigrationEmail([FromBody] CreateAgentIntigrationEmailAccountCommand createAgentIntigrationEmailAccountCommand)
        {
            var result = await mediator.Send(createAgentIntigrationEmailAccountCommand);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
        }

        [Route("updateagentdatastudiourl")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAgentDataStudioUrl([FromBody] UpdateAgentDataStudioUrlCommand updateAgentDataStudioUrlCommand)
        {
            var result = await mediator.Send(updateAgentDataStudioUrlCommand);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
        }

        [Route("updateagentautorespondertemplateforbuyinquiry")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAgentAutoresponderTemplateForBuyInquiry([FromBody] UpdateAgentAutoresponderTemplateForBuyInquiryCommand @command)
        {
            var result = await mediator.Send(@command);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
        }

        [Route("updateagentautorespondertemplateforrentinquiry")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAgentAutoresponderTemplateForRentInquiry([FromBody] UpdateAgentAutoresponderTemplateForRentInquiryCommand @command)
        {
            var result = await mediator.Send(@command);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
        }

        [Route("createagenttypeformaccountforbuyinquiry")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAgentTypeFormAccountForBuyInquiry([FromBody] CreateAgentTypeFormAccountForBuyInquiryCommand @command)
        {
            var result = await mediator.Send(@command);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
        }

        [Route("createagenttypeformaccountforrentinquiry")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAgentTypeFormAccountForRentInquiryCommand([FromBody] CreateAgentTypeFormAccountForRentInquiryCommand @command)
        {
            var result = await mediator.Send(@command);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
        }

        [Route("createagentapreadsheetaccountforbuyinquiry")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAgentSpreadsheetAccountForBuyInquiry([FromBody] CreateAgentSpreadsheetAccountForBuyInquiryCommand @command)
        {
            var result = await mediator.Send(@command);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
        }

        [Route("createagentapreadsheetaccountforrentinquiry")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAgentSpreadsheetAccountForRentInquiry([FromBody] CreateAgentSpreadsheetAccountForRentInquiryCommand @command)
        {
            var result = await mediator.Send(@command);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
        }

        [Route("updateagentspreadsheetforbuyinquiry")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAgentSpreadsheetForBuyInquiryCommand([FromBody] UpdateAgentSpreadsheetForBuyInquiryCommand @command)
        {
            var result = await mediator.Send(@command);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
        }

        [Route("updateagentmortgagespreadsheetforbuyinquiry")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAgentMortgageSpreadsheetForBuyInquiryCommand([FromBody] UpdateAgentMortgageSpreadsheetForBuyInquiryCommand @command)
        {
            var result = await mediator.Send(@command);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
        }

        [Route("updateagentspreadsheetforrentinquiry")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAgentSpreadsheetForRentInquiryCommand([FromBody] UpdateAgentSpreadsheetForRentInquiryCommand @command)
        {
            var result = await mediator.Send(@command);

            return result ?
                (IActionResult)Ok(result) :
                (IActionResult)BadRequest();
        }

        [Route("updateagentlandlordspreadsheetforrentinquiry")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAgentLandlordSpreadsheetForRentInquiryCommand([FromBody] UpdateAgentLandlordSpreadsheetForRentInquiryCommand @command)
        {
            var result = await mediator.Send(@command);

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
