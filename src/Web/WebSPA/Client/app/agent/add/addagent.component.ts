import {Component, OnInit, ViewChild} from '@angular/core';
import {DataSource} from '@angular/cdk/collections';
import {MatPaginator, MatSort} from '@angular/material';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import { Router } from '@angular/router';
import { map, startWith, catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { IAgent} from '../agent.model';
import { AgentService } from '../agent.service';
import { AgentWrapperService } from '../agent.wrapper.service';
import { Guid } from '../../shared/models/guid';

/**
 * @title Table with pagination
 */
@Component({
  selector: 'agent-add',
  styleUrls: [],
  templateUrl: 'addagent.component.html',
})
export class AgnetAddComponent implements OnInit {
    newAgentForm: FormGroup;
    isAgentProcessing: boolean;
    errorReceived: boolean;
    agent: IAgent;
    isAgentProcessingCompleteWithError: boolean;
    isAgentProcessingComplete: boolean;
    agentProcessingStatus = {
        agent: 0,
        mailbox: 0,
        typeform_rent: 0,
        spreadsheet_rent: 0,
        typeform_buy: 0,
        spreadsheet_buy: 0,
    };

    private aggregateId: string = Guid.newGuid();

    constructor(private agentService: AgentService,
        private agnetEvents: AgentWrapperService,
        private router: Router,
        formBuilder: FormBuilder) {

        this.agent = <IAgent>{};

        this.newAgentForm = formBuilder.group({
            'firstname': [this.agent.firstname, Validators.required],
            'lastname': [this.agent.lastname, Validators.required],
            'city': [this.agent.city, Validators.required],
            'country': [this.agent.country, Validators.required],
            'company': [this.agent.company, Validators.required],
            'phone': [this.agent.phone, Validators.required],
            'address': [this.agent.address, Validators.required],
            'email': [this.agent.email, Validators.required]
        });
    }

    ngOnInit() {
        this.agnetEvents.toggleHeaderMenu(false);
    }

    submitForm(value: any) {

        this.agent.firstname = this.newAgentForm.controls['firstname'].value;
        this.agent.lastname = this.newAgentForm.controls['lastname'].value;
        this.agent.city = this.newAgentForm.controls['city'].value;
        this.agent.country = this.newAgentForm.controls['country'].value;
        this.agent.company = this.newAgentForm.controls['company'].value;
        this.agent.phone = this.newAgentForm.controls['phone'].value;
        this.agent.address = this.newAgentForm.controls['address'].value;
        this.agent.email = this.newAgentForm.controls['email'].value;

        this.agent.aggregateId = this.aggregateId;

        this.agentProcessingStatus.agent = 1;

        this.agentService.createAgent(this.agent)
            .catch((errMessage) => {
                this.agentProcessingStatus.agent = 3;
                this.errorReceived = true;
                this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(res => {
                //this.returnToDetail();
                this.agentProcessingStatus.agent = 2;

                this.createMailbox();
                this.createTypeformBuy();
            });

        this.errorReceived = false;
        this.isAgentProcessing = true;
    }

    createMailbox() {
        this.agentProcessingStatus.mailbox = 1;

        this.agentService.createAgentIntegrationEmail(this.aggregateId)
            .catch((errMessage) => {
                this.agentProcessingStatus.mailbox = 3;
                //this.errorReceived = true;
                //this.isAgentProcessing = false;
                this.isAgentProcessingCompleteWithError = true;
                return Observable.throw(errMessage);
            })
            .subscribe(response => {
                this.agentProcessingStatus.mailbox = 2;
            });
    }

    createTypeformBuy() {
        this.agentProcessingStatus.typeform_buy = 1;

        this.agentService.createTypeformAccountForBuyInquiry(this.aggregateId)
            .catch((errMessage) => {
                this.agentProcessingStatus.typeform_buy = 3;
                
                //this.errorReceived = true;
                //this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(response => {
                this.agentProcessingStatus.typeform_buy = 2;
                this.createSpreadsheetBuy();
            });
    }

    createSpreadsheetBuy() {
        this.agentProcessingStatus.spreadsheet_buy = 1;

        this.agentService.createSpreadsheetForBuyInquiry(this.aggregateId)
            .catch((errMessage) => {
                this.agentProcessingStatus.spreadsheet_buy = 3;
                //this.errorReceived = true;
                //this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(response => {
                this.agentProcessingStatus.spreadsheet_buy = 2;
                //this.isAgentProcessing = false;
                //this.returnToDetail();
                //this.isAgentProcessingComplete = true;
                this.createTypeformRent();
            });
    }

    createTypeformRent() {
        this.agentProcessingStatus.typeform_rent = 1;

        this.agentService.createTypeformAccountForRentInquiry(this.aggregateId)
            .catch((errMessage) => {
                this.agentProcessingStatus.typeform_rent = 3;

                //this.errorReceived = true;
                //this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(response => {
                this.agentProcessingStatus.typeform_rent = 2;
                this.createSpreadsheetRent();
            });
    }

    createSpreadsheetRent() {
        this.agentProcessingStatus.spreadsheet_rent = 1;

        this.agentService.createSpreadsheetForRentInquiry(this.aggregateId)
            .catch((errMessage) => {
                this.agentProcessingStatus.spreadsheet_rent = 3;
                //this.errorReceived = true;
                //this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(response => {
                this.agentProcessingStatus.spreadsheet_rent = 2;
                this.isAgentProcessing = false;
                //this.returnToDetail();
                this.isAgentProcessingComplete = true;
            });
    }

    returnToDetail() {
        this.newAgentForm.reset();
        this.isAgentProcessing = false;
        this.agnetEvents.toggleHeaderMenu(true);
        this.router.navigate(['../list/view']);
    }
}
