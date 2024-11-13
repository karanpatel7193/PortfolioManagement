import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PageRoute } from './page.route';
import { LayoutModule } from '../layouts/layout.module';
import { DatepickerModule } from '../components/datepicker/datepicker.module';
@NgModule({
	declarations: [
],
	imports: [
		CommonModule,
		LayoutModule,
		FormsModule,
		NgbModule,
		PageRoute,
		DatepickerModule
	]
})
export class PageModule { }
