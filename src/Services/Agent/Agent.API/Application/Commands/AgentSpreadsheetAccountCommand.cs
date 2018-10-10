using MediatR;

namespace Agent.Command
{

    public class CreateAgentSpreadsheetAccountForBuyInquiryCommand : Command, IRequest<bool>
    {

    }

    public class CreateAgentSpreadsheetAccountForRentInquiryCommand : Command, IRequest<bool>
    {

    }

    public class UpdateAgentSpreadsheetForBuyInquiryCommand : Command, IRequest<bool>
    {
        public string SpreadsheetUrl { get; set; }
        public string SpreadsheetId { get; set; }
        public string SpreadsheetName { get; set; }
        public string SpreadsheetShareableUrl { get; set; }
    }

    public class UpdateAgentMortgageSpreadsheetForBuyInquiryCommand : Command, IRequest<bool>
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

    public class UpdateAgentLandlordSpreadsheetForRentInquiryCommand : Command, IRequest<bool>
    {
        public string SpreadsheetUrl { get; set; }
        public string SpreadsheetId { get; set; }
        public string SpreadsheetName { get; set; }
        public string SpreadsheetShareableUrl { get; set; }
    }
}
