import {Component, OnInit, ViewChild} from '@angular/core';
import {DataSource} from '@angular/cdk/collections';
import {MatPaginator, MatSort} from '@angular/material';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import {merge, Observable, of as observableOf} from 'rxjs';
import {catchError, map, startWith, switchMap} from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IAgent} from '../agent.model';
import { AgentService } from '../agent.service';
import { AgentWrapperService } from '../agent.wrapper.service';

/**
 * @title Table with pagination
 */
@Component({
  selector: 'agent-integration-setup',
  styleUrls: [],
  templateUrl: 'integrationsetup.component.html',
})
export class AgnetIntegrationSetupComponent implements OnInit {
    currentAgent = <IAgent>{};
    integrationEmail;
    isAgentProcessing: boolean;
    errorReceived: boolean;
    integrationForm: FormGroup;

    constructor(private agentService: AgentService,
        private agnetEvents: AgentWrapperService,
        private formBuilder: FormBuilder) {
        this.initAgentForm();
    }

    generateIntegrationEmailClicked() {
        
    }

    submitForm(value: any) {
        var currentAgent = this.agnetEvents.getCurrentAgent();

        this.integrationEmail = this.integrationForm.controls['integrationEmail'].value;

        this.agentService.updateIntegrationEmailAgent(this.currentAgent, this.integrationEmail)
            .catch((errMessage) => {
                this.errorReceived = true;
                this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(response => {

                this.agentService.getAgent(this.currentAgent.id)
                    .catch((errMessage) => {
                        this.errorReceived = true;
                        this.isAgentProcessing = false;
                        return Observable.throw(errMessage);
                    })
                    .subscribe(agent => {
                        this.currentAgent.integrationEmail = agent.integrationEmail;
                        this.initAgentForm();
                    });
            });

        this.errorReceived = false;
        this.isAgentProcessing = true;
    }

    private initAgentForm() {
        var currentAgent = this.agnetEvents.getCurrentAgent();

        this.integrationEmail = currentAgent && currentAgent.integrationEmail ? currentAgent.integrationEmail : '';
        

        this.integrationForm = this.formBuilder.group({
            'integrationEmail': [this.integrationEmail.split('@')[0], Validators.required]
        });
    }

    ngOnInit() {
      this.agnetEvents.currentAgentChanged$.subscribe(
          agent => {
              this.currentAgent = agent;
              this.integrationEmail = agent.integrationEmail;
              this.initAgentForm();
          });
    } 
}
