import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { merge, Observable, of as observableOf } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IAgent } from '../agent.model';
import { AgentService } from '../agent.service';
import { AgentWrapperService } from '../agent.wrapper.service';

/**
 * @title Table with pagination
 */
@Component({
  selector: 'agent-rent-inquiry-setup',
  styleUrls: [],
    templateUrl: 'rentinquirysetup.component.html',
})
export class AgnetRentInquirySetupComponent implements OnInit {

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
        this.agentService.updateAgentSpreadsheetShareableLinkForRentInquiry(this.currentAgent.id,
            this.currentAgent.rentInquiry.spreadsheetUrl,
            this.currentAgent.rentInquiry.spreadsheetId,
            this.currentAgent.rentInquiry.spreadsheetName,
            this.currentAgent.rentInquiry.spreadsheetShareableUrl)
            .catch((errMessage) => {
                this.errorReceived = true;
                this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(response => {

            });
    }

    public saveAutoresponderTemplate() {
        this.agentService.updateAgentAutoresponderTemplateForRentInquiry(this.currentAgent.id,
            this.currentAgent.rentInquiry.inquiryAutoresponderTemplate.agentAutoresponderTemplateId,
            this.currentAgent.rentInquiry.inquiryAutoresponderTemplate.customerAutoresponderTemplateId)
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
