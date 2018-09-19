import { Component, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { ContactWrapperService } from './contact.wrapper.service';

@Component({
  selector: 'contact',
  styles: [],
  templateUrl: 'contact.component.html',
  providers: []
})

export class ContactComponent implements OnInit { 
    showHeaderMenu = false;
    currentIndex = 1;
    totalCount = 0;

    constructor(private agnetEvents: ContactWrapperService) {

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
