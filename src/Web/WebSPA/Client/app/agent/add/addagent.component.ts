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

        this.agentService.createAgent(this.agent)
            .catch((errMessage) => {
                this.errorReceived = true;
                this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(res => {
                this.returnToDetail();
            });

        this.errorReceived = false;
        this.isAgentProcessing = true;
    }

    returnToDetail() {
        this.newAgentForm.reset();
        this.isAgentProcessing = false;
        this.agnetEvents.toggleHeaderMenu(true);
        this.router.navigate(['detail']);
    }
}
