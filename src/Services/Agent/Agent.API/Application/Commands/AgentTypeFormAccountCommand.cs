using MediatR;

namespace Agent.Command
{

    public class CreateAgentTypeFormAccountForBuyInquiryCommand : Command, IRequest<bool>
    {
        
    }

    public class CreateAgentTypeFormAccountForRentInquiryCommand : Command, IRequest<bool>
    {

    }

    public class CreateAgentSpreadsheetAccountForBuyInquiryCommand : Command, IRequest<bool>
    {

    }

    public class CreateAgentSpreadsheetAccountForRentInquiryCommand : Command, IRequest<bool>
    {

    }

    public class CreateAgentTypeformWithSpreadsheetAccountCommand : Command, IRequest<bool>
    {

    }
}
