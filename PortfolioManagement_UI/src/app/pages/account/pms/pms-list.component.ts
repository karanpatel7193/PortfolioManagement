
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { ToastService } from '../../../services/toast.service';
import { PmsGridModel, PmsListModel, PmsParameterModel } from './pms.model';
import { PmsService } from './pms.service';

@Component({
	selector: 'app-pms-list',
	templateUrl: './pms-list.component.html',
})
export class PmsListComponent implements OnInit {
	public access: AccessModel = new AccessModel();
	public pmsParameter: PmsParameterModel = new PmsParameterModel();
	public pmsGrid: PmsGridModel = new PmsGridModel();
	public pmsList: PmsListModel = new PmsListModel();


	constructor(private pmsService: PmsService,
		private sessionService: SessionService,
		private router: Router,
		private toastr: ToastService) {
		this.setAccess();
	}

	ngOnInit() {
		this.setPageListMode();
	}

	public reset(): void {
		this.pmsParameter = new PmsParameterModel();
		this.pmsParameter.sortExpression = 'Id';
		this.pmsParameter.sortDirection = 'asc';
		this.search();
	}

	public search(): void {
		if (!this.access.canView) {
			this.toastr.warning('You do not have view access of this page.');
			return;
		}

		this.pmsService.getForGrid(this.pmsParameter).subscribe(data => {
			this.pmsGrid = data;
		});
	}

	public sort(sortExpression: string): void {
		if (sortExpression === this.pmsParameter.sortExpression) {
			this.pmsParameter.sortDirection = this.pmsParameter.sortDirection === 'asc' ? 'desc' : 'asc';
		}
		else {
			this.pmsParameter.sortExpression = sortExpression;
			this.pmsParameter.sortDirection = 'asc';
		}
		this.search();
	}

	public add(): void {
		if (!this.access.canInsert) {
			this.toastr.warning('You do not have add access of this page.');
			return;
		}

		//this.stateParam.push('pmsParameter', this.pmsParameter);
		this.router.navigate(['/app/account/pms/add']);
	}

	public edit(id: number): void {
		if (!this.access.canUpdate) {
			this.toastr.warning('You do not have edit access of this page.');
			return;
		}

		//this.stateParam.push('pmsParameter', this.pmsParameter);
		this.router.navigate(['/app/account/pms/edit', id]);
	}

	public delete(id: number): void {
		if (!this.access.canDelete) {
			this.toastr.warning('You do not have delete access of this page.');
			return;
		}

		if (window.confirm('Are you sure User IsActive False? OK')) {
			this.pmsService.delete(id).subscribe(data => {
				this.toastr.success('User IsActive False successfully.');
				this.search();
			});
		}
	}

	public setPageListMode(): void {
		if (!this.access.canView) {
			this.toastr.warning('You do not have view access of this page.');
			return;
		}

		this.pmsParameter.sortExpression = 'Id';
		this.setParameterByStateParam();

		this.search();

	}

	public setParameterByStateParam(): void {
		//let params = this.stateParam.popTill('pmsParameter').params;
		//if (params != '') {
		//	this.pmsParameter = params;
		//	this.pmsGrid.totalRecords = +params.totalRecords;
		//}
	}

	public setAccess(): void {
		this.access = this.sessionService.getAccess('account/pms');
	}
}
