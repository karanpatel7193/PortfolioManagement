import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { AccessModel } from '../models/access.model';
import { UserLoginModel} from '../pages/account/user/user.model';

@Injectable({ providedIn: 'root' })
export class SessionService {
	private userSubject: BehaviorSubject<UserLoginModel>;
	private isAuthenticatedSubject: BehaviorSubject<boolean>;
	private isMenuCollapsedSubject: BehaviorSubject<boolean>;

	public userState: Observable<UserLoginModel>;
	public menuCollapsedState: Observable<boolean>;

	constructor(private router: Router) {
		let user = localStorage.getItem('user');
		let authenticated = localStorage.getItem('authenticated');
		let isMenuCollapsed = localStorage.getItem('isMenuCollapsed');

		this.userSubject = new BehaviorSubject<UserLoginModel>(JSON.parse(user || '{}'));
		this.isAuthenticatedSubject = new BehaviorSubject<boolean>(Boolean(authenticated || false));
		this.isMenuCollapsedSubject = new BehaviorSubject<boolean>(Boolean(isMenuCollapsed || false));

		this.userState = this.userSubject.asObservable();
		this.menuCollapsedState = this.isMenuCollapsedSubject.asObservable();
	}

	public get isAuthenticated(): boolean {
		return this.isAuthenticatedSubject.value;
	}

	public setIsAuthenticated(value: boolean) {
		localStorage.setItem('authenticated', String(value));
		this.isAuthenticatedSubject.next(value);
	}

	public get isMenuCollapsedValue(): boolean {
		return this.isMenuCollapsedSubject.value;
	}
	public setIsMenuCollapsed(value: boolean) {
		localStorage.setItem('isMenuCollapsed', String(value));
		this.isMenuCollapsedSubject.next(value);
	}

	public getUser(): UserLoginModel {
		return this.userSubject.value;
    }

    public setUser(user: UserLoginModel): void {
        localStorage.setItem("user", JSON.stringify(user));
		this.userSubject.next(user);
    }

    public destroy(): void {
        localStorage.clear();
    };

	//#endregion Set methods

	public getAccess(CurrentUrl: string): AccessModel {
		let Access = new AccessModel();
		Access.canView = true;
		Access.canInsert = true;
		Access.canUpdate = true;
		Access.canDelete = true;

		let user = this.getUser();
		let currentMenu = user.menus.filter(x => x.routing.startsWith(CurrentUrl));

		if (currentMenu != null && currentMenu.length > 0) {
			let currentMenuId = currentMenu[0].id;
			let menuAccess = user.roleMenuAccesss.filter(x => x.menuId == currentMenuId);

			if (menuAccess != null && menuAccess.length > 0) {
				Access.canView = menuAccess[0].canView;
				Access.canInsert = menuAccess[0].canInsert;
				Access.canUpdate = menuAccess[0].canUpdate;
				Access.canDelete = menuAccess[0].canDelete;
			}
		}
		return Access;
	}

	logout() {
		// remove user from local storage to log user out
		localStorage.removeItem('user');
		localStorage.removeItem('authenticated');
		this.userSubject.next(null!);
		this.setIsAuthenticated(false);
		this.router.navigate(['/auth/login']);
	}
}
