namespace InqueryHistory.Command
{
    using InqueryHistory.Domain;
    using MediatR;

    public class UpdateInqueryStatusToSentForParsingCommand : Command, IRequest<bool>
    {
        
    }
}
