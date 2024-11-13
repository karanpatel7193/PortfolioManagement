import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MasterChildService} from './master.service';
import { MasterChildComponent } from './master.component';
import { MasterChildRoute } from './master.route';
import { MasterFormComponent } from './master-form.component';
import { MasterListComponent } from './master-list.component';

@NgModule({
	imports: [
		FormsModule,
		CommonModule,
		NgbModule,
		MasterChildRoute
	],
	declarations: [
		MasterChildComponent,
		MasterFormComponent,
		MasterListComponent
	],
	providers: [
		MasterChildService
	]
})
export class MasterModule { }
