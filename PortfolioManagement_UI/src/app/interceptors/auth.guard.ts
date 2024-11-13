import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

import { SessionService } from '../services/session.service';
import { CommonPages } from '../data-constant';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  constructor(private sessionService: SessionService, private router:Router) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return true;
    const currentUser = this.sessionService.getUser();
    const isAuthenticated = this.sessionService.isAuthenticated;
    //const currentURLFragment = state.url.split('/');

    // let isExist = false;
    // isExist = currentUser.menus.filter(x => currentURLFragment.indexOf(x.name)).length > 0;

    // CommonPages.forEach(tab => {
    //     if(currentURLFragment.find(x => x.toLowerCase() == (tab.pageName.replace(' ', '-')).toLowerCase())) {
    //         isExist = true;
    //     }
    // });
    
    if (isAuthenticated && currentUser != null && currentUser.token != null){
        if(currentUser.menus.length == 0) {
            return true;
        } else {
            this.router.navigate(["/app/dashboard"]);
            return false;
        }
    } else {
        this.router.navigate(["/auth/login"]);
        return false;
    }
  }

}