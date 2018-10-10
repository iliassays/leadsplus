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
  selector: 'agent-action',
  styleUrls: [],
    templateUrl: 'actionsetup.component.html',
})
export class AgnetActionSetupComponent implements OnInit {

    currentAgent = <IAgent>{

    };

    isAgentProcessing: boolean;
    errorReceived: boolean;

    constructor(private agentService: AgentService,
        private agnetEvents: AgentWrapperService,
        private formBuilder: FormBuilder,
        private el: ElementRef) {

    }

    public markAsLaunched() {
        this.agentService.makedAsLaunched(this.currentAgent.id)
            .catch((errMessage) => {
                this.errorReceived = true;
                this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(response => {
                this.currentAgent.isLaunched = true;
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
