
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute, UrlSegment } from '@angular/router';
import { PmsService} from './pms.service';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { ToastService } from '../../../services/toast.service';
import { PmsAddModel, PmsEditModel, PmsModel, PmsParameterModel } from './pms.model';

@Component({
	selector: 'app-pms-form',
	templateUrl: './pms-form.component.html',
})
export class PmsFormComponent implements OnInit, OnDestroy {
	public access: AccessModel = new AccessModel();
	public pms: PmsModel = new PmsModel();
	public pmss: PmsModel[] = [];
	public pmsAdd: PmsAddModel = new PmsAddModel();
	public pmsEdit: PmsEditModel = new PmsEditModel();
	public pmsParameter: PmsParameterModel = new PmsParameterModel();


	public hasAccess: boolean = false;
	public mode: string = '';
	public id: number = 0;
	private sub: any;

	constructor(private pmsService: PmsService,
		private sessionService: SessionService,
		private router: Router,
		private route: ActivatedRoute,
		private toastr: ToastService) {
			this.setAccess();
	}

	ngOnInit() {
		this.getRouteData();
	}

	ngOnDestroy() {
		this.sub.unsubscribe();
	}

	public getRouteData(): void {
		this.sub = this.route.params.subscribe(params => {
			const segments: UrlSegment[] = this.route.snapshot.url;
			if (segments.toString().toLowerCase() === 'add')
				this.id = 0;
			else
				this.id = +params['id']; // (+) converts string 'id' to a number
			this.setPageMode();
		});
	}

	public clearModels(): void {
		this.pms = new PmsModel();
	}

	public setPageMode(): void {
		if (this.id === undefined || this.id === 0)
			this.setPageAddMode();
		else
			this.setPageEditMode();

		if (this.hasAccess) {
		}
	}

	public setPageAddMode(): void {
		if (!this.access.canInsert) {
			this.toastr.warning('You do not have add access of this page.');
			return;
		}
		this.hasAccess = true;
		this.mode = 'Add';


		this.clearModels();
	}

	public setPageEditMode(): void {
		if (!this.access.canUpdate) {
			this.toastr.warning('You do not have update access of this page.');
			return;
		}
		this.hasAccess = true;
		this.mode = 'Edit';


		this.pmsService.getRecord(this.id).subscribe(data => {
			this.pms = data;
		});

	}
	public add(): void {
		if (!this.access.canInsert) {
			this.toastr.warning('You do not have add access of this page.');
			return;
		}
		this.router.navigate(['/app/account/pms/add']);
	}

	public edit(id: number): void {
		if (!this.access.canUpdate) {
			this.toastr.warning('You do not have edit access of this page.');
			return;
		}
		this.router.navigate(['/app/account/pms/edit', id]);
	}

	public save(isFormValid: boolean | null): void {
		if (!this.access.canInsert && !this.access.canUpdate) {
			this.toastr.warning('You do not have add or edit access of this page.');
			return;
		}
		if (isFormValid) {
			this.pmsService.save(this.pms).subscribe(data => {
				if (data === 0)
					this.toastr.warning('Record is already exist.');
				else if (data > 0) {
					this.toastr.success('Record submitted successfully.');
					this.router.navigate(['app/account/pms/list']);
					this.cancel();
				}
			});
		} else {
			this.toastr.warning('Please provide valid input.');
		}
	}

	public cancel(): void {
		this.router.navigate(['/app/account/pms/list']);
	}
	public setAccess(): void {
		this.access = this.sessionService.getAccess('account/pms');
	}
}
