import {Component, OnInit, ViewChild} from '@angular/core';
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
  selector: 'agent-google-datastudio-setup',
  styleUrls: [],
  templateUrl: 'googledatastudiosetup.component.html',
})
export class AgnetGoogleDataStudioSetupComponent implements OnInit {

  

  constructor(private agentService: AgentService) {
     
  }

  ngOnInit() {
    
  }
}
