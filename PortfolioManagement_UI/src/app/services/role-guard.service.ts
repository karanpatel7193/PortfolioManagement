import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth.service';
import { SessionService } from './session.service';

import { enumAccessUrl } from '../app.enum';
import { ToastService } from './toast.service';

@Injectable()
export class RoleGuardService implements CanActivate {
    constructor(public auth: AuthService, public router: Router, public session: SessionService, public toastService: ToastService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {

        // this will be passed from the route config on the data property
        const expectedAccess: string = route.data.expectedAccess;
        const menuUrl: string = state.url.substring(0, state.url.indexOf(enumAccessUrl[expectedAccess]));

        if (this.auth.IsAuthenticated() && this.session.GetAccess(menuUrl)[expectedAccess]) {
            return true;
        } else {
            this.toastService.warning('You do not have access. Please contact admin.', 'Sorry! Access Denied')
            this.router.navigate(['admin/dashboard']);
            return false;
        }
    }
}
