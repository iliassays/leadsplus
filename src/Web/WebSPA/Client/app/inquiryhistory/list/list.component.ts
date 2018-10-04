import { Component, OnInit, ViewChild } from '@angular/core';
import { DataSource } from '@angular/cdk/collections';
//import { MatPaginator, MatSort } from '@angular/material';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';

import { IInquiryHistory } from '../inquiryhistory.model';
import { InquiryHistoryService } from '../inquiryhistory.service';

/**
 * @title Table with pagination
 */
@Component({
    selector: 'inquiryhistory-list-view',
    styleUrls: [],
    templateUrl: 'list.component.html',
})
export class InquiryHistoryListComponent implements OnInit {

    displayedColumns: string[] = ['OrganizationDomain', 'CustomerEmail', 'AgentEmail', 'InquiryType', 'InquiryStatus', 'CreatedAt'];
    //exampleDatabase: ExampleHttpDao | null;
    data: IInquiryHistory[] = [];

    resultsLength = 0;
    isLoadingResults = true;
    isRateLimitReached = false;

    //@ViewChild(MatPaginator) paginator: MatPaginator;
    //@ViewChild(MatSort) sort: MatSort;

    constructor(private inquiryHistoryService: InquiryHistoryService) {

    }

    ngOnInit() {
        //this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

        merge()
            .pipe(
                startWith({}),
                switchMap(() => {
                    this.isLoadingResults = true;
                    return this.inquiryHistoryService!.getInquiryHistories(
                        "", "", 0, 100);
                }),
                map(data => {
                    // Flip flag to show that loading has finished.
                    this.isLoadingResults = false;
                    this.isRateLimitReached = false;
                    this.resultsLength = data.length;

                    return data;
                }),
                catchError(() => {
                    this.isLoadingResults = false;
                    // Catch if the GitHub API has reached its rate limit. Return empty data.
                    this.isRateLimitReached = true;
                    return observableOf([]);
                })
            ).subscribe(data => this.data = data);
    }
}
