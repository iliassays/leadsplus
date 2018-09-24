import { RouterModule, Routes } from '@angular/router';

import { LayoutComponent } from './layout.component';
import { DashboardComponent } from '../dashboard/dashboard.component';

const routes: Routes = [
  {
    path: 'app',
    component: LayoutComponent,
    children: [
        { path: '', redirectTo: '/app/dashboard', pathMatch: 'full' },
        { path: 'dashboard', component: DashboardComponent },
        { path: 'agent', loadChildren: '../agent/agent.module#AgentModule' },
        { path: 'contact', loadChildren: '../contact/contact.module#ContactModule' },
        { path: 'inquiryhistory', loadChildren: '../inquiryhistory/inquiryhistory.module#InquiryHistoryModule' },
    ]
  }
];

export const LayoutRoutingModule = RouterModule.forChild(routes);
