import { Component, OnInit } from '@angular/core';
import { RoleService } from './role.service';
import { RoleParameterModel, RoleGridModel, RoleModel } from './role.model';
import { Router, ActivatedRoute, UrlSegment } from '@angular/router';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { ToastService } from 'src/app/services/toast.service';

@Component({
    selector: 'role-list',
    templateUrl: './role-list.component.html',
    styles: [`
    nb-card {
    transform: translate3d(0, 0, 0);
    }
    `],
})

export class RoleListComponent implements OnInit {
    public access: AccessModel = new AccessModel();
    public roleParameter: RoleParameterModel = new RoleParameterModel();
    public roleGrid: RoleGridModel = new RoleGridModel();
    public roles: RoleModel[] = [];
    public selectedRole: string = '';

    constructor(private roleService: RoleService,
        private sessionService: SessionService,
        private router: Router,
        private toaster: ToastService) {
        this.setAccess();
    }

    ngOnInit() {
        this.setPageListMode();
    }

    public reset(): void {
        this.roleParameter = new RoleParameterModel();
        this.roleParameter.sortExpression = 'Id';
        this.roleParameter.sortDirection = 'asc';
    }

    public search(): void {
        if (!this.access.canView) {
            this.toaster.warning('You do not have view access of this page.');
            return;
        }

        this.roleService.getForGrid(this.roleParameter).subscribe(data => {
            this.roleGrid = data;
        });

    }

    public sort(sortExpression: string): void {
        if (sortExpression === this.roleParameter.sortExpression) {
            this.roleParameter.sortDirection = this.roleParameter.sortDirection === 'asc' ? 'desc' : 'asc';
        } else {
            this.roleParameter.sortExpression = sortExpression;
            this.roleParameter.sortDirection = 'asc';
        }
        this.search();
    }

    public add(): void {
        if (!this.access.canInsert) {
            this.toaster.warning('You do not have add access of this page.');
            return;
        }

        this.router.navigate(['app/account/role/add']);
    }

    public edit(id: number): void {
        if (!this.access.canUpdate) {
            this.toaster.warning('You do not have edit access of this page.');
            return;
        }

        this.router.navigate(['app/account/role/edit', id]);
    }

    public roleAccess(item: any): void {
        if (!this.access.canUpdate) {
            this.toaster.warning('You do not have edit access of this page.');
            return;
        }

        this.router.navigate(['app/account/role/access',item.id,item.name]);
    }

    public delete(id: number): void {
        if (!this.access.canDelete) {
            this.toaster.warning('You do not have delete access of this page.');
            return;
        }

        if (window.confirm('Are you sure you want to delete?')) {
            this.roleService.delete(id).subscribe(data => {
                this.toaster.success('Record deleted successfully.');
                this.search();
            });
        }
    }

    public setPageListMode(): void {

        if (!this.access.canView) {
            this.toaster.warning('You do not have view access of this page.');
            return;
        }

        this.roleParameter.sortExpression = 'Id';
        this.search();
    }

    public setAccess(): void {
        this.access = this.sessionService.getAccess('account/role');
    }
}
