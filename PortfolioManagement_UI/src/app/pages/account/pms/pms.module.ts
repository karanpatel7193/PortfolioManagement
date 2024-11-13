import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PmsComponent } from './pms.component';
import { PmsFormComponent } from './pms-form.component';
import { PmsListComponent } from './pms-list.component';
import { PmsService } from './pms.service';
import { PmsRoute } from './pms.route';

@NgModule({
	imports: [
		FormsModule,
		CommonModule,
		NgbModule,
		PmsRoute
	],
	declarations: [
		PmsComponent,
		PmsListComponent,
		PmsFormComponent
	],
	providers: [
		PmsService
	]
})
export class PmsModule { }
