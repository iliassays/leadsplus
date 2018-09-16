import * as jQuery from 'jquery';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer, Title } from '@angular/platform-browser';
import { Subscription }   from 'rxjs/Subscription';
import { Router, NavigationEnd } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { APPCONFIG } from './config';
import { LayoutService } from './layout/layout.service';
import { SecurityService } from './shared/services/security.service';
import { ConfigurationService } from './shared/services/configuration.service';

// 3rd
import 'styles/material2-theme.scss';
import 'styles/bootstrap.scss';
// custom
import 'styles/layout.scss';
import 'styles/theme.scss';
import 'styles/ui.scss';
import 'styles/app.scss';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [LayoutService],
})
export class AppComponent implements OnInit {
  public AppConfig: any;
  Authenticated: boolean = false;
  subscription: Subscription;

  constructor(private router: Router,
    private titleService: Title,
    private identityService: SecurityService,
    private configurationService: ConfigurationService,
    public http: HttpClient) { 
      this.Authenticated = this.identityService.IsAuthorized;
    }

  ngOnInit() {
    
        this.AppConfig = APPCONFIG;
        this.subscription = this.identityService.authenticationChallenge$.subscribe(res => this.Authenticated = res);

        // Scroll to top on route change
        this.router.events.subscribe((evt) => {
          if (!(evt instanceof NavigationEnd)) {
            return;
          }
          window.scrollTo(0, 0);
        });
    
      this.configurationService.load();

      if (window.location.href.indexOf("id_token") > -1) {
            this.identityService.AuthorizedCallback();
      }

        //this.configurationService.settingsLoaded$.subscribe(x => {
        //    this.identityService.Authorize();
        //});
    }

      ping() {
    
      }
}
