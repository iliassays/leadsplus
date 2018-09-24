import { Injectable } from '@angular/core';
import { find, filter } from "lodash";
import { Response, Headers, Http } from '@angular/http';
import { Observable, throwError, Subject } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { from } from 'rxjs/observable/from';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse, HttpHeaders } from '@angular/common/http';
import { ConfigurationService } from '../shared/services/configuration.service';
import { IInquiryHistory } from './inquiryhistory.model';
import { DataService } from '../shared/services/data.service';

@Injectable()
export class InquiryHistoryService {

    private inquiryHistoryUrl: string = '';

    constructor(private http: HttpClient,
        private configurationService: ConfigurationService,
        private service: DataService) {

        if (this.configurationService.isReady) {
            this.inquiryHistoryUrl = this.configurationService.serverSettings.inquiryHistoryUrl;
        } else {
            this.configurationService.settingsLoaded$.subscribe(x => this.inquiryHistoryUrl = this.configurationService.serverSettings.inquiryHistoryUrl);
        }
    }

    getInquiryHistories(sort: string, order: string, pageIndex: number, pageSize: number): Observable<IInquiryHistory[]> {
        const url = this.inquiryHistoryUrl + '/api/v1/queries/getall';

        const requestUrl =
            `${url}?pageIndex=${pageIndex}&pageSize=${pageSize}`;

        return this.service.get<IInquiryHistory[]>(requestUrl).map((response: HttpResponse<IInquiryHistory[]>) => {
            return response.body;
        });
    }
}


