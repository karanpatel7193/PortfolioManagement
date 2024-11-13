import { Component, OnInit, OnDestroy } from '@angular/core';
import { RoleService } from '../role/role.service';
import { RoleParameterModel, RoleGridModel, RoleModel } from '../role/role.model';
import { Router, ActivatedRoute, UrlSegment } from '@angular/router';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { RoleMenuAccessModel } from './rolemenuaccess.model';
import { MenuMainModel } from '../menu/menu.model';
import { RoleAccessService } from './role-acess.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
    selector: 'role-access',
    templateUrl: './role-access.component.html',
    styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})

export class RoleAccessComponent implements OnInit, OnDestroy {
    public access: AccessModel = new AccessModel();
    public role: RoleModel = new RoleModel();
    public roleAccesses: RoleMenuAccessModel[] = [];
    public menus: MenuMainModel[] = [];
    public menuId: number=0;
    private sub: any;

    constructor(private roleService: RoleService,
        private roleAccessService: RoleAccessService,
        private sessionService: SessionService,
        private router: Router,
        private route: ActivatedRoute,
        private toaster: ToastService) {
        this.setAccess();
    }

    ngOnInit() {
        this.setPageListMode();
        this.getRouteData();
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    public getRouteData(): void {
        this.sub = this.route.params.subscribe(params => {
            this.role.id = +params['roleId']; // (+) converts string 'id' to a number
            this.role.name = params['roleName'];
        });
    }

    public fillRoleAccss(): void {
        if (!this.access.canView) {
            this.toaster.warning('You do not have view access of this page.');
            return;
        }
        let roleMenu: RoleMenuAccessModel = new RoleMenuAccessModel();
        roleMenu.roleId = this.role.id;
        roleMenu.menuId = this.menuId;
        this.roleAccessService.getRoleAccessById(roleMenu).subscribe(data => {
            this.role.roleMenuAccesss = data;
        });

    }

    public save(): void {
        if (!this.access.canInsert) {
            this.toaster.warning('You do not have add access of this page.');
            return;
        }
        this.roleAccessService.saveRoleAccess(this.role).subscribe(data => {
            this.toaster.success('Record saved successfully.');
        })
    }

    public setPageListMode(): void {

        if (!this.access.canView) {
            this.toaster.warning('You do not have view access of this page.');
            return;
        }
        this.fillParent();
    }

    public fillParent(): void {
        this.roleService.fillParent().subscribe(data => {
            this.menus = data;

            if (this.menus.length > 0) {
                this.menuId = this.menus[0].id;
                this.fillRoleAccss();
            }
        });
    }

    public setAccess(): void {
        this.access = this.sessionService.getAccess('account/rolemenuaccess');
    }

    public cancle():void{
        this.router.navigate(['app/account/role/list'])
    }

}
