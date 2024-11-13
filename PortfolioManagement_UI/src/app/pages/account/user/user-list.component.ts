import { Component, OnInit } from '@angular/core';
import { UserService } from './user.service';
import { UserParameterModel, UserGridModel, UserModel, UserListModel } from './user.model';
import { Router } from '@angular/router';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
// import { ToasterService } from '../../../services/toaster.service';
import { RoleMainModel } from '../role/role.model';
import { ToastService } from 'src/app/services/toast.service';

@Component({
    selector: 'app-user-list',
    templateUrl: './user-list.component.html',
    styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class UserListComponent implements OnInit {
    public access: AccessModel = new AccessModel();
    public userParameter: UserParameterModel = new UserParameterModel();
    public userList: UserListModel = new UserListModel();
    public userGrid: UserGridModel = new UserGridModel();
    public roles: RoleMainModel[] = [];
    public selectedUser: string = '';

    constructor(private userService: UserService,
        private sessionService: SessionService,
        private router: Router,
        private toaster: ToastService) {
        this.setAccess();
    }

    ngOnInit() {
        this.setPageListMode();
    }

    public reset(): void {
        this.userParameter = new UserParameterModel();
        this.userParameter.sortExpression = 'Id';
        this.userParameter.sortDirection = 'asc';
        this.search();
    }

    public search(): void {
        if (!this.access.canView) {
            this.toaster.warning('You do not have view access of this page.');
            return;
        }

        this.userService.getForGrid(this.userParameter).subscribe(data => {
            this.userGrid = data;
        });
    }

    public sort(sortExpression: string): void {
        if (sortExpression === this.userParameter.sortExpression) {
            this.userParameter.sortDirection = this.userParameter.sortDirection === 'asc' ? 'desc' : 'asc';
        } else {
            this.userParameter.sortExpression = sortExpression;
            this.userParameter.sortDirection = 'asc';
        }
        this.search();
    }

    public add(): void {
        if (!this.access.canInsert) {
            this.toaster.warning('You do not have add access of this page.');
            return;
        }

        this.router.navigate(['app/account/user/add']);
    }

    public edit(id: number): void {
        if (!this.access.canUpdate) {
            this.toaster.warning('You do not have edit access of this page.');
            return;
        }

        this.router.navigate(['app/account/user/edit', id]);
    }

    public delete(id: number): void {
        if (!this.access.canDelete) {
            this.toaster.warning('You do not have delete access of this page.');
            return;
        }

        if (window.confirm('Are you sure you want to delete?')) {
            this.userService.delete(id).subscribe(data => {
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

        this.userParameter.sortExpression = 'Id';

        this.fillDropdown();
    }

    public fillDropdown(): void {
        this.userService.getListMode(this.userParameter).subscribe(data => {
            this.userList = data;
            this.userGrid.users = this.userList.users;
            this.userGrid.totalRecords = this.userList.totalRecords;
            this.roles = this.userList.roles;
        });
    }

    public setAccess(): void {
        this.access = this.sessionService.getAccess('account/user');
        // this.access.canInsert = true;
        // this.access.canUpdate = true;
        //this.access.canDelete = true;
        // this.access.canView = true;
    }
}
