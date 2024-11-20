
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ScriptService } from './script.service';
import { ScriptParameterModel, ScriptGridModel, ScriptListModel, ScriptModel } from './script.model';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { ToastService } from '../../../services/toast.service';

@Component({
	selector: 'app-script-list',
	templateUrl: './script-list.component.html',
	styleUrls: ['./script-list.component.scss']
})

export class ScriptListComponent implements OnInit {
	public access: AccessModel = new AccessModel();
	public scriptParameter: ScriptParameterModel = new ScriptParameterModel();
	public scriptGrid: ScriptGridModel = new ScriptGridModel();
	public scriptList: ScriptListModel = new ScriptListModel();


	constructor(private scriptService: ScriptService,
		private sessionService: SessionService,
		private router: Router,
		private toastr: ToastService) {
		this.setAccess();
	}

	ngOnInit() {
		this.setPageListMode();
	}

	public reset(): void {
		this.scriptParameter = new ScriptParameterModel();
		this.scriptParameter.sortExpression = 'Id';
		this.scriptParameter.sortDirection = 'asc';
		this.search();
	}

	public search(): void {
		if (!this.access.canView) {
			this.toastr.warning('You do not have view access of this page.');
			return;
		}

		this.scriptService.getForGrid(this.scriptParameter).subscribe(data => {
			this.scriptGrid = data;
		});
	}

	public sort(sortExpression: string): void {
		if (sortExpression === this.scriptParameter.sortExpression) {
			this.scriptParameter.sortDirection = this.scriptParameter.sortDirection === 'asc' ? 'desc' : 'asc';
		}
		else {
			this.scriptParameter.sortExpression = sortExpression;
			this.scriptParameter.sortDirection = 'asc';
		}
		this.search();
	}

	public add(): void {
		if (!this.access.canInsert) {
			this.toastr.warning('You do not have add access of this page.');
			return;
		}

		//this.stateParam.push('scriptParameter', this.scriptParameter);
		this.router.navigate(['/app/master/script/add']);
	}

	public edit(id: number): void {
		if (!this.access.canUpdate) {
			this.toastr.warning('You do not have edit access of this page.');
			return;
		}

		//this.stateParam.push('scriptParameter', this.scriptParameter);
		this.router.navigate(['/app/master/script/edit', id]);
	}

	public delete(id: number): void {
		if (!this.access.canDelete) {
			this.toastr.warning('You do not have delete access of this page.');
			return;
		}

		if (window.confirm('Are you sure you want to delete?')) {
			this.scriptService.delete(id).subscribe(data => {
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

		this.scriptParameter.sortExpression = 'Id';
		this.setParameterByStateParam();

		this.search();

	}

	public setParameterByStateParam(): void {
		//let params = this.stateParam.popTill('scriptParameter').params;
		//if (params != '') {
		//	this.scriptParameter = params;
		//	this.scriptGrid.totalRecords = +params.totalRecords;
		//}
	}

	public setAccess(): void {
		const currentUrl = this.router.url.substring(0, this.router.url.indexOf('/list'));
		this.access = this.sessionService.getAccess(currentUrl);
	}
}
