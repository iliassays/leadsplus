using MediatR;

namespace Agent.Command
{
    public class CreateAgentCommand : Command, IRequest<bool>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }

        public string OwnerId { get; set; }
    }

    public class UpdateAgentCommand : Command, IRequest<bool>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
    }

    public class UpdateAgentLogoCommand : Command, IRequest<bool>
    {
        public string Logo { get; set; }
    }

    public class MarkAgentAsLaunched : Command, IRequest<bool>
    {
        
    }

    public class DeleteAgentCommand : Command, IRequest<bool>
    {
        
    }

    public class CreateAgentIntigrationEmailAccountCommand : Command, IRequest<bool>
    {
        
    }

    public class UpdateAgentIntigrationEmailAccountCommand : Command, IRequest<bool>
    {
        public string MailboxName { get; set; }
    }

    public class UpdateAgentSocialMediaCommand : Command, IRequest<bool>
    {
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
    }

    public class UpdateAgentDataStudioUrlCommand : Command, IRequest<bool>
    {
        public string DataStudioUrl { get; set; }
    }

    public class UpdateAgentAutoresponderTemplateForBuyInquiryCommand : Command, IRequest<bool>
    {
        public string AgentAutoresponderTemplateForBuyInquiryId { get; set; }
        public string CustomerAutoresponderTemplateForBuyInquiryId { get; set; }
    }

    public class UpdateAgentAutoresponderTemplateForRentInquiryCommand : Command, IRequest<bool>
    {
        public string AgentAutoresponderTemplateForRentInquiryId { get; set; }
        public string CustomerAutoresponderTemplateForRentInquiryId { get; set; }
    }
}
