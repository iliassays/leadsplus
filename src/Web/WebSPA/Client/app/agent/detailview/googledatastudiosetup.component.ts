import {Component, OnInit, ViewChild} from '@angular/core';
import {DataSource} from '@angular/cdk/collections';
import {MatPaginator, MatSort} from '@angular/material';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import {merge, Observable, of as observableOf} from 'rxjs';
import {catchError, map, startWith, switchMap} from 'rxjs/operators';
import { AgentWrapperService } from '../agent.wrapper.service';
import { IAgent } from '../agent.model';
import { AgentService } from '../agent.service';

/**
 * @title Table with pagination
 */
@Component({
  selector: 'agent-google-datastudio-setup',
  styleUrls: [],
  templateUrl: 'googledatastudiosetup.component.html',
})
export class AgnetGoogleDataStudioSetupComponent implements OnInit {

    currentAgent = <IAgent>{

    };
    integrationEmail;
    isAgentProcessing: boolean;
    errorReceived: boolean;
    zapierScript;

    constructor(private agentService: AgentService,
        private agnetEvents: AgentWrapperService) {

    }

    public saveDataStudioLink() {
        this.agentService.updateAgentDatastudioUrl(this.currentAgent.id,
            this.currentAgent.dataStudioUrl)
            .catch((errMessage) => {
                this.errorReceived = true;
                this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(response => {

            });
    }

    ngOnInit() {
        this.agnetEvents.currentAgentChanged$.subscribe(
            agent => {

                this.currentAgent = agent;
            });
    }
}
