import { Component, OnDestroy, OnInit } from '@angular/core';
import { BrokerService } from './broker.service';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { ToastService } from 'src/app/services/toast.service';
import { AccessModel } from 'src/app/models/access.model';
import { BrokerAddModel, BrokerEditModel, BrokerMainModel, BrokerModel, BrokerParameterModel } from './broker.model';
import { SessionService } from 'src/app/services/session.service';
import { MasterValuesModel } from 'src/app/models/mastervalue.model';

@Component({
selector: 'app-broker-form',
templateUrl: './broker-form.component.html',
styleUrls: []
})
export class BrokerFormComponent implements OnInit,OnDestroy {
public access: AccessModel = new AccessModel();
public broker: BrokerModel = new BrokerModel ();
public brokers: BrokerModel [] = [];
public brokerAdd: BrokerAddModel = new BrokerAddModel();
public brokerEdit: BrokerEditModel = new BrokerEditModel();
public brokerParameter: BrokerParameterModel = new BrokerParameterModel();
public brokerId: BrokerMainModel[] = [];
public hasAccess: boolean = false;
public mode: string = '';
public id: number = 0;
private sub: any;
router: any;
public masterValues: MasterValuesModel[] = [];


constructor(private brokerService: BrokerService,
			private sessionService: SessionService,
			private Router: Router,
			private route: ActivatedRoute,
			private toastr: ToastService
) { 
	this.setAccess();

}
ngOnInit() {
		this.getRouteData();
		this.loadMasterValues();
	}
	public loadMasterValues() {
		this.masterValues = this.sessionService.getUser().masterValues.filter(x => x.masterId == 2);
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
		this.broker = new BrokerModel();
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
		this.broker.id = 0;
	}

	public setPageEditMode(): void {
		if (!this.access.canUpdate) {
			this.toastr.warning('You do not have update access of this page.');
			return;
		}
		this.hasAccess = true;
		this.mode = 'Edit';


		this.brokerService.getRecord(this.id).subscribe(data => {
			this.broker = data;
		});

	}

	public save(isFormValid: boolean | null): void {
		if (isFormValid) {
			if (!this.access.canInsert && !this.access.canUpdate) {
				this.toastr.warning('You do not have add or edit access of this page.');
				return;
			}
			this.brokerService.save(this.broker, this.mode).subscribe(data => {
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
		this.Router.navigate(['/app/master/broker/list']);
	}

	public setAccess(): void {
		this.access = this.sessionService.getAccess("master/broker");
	}
}
