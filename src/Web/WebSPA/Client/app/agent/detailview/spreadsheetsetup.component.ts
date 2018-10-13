import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import {DataSource} from '@angular/cdk/collections';
import {MatPaginator, MatSort} from '@angular/material';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import {merge, Observable, of as observableOf} from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IAgent } from '../agent.model';
import { AgentService } from '../agent.service';
import { AgentWrapperService } from '../agent.wrapper.service';

/**
 * @title Table with pagination
 */
@Component({
  selector: 'agent-spreadsheet-setup',
  styleUrls: [],
    templateUrl: 'spreadsheetsetup.component.html',
})
export class AgnetSpreadsheetSetupComponent implements OnInit, AfterViewInit {

    currentAgent = <IAgent>{
        
    };
    integrationEmail;
    isAgentProcessing: boolean;
    errorReceived: boolean;
    zapierScript;

    constructor(private agentService: AgentService,
        private agnetEvents: AgentWrapperService,
        private formBuilder: FormBuilder,
        private el: ElementRef) {

    }

    public saveMortgageSpreadsheet() {
        this.agentService.updateAgentMortgageSpreadsheetShareableLink(this.currentAgent.id,
            this.currentAgent.agentSpreadsheet.mortgageSpreadsheetUrl,
            this.currentAgent.agentSpreadsheet.mortgageSpreadsheetId,
            this.currentAgent.agentSpreadsheet.mortgageSpreadsheetName,
            this.currentAgent.agentSpreadsheet.mortgageSpreadsheetShareableUrl)
            .catch((errMessage) => {
                this.errorReceived = true;
                this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(response => {

            });
    }

    public saveLandlordSpreadsheet() {
        this.agentService.updateAgentLandlordSpreadsheetShareableLink(this.currentAgent.id,
            this.currentAgent.agentSpreadsheet.landlordSpreadsheetUrl,
            this.currentAgent.agentSpreadsheet.landlordSpreadsheetId,
            this.currentAgent.agentSpreadsheet.landlordSpreadsheetName,
            this.currentAgent.agentSpreadsheet.landlordSpreadsheetShareableUrl)
            .catch((errMessage) => {
                this.errorReceived = true;
                this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(response => {

            });
    }

    public saveVendorSpreadsheet() {
        this.agentService.updateAgentVendorSpreadsheetShareableLink(this.currentAgent.id,
            this.currentAgent.agentSpreadsheet.vendorSpreadsheetUrl,
            this.currentAgent.agentSpreadsheet.vendorSpreadsheetId,
            this.currentAgent.agentSpreadsheet.vendorSpreadsheetName,
            this.currentAgent.agentSpreadsheet.vendorSpreadsheetShareableUrl)
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

    ngAfterViewInit() {
      
    }
}

