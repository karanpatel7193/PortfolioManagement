import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { ToastService } from '../../../services/toast.service';
import { MasterChildService } from './master.service';
import {
	MasterGridModel,
	MasterListModel,
	MasterModel,
	MasterParameterModel,
	MasterValueModel,
} from './master.model';

@Component({
	selector: 'app-master-add',
	templateUrl: './master-form.component.html',
})
export class MasterFormComponent implements OnInit {
	public access: AccessModel = new AccessModel();
	public masterGrid: MasterGridModel = new MasterGridModel();
	public masterList: MasterListModel = new MasterListModel();
	public master: MasterModel = new MasterModel();
	public masterValueModel: MasterValueModel = new MasterValueModel();
	public masterParameter: MasterParameterModel = new MasterParameterModel();

	public hasAccess: boolean = false;
	public mode: string = '';
	public id: number = 0;
	private sub: any;

	constructor(
		private masterService: MasterChildService,
		private sessionService: SessionService,
		private router: Router,
		private route: ActivatedRoute,
		private toaster: ToastService
	) 
	{
		this.setAccess();
	}

	ngOnInit() {
		this.getRouteData();
		// this.setPageListMode();
	}
	public setPageMode(): void {
		if (this.id === undefined || this.id === 0)
			this.setPageAddMode();
		else
			this.setPageEditMode();

		if (this.hasAccess) {
		}
	}

	public getRouteData(): void {
		this.sub = this.route.params.subscribe(params => {
			const segments: UrlSegment[] = this.route.snapshot.url;
			this.id = segments.toString().toLowerCase() === 'add' ? 0 : +params['id'];
			this.setPageMode();
		});
	}

	public setPageAddMode(): void {
		if (!this.access.canInsert) {
			this.toaster.warning('You do not have add access of this page.');
			return;
		}
		this.hasAccess = true;
		this.mode = 'Add';

	}

	public setPageEditMode(): void {
		if (!this.access.canUpdate) {
			this.toaster.warning('You do not have update access of this page.');
			return;
		}
		this.hasAccess = true;
		this.mode = 'Edit';

		this.masterService.getRecord(this.id).subscribe(data => {
			this.master = data;
		});
	}

	public add(): void {
		if (!this.access.canInsert) {
			this.toaster.warning('You do not have add access of this page.');
			return;
		}

		this.router.navigate(['/app/master/master-values/add']);
	}

	public delete(id: number): void {
		if (!this.access.canDelete) {
			this.toaster.warning('You do not have delete access of this page.');
			return;
		}

		if (window.confirm('Are you sure you want to delete?')) {
			this.masterService.delete(id).subscribe((data) => {
				this.toaster.success('Record deleted successfully.');
			});
		}
	}

	public setPageListMode(): void {
		if (!this.access.canView) {
			this.toaster.warning('You do not have view access of this page.');
			return;
		}
	}

	public newRows: MasterValueModel[] = [];

	save(isFormValid: boolean | null): void {
		if (!this.access.canInsert && !this.access.canUpdate) {
			this.toaster.warning('You do not have add or edit access to this page.');
			return;
		}
		if (isFormValid) {
			this.masterService.save(this.master, this.mode).subscribe((data) => {
				if (data === 0) {
					this.toaster.warning('Record already exists.');
				} else if (data > 0) {
					this.toaster.success('Record submitted successfully.');
					this.router.navigate(['app/master/master-values/list']);
					this.cancel();
				}
			});
		} else {
			this.toaster.warning('Please provide valid input.');
		}
	}

	addRow(): void {
		const newRow: MasterValueModel = {
			masterId: this.master.id,
			value: this.masterValueModel.value,
			valueText: this.masterValueModel.valueText,
		};
		this.master.masterValues.push(newRow);
		this.masterValueModel = { masterId: 0, value: 0, valueText: '' };
	}

	public cancel(): void {
		this.router.navigate(['app/master/master-values/list']);
	}
	updateRow(index: number) {
		const updatedRow = this.newRows[index];
		console.log('Updated Row:', updatedRow);
	}

	deleteRow(index: number): void {
		this.master.masterValues.splice(index, 1);
	}
	public setAccess(): void {
		 this.access = this.sessionService.getAccess('master/master-values');
	}

}
