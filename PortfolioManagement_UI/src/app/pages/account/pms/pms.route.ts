import { NgModule } from '@angular/core';
import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { PmsComponent } from './pms.component';
import { PmsListComponent } from './pms-list.component';
import { PmsFormComponent } from './pms-form.component';

const routes: Routes = [{
	path: '',
	component: PmsComponent,
	children: [
		{
			path: '',
			redirectTo: 'list',
			pathMatch: 'full'
		},
		{
			path: 'list',
			component: PmsListComponent,
		},
		{
			path: 'add',
			component: PmsFormComponent,
		},
		{
			path: 'edit/:id',
			component: PmsFormComponent,
		},
	],
}];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class PmsRoute {
}
