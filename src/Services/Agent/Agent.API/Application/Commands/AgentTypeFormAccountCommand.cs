using MediatR;

namespace Agent.Command
{

    public class CreateAgentTypeFormAccountForBuyInquiryCommand : Command, IRequest<bool>
    {
        
    }

    public class CreateAgentTypeFormAccountForRentInquiryCommand : Command, IRequest<bool>
    {

    }
}
