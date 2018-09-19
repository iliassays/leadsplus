import { Injectable } from '@angular/core';
import { Router, CanActivate, CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { SecurityService } from './security.service';

@Injectable({ providedIn: 'root' })
export class AuthenticationGuard implements CanActivate, CanActivateChild {
    subscription: Subscription;

    constructor(private router: Router,
        private identityService: SecurityService)
    {
    }

    canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

      
        if (this.identityService.IsAuthorized) {
            return true;
        }
        else {
            this.identityService.Authorize();
        }

        return false;
    }

    canActivateChild() {
        console.log('checking child route access');
        return true;
    }
}