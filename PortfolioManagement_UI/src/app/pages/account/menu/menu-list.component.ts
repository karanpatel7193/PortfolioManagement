import { Component, OnInit } from '@angular/core';
import { MenuService } from './menu.service';
import { MenuParameterModel, MenuGridModel, MenuModel } from './menu.model';
import { Router } from '@angular/router';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { ToastService } from 'src/app/services/toast.service';

@Component({
    selector: 'app-menu-list',
    templateUrl: './menu-list.component.html',
    styles: [`
    nb-card {
        transform: translate3d(0, 0, 0);
    }
    `],
})
export class MenuListComponent implements OnInit {
    public access: AccessModel = new AccessModel();
    public menuParameter: MenuParameterModel = new MenuParameterModel();
    public menuGrid: MenuGridModel = new MenuGridModel();
    public menus: MenuModel[] = [];
    public selectedMenu: string = "";

    constructor(private menuService: MenuService,
        private sessionService: SessionService,
        private router: Router,
        private toaster: ToastService) {
        this.setAccess();
    }

    ngOnInit() {
        this.setPageListMode();
    }

    public reset(): void {
        this.menuParameter = new MenuParameterModel();
        this.menuParameter.sortExpression = 'Id';
        this.menuParameter.sortDirection = 'asc';
    }

    public search(): void {
        if (!this.access.canView) {
			this.toaster.warning('You do not have view access of this page.');
            return;
        }

        this.menuService.getForGrid(this.menuParameter).subscribe(data => {
            this.menuGrid = data;
        });

    }

    public sort(sortExpression: string): void {
        if (sortExpression === this.menuParameter.sortExpression) {
            this.menuParameter.sortDirection = this.menuParameter.sortDirection === 'asc' ? 'desc' : 'asc';
        } else {
            this.menuParameter.sortExpression = sortExpression;
            this.menuParameter.sortDirection = 'asc';
        }
        this.search();
    }

    public add(): void {
        if (!this.access.canInsert) {
			this.toaster.warning('You do not have add access of this page.');
            return;
        }

        this.router.navigate(['app/account/menu/add']);
    }

    public edit(id: number): void {
        if (!this.access.canUpdate) {
			this.toaster.warning('You do not have edit access of this page.');
            return;
        }

        this.router.navigate(['app/account/menu/edit', id]);
    }

    public delete(id: number): void {
        if (!this.access.canDelete) {
			this.toaster.warning('You do not have delete access of this page.');
            return;
        }

        if (window.confirm('Are you sure you want to delete?')) {
            this.menuService.delete(id).subscribe(data => {
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

        this.menuParameter.sortExpression = 'Id';
        this.search();
    }

    public setAccess(): void {
        this.access = this.sessionService.getAccess('account/menu');
    }
}
