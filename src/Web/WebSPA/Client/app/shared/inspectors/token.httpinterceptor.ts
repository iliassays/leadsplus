import { Observable } from 'rxjs/Observable';
import { Location } from '@angular/common';
import { Injectable } from '@angular/core';
import { HttpInterceptor } from '@angular/common/http';
import { HttpRequest } from '@angular/common/http';
import { HttpHandler } from '@angular/common/http';
import { HttpEvent } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import 'rxjs/add/observable/fromPromise';
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class TokenHttpInterceptor implements HttpInterceptor {
  constructor(private cookieService: CookieService ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
  
    request = request.clone({
      withCredentials: true,
    });
    return next.handle(request);
  }

}
