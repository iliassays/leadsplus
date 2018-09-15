import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import {DataSource} from '@angular/cdk/collections';
import {MatPaginator, MatSort} from '@angular/material';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import {merge, Observable, of as observableOf} from 'rxjs';
import {catchError, map, startWith, switchMap} from 'rxjs/operators';

import { IAgent } from '../agent.model';
import { AgentService } from '../agent.service';

/**
 * @title Table with pagination
 */
@Component({
    selector: 'agent-zap-setup',
    styleUrls: [],
    templateUrl: 'zapsetup.component.html',
})
export class AgnetZapSetupComponent implements OnInit, AfterViewInit {

    zapierScript;

    constructor(private agentService: AgentService,
        private el: ElementRef) {
     
    }

    ngOnInit() {
    
    }

    ngAfterViewInit() {
        debugger;
        this.zapierScript = document.createElement('script');
        this.zapierScript.type = 'text/javascript';
        this.zapierScript.asyc = true;
        this.zapierScript.src =
            'https://zapier.com/apps/embed/widget.js?services=sendgrid&html_id=zap_integration'
        $(this.el.nativeElement).find('#zap_integration').append(this.zapierScript);
    }
}
