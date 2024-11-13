import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {  SplitFormComponent } from './splitBonus-form.component';
import { SplitRoute } from './splitBonus.route';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SplitComponent } from './splitBonus.component';
import { SplitListComponent } from './splitBonus-list.component';
import { SplitService } from './splitBonus.service';
import { ScriptService } from '../script/script.service';

@NgModule({
	imports: [
		FormsModule,
		CommonModule,
		NgbModule,
		SplitRoute
	],
	declarations: [
		SplitComponent,
		SplitListComponent,
		SplitFormComponent
	],
	providers: [
		SplitService,
		ScriptService
	]
})
export class SplitModule { }
