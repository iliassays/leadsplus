import { Injectable } from '@angular/core';
import { find, filter } from "lodash";
import { Response, Headers, Http }        from '@angular/http';
import {Observable, throwError, Subject} from 'rxjs';
import {map, catchError} from 'rxjs/operators';
import { from } from 'rxjs/observable/from';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse, HttpHeaders } from '@angular/common/http';
import { ConfigurationService } from '../shared/services/configuration.service';
import { IAgent, IAgentTypeForm} from './agent.model';
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
            `${this.agentUrl}/api/v1/commands/createagentintigrationemail`;

        let data = {
            AggregateId: agent.id,
            MailboxName: integrationEmail
        };
        debugger;
        return this.service.post(requestUrl, data).map((response: HttpResponse<Object>) => {
            this.agnetEvents.agentUpdated();
            return response.body.toString();
        });
    }

    generateTypeformAccount(agentId: string): Observable<IAgentTypeForm> {
        const requestUrl =
            `${this.agentUrl}/api/v1/commands/createagenttypeformaccount`;
        debugger;
        let data = {
            aggregateId: agentId
        };

        return this.service.post<IAgentTypeForm>(requestUrl, data).map((response: HttpResponse<IAgentTypeForm>) => {
            this.agnetEvents.agentUpdated();
            return response.body;
        });
    }
}


