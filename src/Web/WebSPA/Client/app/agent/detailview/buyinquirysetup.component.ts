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
  selector: 'agent-buy-inquiry-setup',
  styleUrls: [],
    templateUrl: 'buyinquirysetup.component.html',
})
export class AgnetBuyInquirySetupComponent implements OnInit, AfterViewInit {

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

    public saveSpreadsheet() {
        this.agentService.updateAgentSpreadsheetShareableLinkForBuyInquiry(this.currentAgent.id,
            this.currentAgent.buyInquiry.spreadsheetUrl,
            this.currentAgent.buyInquiry.spreadsheetId,
            this.currentAgent.buyInquiry.spreadsheetName,
            this.currentAgent.buyInquiry.spreadsheetShareableUrl)
            .catch((errMessage) => {
                this.errorReceived = true;
                this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(response => {

            });
    }

    public saveMortgageSpreadsheet() {
        this.agentService.updateAgentMortgageSpreadsheetShareableLinkForBuyInquiry(this.currentAgent.id,
            this.currentAgent.buyInquiry.mortgageSpreadsheetUrl,
            this.currentAgent.buyInquiry.mortgageSpreadsheetId,
            this.currentAgent.buyInquiry.mortgageSpreadsheetName,
            this.currentAgent.buyInquiry.mortgageSpreadsheetShareableUrl)
            .catch((errMessage) => {
                this.errorReceived = true;
                this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(response => {

            });
    }

    public saveAutoresponderTemplate() {
        this.agentService.updateAgentAutoresponderTemplateForBuyInquiry(this.currentAgent.id,
            this.currentAgent.buyInquiry.inquiryAutoresponderTemplate.agentAutoresponderTemplateId,
            this.currentAgent.buyInquiry.inquiryAutoresponderTemplate.customerAutoresponderTemplateId)
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
      
        this.zapierScript = document.createElement('script');
        this.zapierScript.type = 'text/javascript';
        this.zapierScript.asyc = true;
        this.zapierScript.src =
            'https://zapier.com/apps/embed/widget.js?services=typeform&html_id=zap_integration'
        $(this.el.nativeElement).find('#zap_integration').append(this.zapierScript);
    }
}

