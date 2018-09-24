import { Routes, RouterModule } from '@angular/router';

import { InquiryHistoryComponent } from './inquiryhistory.component';

//import { AgentFilterComponent } from './filter/filter.component';
import { InquiryHistoryListComponent } from './list/list.component';

export const InquiryHistoryRoutes: Routes = [
  {
    path: '',
        component:  InquiryHistoryComponent,
    children: [
      { path: '', redirectTo: '/app/dashboard', pathMatch: 'full' },
      //{ path: 'filter', component: AgentFilterComponent  },
        { path: 'list', component:  InquiryHistoryListComponent },
    ]
  }
];

export const InquiryHistoryRoutingModule = RouterModule.forChild(InquiryHistoryRoutes);
