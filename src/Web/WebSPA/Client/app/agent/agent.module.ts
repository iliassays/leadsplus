import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { AgentRoutingModule } from './agent-routing.module';
import { AgentComponent } from './agent.component';

import { AgentService } from './agent.service';
import { AgnetAddComponent } from './add/addagent.component';
import { AgentListViewComponent } from './listview/list.component';
import { AgentDetailViewComponent } from './detailview/detail.component';
import { AgnetIntegrationSetupComponent } from './detailview/integrationsetup.component';
import { AgnetZapSetupComponent } from './detailview/zapsetup.component';
import { AgnetGoogleDataStudioSetupComponent } from './detailview/googledatastudiosetup.component';
import { AgnetTypeformSetupComponent } from './detailview/typeformsetup.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {CdkTableModule} from '@angular/cdk/table';
import { AgentWrapperService } from './agent.wrapper.service';

import {
  MatAutocompleteModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatStepperModule,

} from '@angular/material';


@NgModule({
  imports: [
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    MatStepperModule,
    CdkTableModule,
    CommonModule,
    AgentRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  declarations: [    
    AgentComponent,
    AgentDetailViewComponent,
    AgnetIntegrationSetupComponent,
    AgnetZapSetupComponent,
    AgnetGoogleDataStudioSetupComponent,
    AgnetTypeformSetupComponent,
    AgnetAddComponent,
    AgentListViewComponent
    ],
    providers: [
        AgentService,
        AgentWrapperService
    ]
})

export class AgentModule { }
