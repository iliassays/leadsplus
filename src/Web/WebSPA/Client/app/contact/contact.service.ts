import { Injectable } from '@angular/core';
import { find, filter } from "lodash";
import { Response, Headers, Http }        from '@angular/http';
import {Observable, throwError, Subject} from 'rxjs';
import {map, catchError} from 'rxjs/operators';
import { from } from 'rxjs/observable/from';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse, HttpHeaders } from '@angular/common/http';
import { ConfigurationService } from '../shared/services/configuration.service';
import { IContact} from './contact.model';
import { DataService } from '../shared/services/data.service';
import { ContactWrapperService } from './contact.wrapper.service';

@Injectable()
export class ContactService {
    
    private contactUrl: string = '';

    constructor(private http: HttpClient,
        private configurationService: ConfigurationService,
        private service: DataService,
        private agnetEvents: ContactWrapperService) { 

        if (this.configurationService.isReady) {
                this.contactUrl = this.configurationService.serverSettings.contactUrl;
            } else{
            this.configurationService.settingsLoaded$.subscribe(x => this.contactUrl = this.configurationService.serverSettings.contactUrl);
            }
        }

    getAgents(pageIndex: number, pageSize: number): Observable<IContact[]> {
        const url = this.contactUrl + '/api/v1/queries/getallcontact';

        const requestUrl =
            `${url}?pageIndex=${pageIndex}&pageSize=${pageSize}`;

        return this.service.get<IContact[]>(requestUrl).map((response: HttpResponse<IContact[]>) => {
            return response.body;
        });
    }
}


