import { Routes, RouterModule } from '@angular/router';

import { ContactComponent } from './contact.component';

//import { AgentFilterComponent } from './filter/filter.component';
import { ContactDetailViewComponent } from './detailview/detail.component';

export const ContactRoutes: Routes = [
  {
    path: '',
    component: ContactComponent,
    children: [
      { path: '', redirectTo: '/app/dashboard', pathMatch: 'full' },
      //{ path: 'filter', component: AgentFilterComponent  },
      { path: 'detail', component: ContactDetailViewComponent },
    ]
  }
];

export const ContactRoutingModule = RouterModule.forChild(ContactRoutes);
