﻿import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpErrorResponse } from '@angular/common/http';

import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { SecurityService } from './security.service';
import { Guid } from '../../../guid';

// Implementing a Retry-Circuit breaker policy 
// is pending to do for the SPA app
@Injectable()
export class DataService {
    constructor(private http: HttpClient, private securityService: SecurityService) { }

    get<T>(url: string, params?: any): Observable<HttpResponse<T>> {
        let headers: HttpHeaders = new HttpHeaders();

        if (this.securityService) {
            headers = headers.set('Authorization', 'Bearer ' + this.securityService.GetToken());
        }

        return this.http.get(url, { headers: headers, observe: "response" }).map(
            (res: HttpResponse<T>) => res).catch(this.handleError);
    }

    postWithId<T>(url: string, data: any, params?: any): Observable<HttpResponse<T>> {
        return this.doPost(url, data, true, params);
    }

    post<T>(url: string, data: any, params?: any): Observable<HttpResponse<T>> {
        return this.doPost(url, data, false, params);
    }

    putWithId<T>(url: string, data: any, params?: any): Observable<HttpResponse<T>> {
        return this.doPut(url, data, true, params);
    }

    private doPost<T>(url: string, data: any, needId: boolean, params?: any): Observable<HttpResponse<T>> {
        let headers: HttpHeaders = new HttpHeaders();

        if (this.securityService) {
            headers = headers.set('Authorization', 'Bearer ' + this.securityService.GetToken());
        }
        if (needId) {
            let guid = Guid.newGuid();
            headers = headers.set('x-requestid', guid);
        }

        return this.http.post(url, data, { headers: headers, observe: 'response' }).map(
            (res: HttpResponse<T>) => {
                return res;
            }).catch(this.handleError);
    }

    private doPut<T>(url: string, data: any, needId: boolean, params?: any): Observable<HttpResponse<T>> {
        let headers: HttpHeaders = new HttpHeaders();
        if (this.securityService) {
            headers = headers.set('Authorization', 'Bearer ' + this.securityService.GetToken());
        }
        if (needId) {
            let guid = Guid.newGuid();
            headers = headers.set('x-requestid', guid);
        }

        return this.http.put(url, data, { headers: headers, observe: 'response' }).map(
            (res: HttpResponse<T>) => {
                return res;
            }).catch(this.handleError);
    }

    delete(url: string, params?: any) {
        let headers: HttpHeaders = new HttpHeaders();

        if (this.securityService) {
            headers = headers.set('Authorization', 'Bearer ' + this.securityService.GetToken());
        }

        console.log('data.service deleting');

        this.http.delete(url, { headers: headers, observe: 'response' }).subscribe((res) => {
            console.log('deleted');
        });
    }

    private handleError(error: HttpErrorResponse) {
        console.error('server error:', error);
        if (error.error instanceof Error) {
            let errMessage = '';
            if (error.error.message && error.error.message !== '') {
                errMessage = error.error.message;
            }
            else {
                errMessage = error.status.toString();
            }
            return Observable.throw(errMessage);
        }
        return Observable.throw(error || 'server error');
    }
}
