import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { SessionService } from '../services/session.service';

@Injectable({ providedIn: 'root' })
export class ApiInterceptor implements HttpInterceptor {
    apiUrl: string = environment.apiUrl;
    downloadUrl: string = environment.downloadUrl;
    constructor(private sessionService: SessionService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization header with basic auth credentials if available
        const currentUser = this.sessionService.getUser();
        if (currentUser && currentUser.token) {
            const contentType = request.headers.has('Content-Type') ? request.headers.get('Content-Type') : 'application/json';
            request = request.clone({
                url: request.url.indexOf('.json') > -1 ? request.url : this.apiUrl + request.url,
                setHeaders: {
                    'Oauth': `Bearer ${currentUser.token}`,
                    'Content-Type': contentType || [],
                },
            });
        } 
        else {
            request = request.clone({
                url: this.apiUrl + request.url,
                setHeaders: {
                    'Content-Type': 'application/json',
                },
            });
        }
        return next.handle(request);
    }
}
