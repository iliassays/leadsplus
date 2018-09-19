import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { IContact } from './contact.model';
import { from } from 'rxjs/observable/from';
import { filter } from 'rxjs/operators';
import { Guid } from '../shared/models/guid';
import { Number } from 'core-js';

@Injectable()
export class ContactWrapperService {
    
    constructor() {
        
    }

    private currentAgent = <IContact>{};
    private agents: IContact[];

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

    private currentAgentChangedSource = new Subject<IContact>();
    currentAgentChanged$ = this.currentAgentChangedSource.asObservable();

    private agentLoadedSource = new Subject<IContact[]>();
    agentLoaded$ = this.agentLoadedSource.asObservable();

    setAgents(agents) {
        this.agents = agents;
        this.totalAgent = agents.length;

        this.agentLoadedSource.next();
    }

    getAgents() {
        return this.agents;
    }

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
        if (this.currentAgentIndex < this.totalAgent - 1) {
            this.currentAgentIndex++;

            this.setCurrentAgent(this.agents[this.currentAgentIndex]);
            this.nextAgentClickedSource.next(this.currentAgentIndex);
        }
        
    }

    previousAgentClicked() {
        if (this.currentAgentIndex != 0) {
            this.currentAgentIndex--;
            this.setCurrentAgent(this.agents[this.currentAgentIndex]);
            this.previousAgentClickedSource.next(this.currentAgentIndex);
        }
    }

    toggleHeaderMenu(show) {
        this.headerMenuTogglerSource.next(show);
    }
}
