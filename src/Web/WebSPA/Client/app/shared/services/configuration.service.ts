import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { IConfiguration } from '../models/configuration.model';
import { StorageService } from './storage.service';

import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class ConfigurationService {
    serverSettings: IConfiguration;
    // observable that is fired when settings are loaded from server
    private settingsLoadedSource = new Subject();
    settingsLoaded$ = this.settingsLoadedSource.asObservable();
    isReady: boolean = false;

    constructor(private http: HttpClient, private storageService: StorageService) { }

    load() {
        //const baseURI = document.baseURI.endsWith('/') ? document.baseURI : `${document.baseURI}/`;
        let url = `/Home/Configuration`;

        this.http.get<IConfiguration>(url).subscribe(
            response => {
                console.log('server settings loaded');
                this.serverSettings = response;
                console.log(this.serverSettings);
                this.storageService.store('identityUrl', this.serverSettings.identityUrl);
                this.storageService.store('agentUrl', this.serverSettings.agentUrl);
                this.storageService.store('signalrHubUrl', this.serverSettings.signalrHubUrl);
                this.isReady = true;
                this.settingsLoadedSource.next();
            }
        );
    }
}
