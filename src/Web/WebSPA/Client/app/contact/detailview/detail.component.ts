import {Component, OnInit, ViewChild} from '@angular/core';
import {DataSource} from '@angular/cdk/collections';
import {MatPaginator, MatSort} from '@angular/material';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import { Observable, throwError } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { IContact } from '../contact.model';
import { ContactService } from '../contact.service';
import { ContactWrapperService } from '../contact.wrapper.service';
import { ConfigurationService } from '../../shared/services/configuration.service';
import { Guid } from '../../shared/models/guid';

/**
 * @title Table with pagination
 */
@Component({
  selector: 'contact-detail-view',
  styleUrls: [],
  templateUrl: 'detail.component.html',
})
export class ContactDetailViewComponent implements OnInit {

    newAgentForm: FormGroup;
    isAgentProcessing: boolean;
    errorReceived: boolean;
    agents: IContact[];

    constructor(private agentService: ContactService,
        private agnetEvents: ContactWrapperService,
        private configurationService: ConfigurationService,
        private router: Router,
        private formBuilder: FormBuilder) {

        this.initAgentForm();
    }

    getAgents(pageSize: number, pageIndex: number) {
        this.errorReceived = false;
        this.agentService.getAgents(pageIndex, pageSize)
            .catch((err) => this.handleError(err))
            .subscribe(agents => {
                this.agnetEvents.setAgents(agents);
                this.agents = agents;
                this.agnetEvents.setCurrentAgent(this.agents[0]);
                
                this.initAgentForm();
            });
    }

    submitForm(value: any) {
        
    }

    private handleError(error: any) {
        this.errorReceived = true;
        return Observable.throw(error);
    }  

    private initAgentForm() {
        var currentAgent = this.agnetEvents.getCurrentAgent();

        this.newAgentForm = this.formBuilder.group({
            'firstname': [currentAgent.firstname, Validators.required],
            'lastname': [currentAgent.lastname, Validators.required],
            'city': [currentAgent.city, Validators.required],
            'country': [currentAgent.country, Validators.required],
            'company': [currentAgent.company, Validators.required],
            'phone': [currentAgent.phone, Validators.required],
            'address': [currentAgent.address, Validators.required],
            'email': [currentAgent.email, Validators.required]
        });
    }

  ngOnInit() {
      this.agnetEvents.toggleHeaderMenu(true);
      this.agnetEvents.currentAgentIndex = 0;

      if (this.configurationService.isReady) {
          this.getAgents(100, 0)
      } else {
          this.configurationService.settingsLoaded$.subscribe(x => {
              this.getAgents(100, 0)
          });
      }

      this.agnetEvents.currentAgentChanged$.subscribe(
          agent => {
              this.initAgentForm();
          });
    }
}
