import {Component, OnInit, ViewChild} from '@angular/core';
import {DataSource} from '@angular/cdk/collections';
import {MatPaginator, MatSort} from '@angular/material';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import { Observable, throwError } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { IAgent } from '../agent.model';
import { AgentService } from '../agent.service';
import { AgentWrapperService } from '../agent.wrapper.service';
import { ConfigurationService } from '../../shared/services/configuration.service';
import { Guid } from '../../shared/models/guid';
import { CloudinaryOptions, CloudinaryUploader } from 'ng2-cloudinary';
/**
 * @title Table with pagination
 */
@Component({
  selector: 'agent-detail-view',
  styleUrls: [],
  templateUrl: 'detail.component.html',
})
export class AgentDetailViewComponent implements OnInit {

    newAgentForm: FormGroup;
    isAgentProcessing: boolean;
    errorReceived: boolean;
    agents: IAgent[];
    loading: any;
    logo: string;

    uploader: CloudinaryUploader = new CloudinaryUploader(
        new CloudinaryOptions({
            cloudName: "adfenixleads-com",
            uploadPreset: 'ezm7phkq'
        })
    );

    constructor(private agentService: AgentService,
        private agnetEvents: AgentWrapperService,
        private configurationService: ConfigurationService,
        private router: Router,
        private formBuilder: FormBuilder) {

        this.initAgentForm();
    }

    uploadLogo() {
        this.loading = true;

        this.uploader.uploadAll(); // call for uploading the data to Cloudinary

        /* Getting the success response from Cloudinary. */
        this.uploader.onSuccessItem = (item: any, response: string, status: number, headers: any): any => {
            let res = JSON.parse(response);
            this.loading = false;
            
            var currentAgent = this.agnetEvents.getCurrentAgent();
            this.logo = res.secure_url;

            this.agentService.saveLogo(currentAgent.id, res.secure_url)
                .catch((err) => this.handleError(err))
                .subscribe(agents => {
                    
                });
        }

        /* Getting the Error message Cloudinary throws. */
        this.uploader.onErrorItem = function (fileItem, response, status, headers) {
            console.info('onErrorItem', fileItem, response, status, headers)
        };
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
        var currentAgent = this.agnetEvents.getCurrentAgent();

        currentAgent.firstname = this.newAgentForm.controls['firstname'].value;
        currentAgent.lastname = this.newAgentForm.controls['lastname'].value;
        currentAgent.city = this.newAgentForm.controls['city'].value;
        currentAgent.country = this.newAgentForm.controls['country'].value;
        currentAgent.company = this.newAgentForm.controls['company'].value;
        currentAgent.phone = this.newAgentForm.controls['phone'].value;
        currentAgent.address = this.newAgentForm.controls['address'].value;
        currentAgent.email = this.newAgentForm.controls['email'].value;
        currentAgent.aggregateId = currentAgent.id;

        this.agentService.updateAgent(currentAgent)
            .catch((errMessage) => {
                this.errorReceived = true;
                this.isAgentProcessing = false;
                return Observable.throw(errMessage);
            })
            .subscribe(res => {
                
            });

        this.errorReceived = false;
        this.isAgentProcessing = true;
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

        this.logo = currentAgent.logo;
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
