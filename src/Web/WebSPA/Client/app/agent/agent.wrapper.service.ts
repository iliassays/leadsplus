import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { IAgent } from './agent.model';
import { from } from 'rxjs/observable/from';
import { filter } from 'rxjs/operators';
import { Guid } from '../shared/models/guid';
import { Number } from 'core-js';

@Injectable()
export class AgentWrapperService {
    
    constructor() {
        
    }

    private currentAgent = <IAgent>{};

    totalAgent = 0;
    currentAgentIndex = 0;

    private headerMenuTogglerSource = new Subject<boolean>();
    headerMenuToggler$ = this.headerMenuTogglerSource.asObservable();

    private agentCreatedSource = new Subject();
    agentCreated$ = this.agentCreatedSource.asObservable();

    private agentUpdatedSource = new Subject();
    agentUpdated$ = this.agentUpdatedSource.asObservable();

    private previousAgentClickedSource = new Subject<number>();
    previousAgentClicked$ = this.previousAgentClickedSource.asObservable();

    private nextAgentClickedSource = new Subject<number>();
    nextAgentClicked$ = this.nextAgentClickedSource.asObservable();

    private currentAgentChangedSource = new Subject<IAgent>();
    currentAgentChanged$ = this.currentAgentChangedSource.asObservable();

    setCurrentAgent(agent) {
        this.currentAgent = agent;
        this.currentAgentChangedSource.next(agent);
    }

    getCurrentAgent() {
        return this.currentAgent;
    }

    agentCreated() {
        this.agentCreatedSource.next();
    }

    agentUpdated() {
        this.agentUpdatedSource.next();
    }

    nextAgentClicked() {
        if (this.currentAgentIndex < this.totalAgent) {
            this.currentAgentIndex++;
            this.nextAgentClickedSource.next(this.currentAgentIndex);
        }
        
    }

    previousAgentClicked() {
        if (this.currentAgentIndex != 0) {
            this.currentAgentIndex--;
            this.previousAgentClickedSource.next(this.currentAgentIndex);
        }
    }

    toggleHeaderMenu(show) {
        this.headerMenuTogglerSource.next(show);
    }
}
