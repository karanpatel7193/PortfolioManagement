import { Component, OnInit } from '@angular/core';
import { AccountGridModel, AccountListModel, AccountParameterModel } from './account.model';
import { AccessModel } from 'src/app/models/access.model';
import { AccountService } from './account.service';
import { SessionService } from 'src/app/services/session.service';
import { Router } from '@angular/router';
import { ToastService } from 'src/app/services/toast.service';

@Component({
selector: 'app-account-list',
templateUrl: './account-list.component.html',
styleUrls: []
})

export class AccountListComponent implements OnInit {
public get AccountService(): AccountService {
    return this._AccountService;
}
public set AccountService(value: AccountService) {
    this._AccountService = value;
}
	public access: AccessModel = new AccessModel();
	public accountParameter: AccountParameterModel = new AccountParameterModel();
	public accountGrid: AccountGridModel = new AccountGridModel();
	public accountList: AccountListModel = new AccountListModel();


	constructor(private _AccountService: AccountService,
		private sessionService: SessionService,
		private router: Router,
		private toastr: ToastService) 
	{
		this.setAccess();
	}

	ngOnInit() {
		this.setPageListMode();
	}

	public reset(): void {
		this.accountParameter = new AccountParameterModel();
		this.accountParameter.sortExpression = 'Id';
		this.accountParameter.sortDirection = 'asc';
		this.search();
	}

	public search(): void {
		if (!this.access.canView) {
			this.toastr.warning('You do not have view access of this page.');
			return;
		}

		this.AccountService.getForGrid(this.accountParameter).subscribe(data => {
			this.accountGrid = data;
		});
	}

	public sort(sortExpression: string): void {
		if (sortExpression === this.accountParameter.sortExpression) {
			this.accountParameter.sortDirection = this.accountParameter.sortDirection === 'asc' ? 'desc' : 'asc';
		}
		else {
			this.accountParameter.sortExpression = sortExpression;
			this.accountParameter.sortDirection = 'asc';
		}
		this.search();
	}
	

	public add(): void {
		if (!this.access.canInsert) {
			this.toastr.warning('You do not have add access of this page.');
			return;
		}
		//this.stateParam.push('accountParameter', this.accountParameter);
		this.router.navigate(['app/master/account/add']);
	}

	public edit(id: number): void {
		if (!this.access.canUpdate) {
			this.toastr.warning('You do not have edit access of this page.');
			return;
		}

		//this.stateParam.push('accountParameter', this.accountParameter);
		this.router.navigate(['app/master/account/edit', id]);
	}

	public delete(id: number): void {
		if (!this.access.canDelete) {
			this.toastr.warning('You do not have delete access of this page.');
			return;
		}

		if (window.confirm('Are you sure you want to delete?')) {
			this.AccountService.delete(id).subscribe(data => {
				this.toastr.success('Record deleted successfully.');
				this.search();
			});
		}
	}

	public setPageListMode(): void {
		if (!this.access.canView) {
			this.toastr.warning('You do not have view access of this page.');
			return;
		}

		this.accountParameter.sortExpression = 'Id';
		this.setParameterByStateParam();

		this.search();

	}

	public setParameterByStateParam(): void {
		//let params = this.stateParam.popTill('accountParameter').params;
		//if (params != '') {
		//	this.accountParameter = params;
		//	this.AccountGrid.totalRecords = +params.totalRecords;
		//}
	}

	public setAccess(): void {
		this.access = this.sessionService.getAccess("master/account")
			// this.access.canInsert = true;
			// this.access.canUpdate = true;
			// this.access.canView = true;
			// this.access.canDelete= true;
	}
}