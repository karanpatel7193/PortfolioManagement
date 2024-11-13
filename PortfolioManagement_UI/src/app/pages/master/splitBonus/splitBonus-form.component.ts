
import { Component, OnInit, OnDestroy, SimpleChanges } from '@angular/core';
import { Router, ActivatedRoute, UrlSegment } from '@angular/router';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { ToastService } from '../../../services/toast.service';
import { SplitModel, SplitListModel } from './splitBonus.model';
import { SplitService } from './splitBonus.service';
import { ScriptMainModel, ScriptParameterModel } from '../script/script.model';
import { ScriptService } from '../script/script.service';

@Component({
	selector: 'app-Split-form',
	templateUrl: './splitBonus-form.component.html',
})
export class SplitFormComponent implements OnInit, OnDestroy {
	public access: AccessModel = new AccessModel();
	public split: SplitModel = new SplitModel();
	public splits: SplitModel[] = [];
	public splitListModel: SplitListModel = new SplitListModel();
	public scripts: ScriptMainModel[] = [];
	public scriptParameter: ScriptParameterModel = new ScriptParameterModel();

	public hasAccess: boolean = false;
	public mode: string = '';
	public id: number = 0;
	private sub: any;

	constructor(private splitService: SplitService,
		private scriptService: ScriptService,
		private sessionService: SessionService,
		private router: Router,
		private route: ActivatedRoute,
		private toastr: ToastService,
	) {
		this.setAccess();
	}

	ngOnInit() {
		this.getRouteData();
		this.fillDropdown();
	}
	ngOnDestroy() {
		this.sub.unsubscribe();
	}

	public fillDropdown(): void {
		this.splitService.getForLOV(this.scriptParameter).subscribe(data => {
			this.scripts = data;
		});
	}

	public getFaceValue(id: number): void {
		this.scriptService.getRecord(id).subscribe(data => {
			this.split.oldFaceValue = data.faceValue;
		})
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
		this.split = new SplitModel();
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


		this.splitService.getRecord(this.id).subscribe(data => {
			this.split = data;
		});

	}

	public save(isFormValid: boolean | null): void {
		if (isFormValid) {
			if (!this.access.canInsert && !this.access.canUpdate) {
				this.toastr.warning('You do not have add or edit access of this page.');
				return;
			}

			this.splitService.save(this.split).subscribe(data => {
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
		this.router.navigate(['/app/master/split/list']);
	}

	public apply(id: number): void {
		if (!this.access.canUpdate) {
			this.toastr.warning('You do not have delete access of this page.');
			return;
		}
		if (window.confirm('Are you sure you want to Apply?')) {
			const isApply = true;
			this.splitService.splitBonusApply(id, isApply).subscribe(data => {
				this.toastr.success('New Record Applied successfully.');
			});
		}

	}
	public setAccess(): void {
		this.access = this.sessionService.getAccess('master/split');
	}
}
