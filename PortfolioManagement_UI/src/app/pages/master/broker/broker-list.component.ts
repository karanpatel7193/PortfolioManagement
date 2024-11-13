import { Component, OnInit } from '@angular/core';
import { BrokerService } from './broker.service';
import { SessionService } from 'src/app/services/session.service';
import { Router } from '@angular/router';
import { ToastService } from 'src/app/services/toast.service';
import { BrokerGridModel, BrokerParameterModel } from './broker.model';
import { AccessModel } from 'src/app/models/access.model';

@Component({
selector: 'app-broker-list',
templateUrl: './broker-list.component.html',
styleUrls: []
})
export class BrokerListComponent implements OnInit {
	public access: AccessModel = new AccessModel();
	public brokerParameter: BrokerParameterModel = new BrokerParameterModel();
	public brokerGrid: BrokerGridModel = new BrokerGridModel();

constructor(private brokerService: BrokerService,
		private sessionService: SessionService,
		private router: Router,
		private toastr: ToastService
		) { 
		this.setAccess();
	}

ngOnInit() {
		this.setPageListMode();
	}

	public reset(): void {
		this.brokerParameter = new BrokerParameterModel();
		this.brokerParameter.sortExpression = 'Id';
		this.brokerParameter.sortDirection = 'asc';
		this.search();
	}

	public search(): void {
		if (!this.access.canView) {
			this.toastr.warning('You do not have view access of this page.');
			return;
		}

		this.brokerService.getForGrid(this.brokerParameter).subscribe(data => {
			this.brokerGrid = data;
		});
	}

	public sort(sortExpression: string): void {
		if (sortExpression === this.brokerParameter.sortExpression) {
			this.brokerParameter.sortDirection = this.brokerParameter.sortDirection === 'asc' ? 'desc' : 'asc';
		}
		else {
			this.brokerParameter.sortExpression = sortExpression;
			this.brokerParameter.sortDirection = 'asc';
		}
		this.search();
	}

	public add(): void {
		if (!this.access.canInsert) {
			this.toastr.warning('You do not have add access of this page.');
			return;
		}

		this.router.navigate(['/app/master/broker/add']);
	}

	public edit(id: number): void {
		if (!this.access.canUpdate) {
			this.toastr.warning('You do not have edit access of this page.');
			return;
		}

		this.router.navigate(['/app/master/broker/edit', id]);
	}

	public delete(id: number): void {
		if (!this.access.canDelete) {
			this.toastr.warning('You do not have delete access of this page.');
			return;
		}
		if (window.confirm('Are you sure you want to delete?')) {
			this.brokerService.delete(id).subscribe(data => {
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

		this.brokerParameter.sortExpression = 'Id';
		this.setParameterByStateParam();

		this.search();
	}

	public setParameterByStateParam(): void {
	}

	public setAccess(): void {
		this.access = this.sessionService.getAccess("master/broker");
	}
}
