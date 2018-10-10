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
  selector: 'agent-social-setup',
  styleUrls: [],
    templateUrl: 'socialmediasetup.component.html',
})
export class AgnetSocialMediaSetupComponent implements OnInit {

    currentAgent = <IAgent>{

    };
    integrationEmail;
    isAgentProcessing: boolean;
    errorReceived: boolean;

    constructor(private agentService: AgentService,
        private agnetEvents: AgentWrapperService,
        private formBuilder: FormBuilder,
        private el: ElementRef) {

    }

    public saveSocialMedia() {
        this.agentService.updateAgentSocialMedia(this.currentAgent.id,
            this.currentAgent.facebook,
            this.currentAgent.instagram,
            this.currentAgent.twitter,
            this.currentAgent.linkedIn)
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
