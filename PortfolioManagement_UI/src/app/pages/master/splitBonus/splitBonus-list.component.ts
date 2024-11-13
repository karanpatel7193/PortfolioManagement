
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { ToastService } from '../../../services/toast.service';
import { SplitParameterModel, SplitGridModel, SplitListModel } from './splitBonus.model';
import { SplitService } from './splitBonus.service';

@Component({
	selector: 'app-Split-list',
	templateUrl: './SplitBonus-list.component.html',
})
export class SplitListComponent implements OnInit {
	public access: AccessModel = new AccessModel();
	public splitParameter: SplitParameterModel = new SplitParameterModel();
	public splitGrid: SplitGridModel = new SplitGridModel();
	public splitList: SplitListModel = new SplitListModel();


	constructor(private splitService: SplitService,
		private sessionService: SessionService,
		private router: Router,
		private toastr: ToastService) {
		this.setAccess();
	}

	ngOnInit() {
		this.setPageListMode();
	}

	public reset(): void {
		this.splitParameter = new SplitParameterModel();
		this.splitParameter.sortExpression = 'Id';
		this.splitParameter.sortDirection = 'asc';
		this.search();
	}

	public search(): void {
		if (!this.access.canView) {
			this.toastr.warning('You do not have view access of this page.');
			return;
		}

		this.splitService.getForGrid(this.splitParameter).subscribe(data => {
			this.splitGrid = data;
		});
	}

	public sort(sortExpression: string): void {
		if (sortExpression === this.splitParameter.sortExpression) {
			this.splitParameter.sortDirection = this.splitParameter.sortDirection === 'asc' ? 'desc' : 'asc';
		}
		else {
			this.splitParameter.sortExpression = sortExpression;
			this.splitParameter.sortDirection = 'asc';
		}
		this.search();
	}

	public add(): void {
		if (!this.access.canInsert) {
			this.toastr.warning('You do not have add access of this page.');
			return;
		}

		//this.stateParam.push('SplitParameter', this.SplitParameter);
		this.router.navigate(['/app/master/split/add']);
	}

	public edit(id: number): void {
		if (!this.access.canUpdate) {
			this.toastr.warning('You do not have edit access of this page.');
			return;
		}

		//this.stateParam.push('SplitParameter', this.SplitParameter);
		this.router.navigate(['/app/master/split/edit', id]);
	}

	public delete(id: number): void {
		if (!this.access.canDelete) {
			this.toastr.warning('You do not have delete access of this page.');
			return;
		}

		if (window.confirm('Are you sure you want to delete?')) {
			this.splitService.delete(id).subscribe(data => {
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

		this.splitParameter.sortExpression = 'Id';
		this.setParameterByStateParam();

		this.search();

	}

	public setParameterByStateParam(): void {
		//let params = this.stateParam.popTill('SplitParameter').params;
		//if (params != '') {
		//	this.SplitParameter = params;
		//	this.SplitGrid.totalRecords = +params.totalRecords;
		//}
	}

	public setAccess(): void {
		this.access = this.sessionService.getAccess('master/split');
	}
}
