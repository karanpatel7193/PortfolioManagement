		import { Component, OnInit, OnDestroy } from '@angular/core';
		import { AccountModel, AccountParameterModel, } from './account.model';
		import { AccessModel } from 'src/app/models/access.model';
		import { AccountService } from './account.service';
		import { SessionService } from 'src/app/services/session.service';
		import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
		import { ToastService } from 'src/app/services/toast.service';
		import { Subscription } from 'rxjs';
		import { BrokerService } from '../broker/broker.service';

		@Component({
			selector: 'app-account-form',
			templateUrl: './account-form.component.html',
			styleUrls: []
		})
		export class AccountFormComponent implements OnInit, OnDestroy {
			public access: AccessModel = new AccessModel();
			public accountModel: AccountModel = new AccountModel();
			public accountParameter: AccountParameterModel = new AccountParameterModel();
			public hasAccess: boolean = false;
			public mode: string = '';
			public id: number = 0;
			private sub: Subscription | undefined;

			constructor(
				private accountService: AccountService,
				private sessionService: SessionService,
				private router: Router,
				private route: ActivatedRoute,
				private toastr: ToastService,
				private brokerService: BrokerService, // Inject BrokerService
			) {
				this.setAccess();
			}

			ngOnInit() {
				this.getRouteData();
			}

			ngOnDestroy() {
				if (this.sub) {
					this.sub.unsubscribe();
				}
			}

			public getRouteData(): void {
				this.sub = this.route.params.subscribe(params => {
					const segments: UrlSegment[] = this.route.snapshot.url;
					this.id = segments.toString().toLowerCase() === 'add' ? 0 : +params['id'];
					this.setPageMode();
				});
			}

			public clearModels(): void {
				this.accountModel.id = 0;
				this.accountModel.name = '';
				this.accountModel.brokers.forEach(broker => broker.isSelected = false); // Unselect all brokers
			}

			public setPageMode(): void {
				if (this.id === 0) {
					this.setPageAddMode();
				} else {
					this.setPageEditMode();
				}
			}

			public setPageAddMode(): void {
				if (!this.access.canInsert) {
					this.toastr.warning('You do not have add access to this page.');
					return;
				}

				this.hasAccess = true;
				this.mode = 'Add';
				this.clearModels();

				this.accountService.getForAdd(this.accountParameter).subscribe((data) => {
					this.accountModel.brokers = data.brockers;
				})
			}
			
			public setPageEditMode(): void {
				if (!this.access.canUpdate) {
					this.toastr.warning('You do not have update access to this page.');
					return;
				}
				this.hasAccess = true;
				this.mode = 'Edit';
				this.accountParameter.id = this.id;
				this.accountService.getForEdit(this.accountParameter).subscribe((data) => {
					this.accountModel = data.account;
				})
			}

			public save(isFormValid: boolean | null): void {

				if (isFormValid) {
					if (!this.access.canInsert && !this.access.canUpdate) {
						this.toastr.warning('You do not have add or edit access to this page.');
						return;
					}
					this.accountService.save(this.accountModel).subscribe(data => {
						if (data === 0) {
							this.toastr.warning('Record already exists.');
						} else if (data > 0) {
							this.toastr.success('Record submitted successfully.');
							this.cancel();
						}
					});
				} else {
					this.toastr.warning('Please provide valid input.');
				}

			}

			public cancel(): void {
				this.router.navigate(['app/master/account/list']);
			}
			public setAccess(): void {
				this.access = this.sessionService.getAccess("master/account")
			}	
		}