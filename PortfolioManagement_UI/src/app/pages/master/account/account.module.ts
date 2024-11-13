import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AccountComponent } from './account.componets';
import { AccountListComponent } from './account-list.component';
import { AccountFormComponent } from './account-form.component';
import { AccountRoute } from './account.route';
import { AccountService } from './account.service';
import { BrokerService } from '../broker/broker.service';


@NgModule({
	imports: [
		FormsModule,
		CommonModule,
		NgbModule,
		AccountRoute
	],
	declarations: [
		AccountComponent,
		AccountListComponent,
		AccountFormComponent
	],
	providers: [
		AccountService,
		BrokerService
	]
})
export class AccountModule { }
