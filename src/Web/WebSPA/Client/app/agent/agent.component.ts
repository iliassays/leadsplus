import { Component, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { AgentWrapperService } from './agent.wrapper.service';

@Component({
  selector: 'agent',
  styles: [],
  templateUrl: 'agent.component.html',
  providers: []
})

export class AgentComponent implements OnInit { 
    showHeaderMenu = false;
    currentIndex = 1;
    totalCount = 0;

    constructor(private agnetEvents: AgentWrapperService) {

    }

    previousAgentClicked() {
        this.agnetEvents.previousAgentClicked();
    }

    nextAgentClicked() {
        this.agnetEvents.nextAgentClicked();
    }

    ngOnInit(): void {
        this.agnetEvents.headerMenuToggler$.subscribe(
            param => {
                this.showHeaderMenu = param;
            });

        this.agnetEvents.agentLoaded$.subscribe(
            agent => {
                this.totalCount = this.agnetEvents.totalAgent;
                this.currentIndex = 0;
            });

        this.agnetEvents.currentAgentChanged$.subscribe(
            agent => {
                this.currentIndex = this.agnetEvents.currentAgentIndex + 1;
            });
    }
}
