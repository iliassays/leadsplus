import { Injectable } from '@angular/core';
import { find, filter } from "lodash";
import { Response, Headers, Http }        from '@angular/http';
import {Observable, throwError, Subject} from 'rxjs';
import {map, catchError} from 'rxjs/operators';
import { from } from 'rxjs/observable/from';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse, HttpHeaders } from '@angular/common/http';
import { ConfigurationService } from '../shared/services/configuration.service';
import { IAgent} from './agent.model';
import { DataService } from '../shared/services/data.service';
import { AgentWrapperService } from './agent.wrapper.service';

@Injectable()
export class AgentService {
    
    private agentUrl: string = '';

    constructor(private http: HttpClient,
        private configurationService: ConfigurationService,
        private service: DataService,
        private agnetEvents: AgentWrapperService) { 

        if (this.configurationService.isReady) {
                this.agentUrl = this.configurationService.serverSettings.agentUrl;
            } else{
                this.configurationService.settingsLoaded$.subscribe(x => this.agentUrl = this.configurationService.serverSettings.agentUrl);
            }
        }

    createAgent(agnet: IAgent): Observable<boolean> {            
        let url = this.agentUrl + '/api/v1/commands/create';

        return this.service.post(url, agnet).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentCreated();
            return true;
        });
    }

    updateAgent(agnet: IAgent): Observable<boolean> {
        let url = this.agentUrl + '/api/v1/commands/update';

        return this.service.post(url, agnet).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return true;
        });
    }

    getAgent(id: string): Observable<IAgent> {
        const requestUrl =
            `${this.agentUrl}/api/v1/queries/getagentbyid?id=${id}`;

        return this.service.get<IAgent>(requestUrl).map((response: HttpResponse<IAgent>) => {
            return response.body;
        });
    }    

    getAgents(pageIndex: number, pageSize: number): Observable<IAgent[]> {
        const url = this.agentUrl + '/api/v1/queries/getallagent';

        const requestUrl =
            `${url}?pageIndex=${pageIndex}&pageSize=${pageSize}`;

        return this.service.get<IAgent[]>(requestUrl).map((response: HttpResponse<IAgent[]>) => {
            return response.body;
        });
    }

    updateIntegrationEmailAgent(agent: IAgent, integrationEmail): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/updateagentintigrationemail`;

        let data = {
            AggregateId: agent.id,
            MailboxName: integrationEmail
        };
      
        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    saveLogo(agentId: string, logo: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/updateagentlogo`;

        let data = {
            AggregateId: agentId,
            logo: logo
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    updateAgentSocialMedia(agentId: string, facebook, instagram, twitter, linkedin): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/updateagentsocialmedia`;

        let data = {
            AggregateId: agentId,
            Facebook: facebook,
            Instagram: instagram,
            Twitter: twitter,
            Linkedin: linkedin
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    makedAsLaunched(agentId: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/markagentaslaunched`;

        let data = {
            AggregateId: agentId
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    createAgentIntegrationEmail(agentId: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/createagentintigrationemail`;

        let data = {
            AggregateId: agentId
        };
        
        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    createTypeformAccountForBuyInquiry(agentId: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/createagenttypeformaccountforbuyinquiry`;
        
        let data = {
            aggregateId: agentId
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    createSpreadsheetForBuyInquiry(agentId: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/createagentapreadsheetaccountforbuyinquiry`;
        
        let data = {
            aggregateId: agentId
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    createTypeformAccountForRentInquiry(agentId: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/createagenttypeformaccountforrentinquiry`;

        let data = {
            aggregateId: agentId
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    createSpreadsheetForRentInquiry(agentId: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/createagentapreadsheetaccountforrentinquiry`;

        let data = {
            aggregateId: agentId
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    createSpreadsheetForMortgage(agentId: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/createagentmortgagespreadsheet`;

        let data = {
            aggregateId: agentId
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    createSpreadsheetForLandlord(agentId: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/createagentlandlordspreadsheet`;

        let data = {
            aggregateId: agentId
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    createSpreadsheetForVendor(agentId: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/createagentvendorspreadsheet`;

        let data = {
            aggregateId: agentId
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    updateAgentAutoresponderTemplateForBuyInquiry(agentId: string, agentAutoresponderTemplateForBuyInquiryId: string, customerAutoresponderTemplateForBuyInquiryId: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/updateagentautorespondertemplateforbuyinquiry`;

        let data = {
            aggregateId: agentId,
            agentAutoresponderTemplateForBuyInquiryId: agentAutoresponderTemplateForBuyInquiryId,
            customerAutoresponderTemplateForBuyInquiryId: customerAutoresponderTemplateForBuyInquiryId
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    updateAgentAutoresponderTemplateForRentInquiry(agentId: string, agentAutoresponderTemplateForRentInquiryId: string, customerAutoresponderTemplateForRentInquiryId: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/updateagentautorespondertemplateforrentinquiry`;

        let data = {
            aggregateId: agentId,
            agentAutoresponderTemplateForRentInquiryId: agentAutoresponderTemplateForRentInquiryId,
            customerAutoresponderTemplateForRentInquiryId: customerAutoresponderTemplateForRentInquiryId
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    updateAgentDatastudioUrl(agentId: string, dataStudioUrl: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/updateagentdatastudiourl`;

        let data = {
            aggregateId: agentId,
            dataStudioUrl: dataStudioUrl
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    updateAgentSpreadsheetShareableLinkForBuyInquiry(agentId: string, spreadsheetUrl: string,
        spreadsheetId: string, spreadsheetName: string, spreadsheetShareableUrl: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/updateagentspreadsheetforbuyinquiry`;

        let data = {
            aggregateId: agentId,
            spreadsheetUrl: spreadsheetUrl,
            spreadsheetId: spreadsheetId,
            SpreadsheetName: spreadsheetName,
            SpreadsheetShareableUrl: spreadsheetShareableUrl,
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    updateAgentSpreadsheetShareableLinkForRentInquiry(agentId: string, spreadsheetUrl: string,
        spreadsheetId: string, spreadsheetName: string, spreadsheetShareableUrl: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/updateagentspreadsheetforrentinquiry`;

        let data = {
            aggregateId: agentId,
            spreadsheetUrl: spreadsheetUrl,
            spreadsheetId: spreadsheetId,
            SpreadsheetName: spreadsheetName,
            SpreadsheetShareableUrl: spreadsheetShareableUrl,
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    updateAgentMortgageSpreadsheetShareableLink(agentId: string, spreadsheetUrl: string,
        spreadsheetId: string, spreadsheetName: string, spreadsheetShareableUrl: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/updateagentmortgagespreadsheet`;

        let data = {
            aggregateId: agentId,
            spreadsheetUrl: spreadsheetUrl,
            spreadsheetId: spreadsheetId,
            SpreadsheetName: spreadsheetName,
            SpreadsheetShareableUrl: spreadsheetShareableUrl,
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    updateAgentLandlordSpreadsheetShareableLink(agentId: string, spreadsheetUrl: string,
        spreadsheetId: string, spreadsheetName: string, spreadsheetShareableUrl: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/updateagentlandlordspreadsheet`;

        let data = {
            aggregateId: agentId,
            spreadsheetUrl: spreadsheetUrl,
            spreadsheetId: spreadsheetId,
            SpreadsheetName: spreadsheetName,
            SpreadsheetShareableUrl: spreadsheetShareableUrl,
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    updateAgentVendorSpreadsheetShareableLink(agentId: string, spreadsheetUrl: string,
        spreadsheetId: string, spreadsheetName: string, spreadsheetShareableUrl: string): Observable<string> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/updateagentvendorspreadsheet`;

        let data = {
            aggregateId: agentId,
            spreadsheetUrl: spreadsheetUrl,
            spreadsheetId: spreadsheetId,
            SpreadsheetName: spreadsheetName,
            SpreadsheetShareableUrl: spreadsheetShareableUrl,
        };

        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }
}


