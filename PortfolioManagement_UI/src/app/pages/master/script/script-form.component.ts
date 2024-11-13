
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute, UrlSegment } from '@angular/router';
import { ScriptService } from './script.service';
import { ScriptModel, ScriptAddModel, ScriptEditModel, ScriptParameterModel } from './script.model';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { ToastService } from '../../../services/toast.service';

@Component({
	selector: 'app-script-form',
	templateUrl: './script-form.component.html',
})
export class ScriptFormComponent implements OnInit, OnDestroy {
	public access: AccessModel = new AccessModel();
	public script: ScriptModel = new ScriptModel();
	public scripts: ScriptModel[] = [];
	public scriptAdd: ScriptAddModel = new ScriptAddModel();
	public scriptEdit: ScriptEditModel = new ScriptEditModel();
	public scriptParameter: ScriptParameterModel = new ScriptParameterModel();


	public hasAccess: boolean = false;
	public mode: string = '';
	public id: number = 0;
	private sub: any;

	constructor(private scriptService: ScriptService,
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
		this.script = new ScriptModel();
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


		this.scriptService.getRecord(this.id).subscribe(data => {
			this.script = data;
		});

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

	public save(isFormValid: boolean | null): void {
		if (!this.access.canInsert && !this.access.canUpdate) {
			this.toastr.warning('You do not have add or edit access of this page.');
			return;
		}
		if (isFormValid) {
			this.scriptService.save(this.script).subscribe(data => {
				if (data === 0)
					this.toastr.warning('Record is already exist.');
				else if (data > 0) {
					this.toastr.success('Record submitted successfully.');
					this.cancel();
				}
			});
		} else {
			this.toastr.warning('Please provide valid input.');
		}
	}

	public cancel(): void {
		this.router.navigate(['/app/master/script/list']);
	}
	public setAccess(): void {
		const currentUrl = this.router.url.substring(0, this.router.url.indexOf('/list'));
		this.access = this.sessionService.getAccess(currentUrl);
	}
}
