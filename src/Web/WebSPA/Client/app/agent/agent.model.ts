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
    facebook: string;
    instagram: string;
    twitter: string;
    linkedIn: string;
    logo: string;
    id: string;
    aggregateId: string;
    dataStudioUrl: string;
    isLaunched: boolean;
    buyInquiry: IAgentBuyInquiry;
    rentInquiry: IAgentRentInquiry;
    agentSpreadsheet: IAgentSpreadsheet;
}

export interface IAgentBuyInquiry {
    typeFormUrl: string;
    spreadsheetUrl: string;
    spreadsheetId: string;
    spreadsheetName: string;
    spreadsheetShareableUrl: string;
    type: string;
    inquiryAutoresponderTemplate: IInquiryAutoresponderTemplate;
}

export interface IAgentRentInquiry {
    typeFormUrl: string;
    spreadsheetUrl: string;
    spreadsheetId: string;
    spreadsheetName: string;
    spreadsheetShareableUrl: string;
    type: string;    
    inquiryAutoresponderTemplate: IInquiryAutoresponderTemplate;
}

export interface IAgentSpreadsheet {    
    mortgageSpreadsheetUrl: string;
    mortgageSpreadsheetId: string;
    mortgageSpreadsheetName: string;
    mortgageSpreadsheetShareableUrl: string;

    landlordSpreadsheetUrl: string;
    landlordSpreadsheetId: string;
    landlordSpreadsheetName: string;
    landlordSpreadsheetShareableUrl: string;

    vendorSpreadsheetUrl: string;
    vendorSpreadsheetId: string;
    vendorSpreadsheetName: string;
    vendorSpreadsheetShareableUrl: string;
}

export interface IInquiryAutoresponderTemplate {
    agentAutoresponderTemplateId: string;
    customerAutoresponderTemplateId: string;
}