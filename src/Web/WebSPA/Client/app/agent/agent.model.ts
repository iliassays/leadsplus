export interface IAgent {
    firstname: string;
    lastname: string;
    city: string;
    country: string;
    company: string;
    phone: string;
    address: string;
    email: string;
    integrationEmail: string;
    id: string;
    aggregateId: string;
    dataStudioUrl: string;
    buyInquiry: IAgentBuyInquiry;
    rentInquiry: IAgentRentInquiry
}

export interface IAgentBuyInquiry {
    typeFormUrl: string;
    spreadsheetUrl: string;
    spreadsheetId: string;
    spreadsheetName: string;
    type: string;
    mortgageSpreadsheetUrl: string;
    mortgageSpreadsheetId: string;
    mortgageSpreadsheetName: string;
    inquiryAutoresponderTemplate: IInquiryAutoresponderTemplate;
}

export interface IAgentRentInquiry {
    typeFormUrl: string;
    spreadsheetUrl: string;
    spreadsheetId: string;
    spreadsheetName: string;
    type: string;
    landlordSpreadsheetUrl: string;
    landlordSpreadsheetId: string;
    landlordSpreadsheetName: string;
    inquiryAutoresponderTemplate: IInquiryAutoresponderTemplate;
}

export interface IInquiryAutoresponderTemplate {
    agentAutoresponderTemplateId: string;
    customerAutoresponderTemplateId: string;
}