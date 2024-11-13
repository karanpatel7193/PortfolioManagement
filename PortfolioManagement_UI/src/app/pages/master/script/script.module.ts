import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
//import { ColumnResizerModule } from '../../common-component/column-resizer/column-resizer.module';
import { ScriptListComponent } from './script-list.component';
import { ScriptFormComponent } from './script-form.component';
import { ScriptRoute } from './script.route';
import { ScriptService } from './script.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ScriptComponent } from './script.component';

@NgModule({
	imports: [
		FormsModule,
		CommonModule,
		NgbModule,

		//ColumnResizerModule,
		ScriptRoute
	],
	declarations: [
		ScriptComponent,
		ScriptListComponent,
		ScriptFormComponent
	],
	providers: [
		ScriptService
	]
})
export class ScriptModule { }
