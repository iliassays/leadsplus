import { Component, OnInit } from '@angular/core';
import { APPCONFIG } from '../../config';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

import { SecurityService } from '../../shared/services/security.service';

@Component({
  selector: 'my-app-header',
  styles: [],
  templateUrl: './header.component.html'
})

export class AppHeaderComponent implements OnInit {
  public AppConfig: any;
  authenticated: boolean = false;
  private subscription: Subscription;
  private userName: string = '';
    
  constructor(private auth: SecurityService, private router: Router) { }

  ngOnInit() {
      this.AppConfig = APPCONFIG;

      this.subscription = this.auth.authenticationChallenge$.subscribe(res => {
          this.authenticated = res;
          this.userName = this.auth.UserData.email;
      });

      console.log('identity component, checking authorized' + this.auth.IsAuthorized);
      this.authenticated = this.auth.IsAuthorized;

      if (this.authenticated) {
          if (this.auth.UserData)
              this.userName = this.auth.UserData.email;
      }
  }

  logout() {
     this.auth.Logoff();
  }
}
