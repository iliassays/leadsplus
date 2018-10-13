using MediatR;

namespace Agent.Command
{

    public class CreateAgentSpreadsheetAccountForBuyInquiryCommand : Command, IRequest<bool>
    {

    }

    public class CreateAgentSpreadsheetAccountForRentInquiryCommand : Command, IRequest<bool>
    {

    }

    public class CreateAgentMortgageSpreadsheetCommand : Command, IRequest<bool>
    {

    }

    public class CreateAgentLandlordSpreadsheetCommand : Command, IRequest<bool>
    {

    }

    public class CreateAgentVendorSpreadsheetCommand : Command, IRequest<bool>
    {

    }

    public class UpdateAgentSpreadsheetForBuyInquiryCommand : Command, IRequest<bool>
    {
        public string SpreadsheetUrl { get; set; }
        public string SpreadsheetId { get; set; }
        public string SpreadsheetName { get; set; }
        public string SpreadsheetShareableUrl { get; set; }
    }

    public class UpdateAgentMortgageSpreadsheetCommand : Command, IRequest<bool>
    {
        public string SpreadsheetUrl { get; set; }
        public string SpreadsheetId { get; set; }
        public string SpreadsheetName { get; set; }
        public string SpreadsheetShareableUrl { get; set; }
    }

    public class UpdateAgentSpreadsheetForRentInquiryCommand : Command, IRequest<bool>
    {
        public string SpreadsheetUrl { get; set; }
        public string SpreadsheetId { get; set; }
        public string SpreadsheetName { get; set; }
        public string SpreadsheetShareableUrl { get; set; }
    }

    public class UpdateAgentLandlordSpreadsheetCommand : Command, IRequest<bool>
    {
        public string SpreadsheetUrl { get; set; }
        public string SpreadsheetId { get; set; }
        public string SpreadsheetName { get; set; }
        public string SpreadsheetShareableUrl { get; set; }
    }

    public class UpdateAgentVendorSpreadsheetCommand : Command, IRequest<bool>
    {
        public string SpreadsheetUrl { get; set; }
        public string SpreadsheetId { get; set; }
        public string SpreadsheetName { get; set; }
        public string SpreadsheetShareableUrl { get; set; }
    }
}
