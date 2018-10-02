using MediatR;

namespace Agent.Command
{

    public class CreateAgentSpreadsheetAccountForBuyInquiryCommand : Command, IRequest<bool>
    {

    }

    public class CreateAgentSpreadsheetAccountForRentInquiryCommand : Command, IRequest<bool>
    {

    }
}
