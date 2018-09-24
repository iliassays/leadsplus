import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { InquiryHistoryRoutingModule } from './inquiryhistory-routing.module';
import { InquiryHistoryComponent } from './inquiryhistory.component';

import { InquiryHistoryService } from './inquiryhistory.service';
import { InquiryHistoryListComponent } from './list/list.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {CdkTableModule} from '@angular/cdk/table';

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
    InquiryHistoryRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  declarations: [    
    InquiryHistoryComponent,
    InquiryHistoryListComponent,
    ],
    providers: [
        InquiryHistoryService
    ]
})

export class InquiryHistoryModule { }
