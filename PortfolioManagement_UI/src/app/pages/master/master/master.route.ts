import { NgModule } from '@angular/core';
import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { MasterChildComponent } from './master.component';
import { MasterFormComponent } from './master-form.component';
import { MasterListComponent } from './master-list.component';

const routes: Routes = [{
	path: '',
	component: MasterChildComponent,
	children: [
		{
			path:'',
			redirectTo: 'list',
			pathMatch: 'full' 
		},
		{
			path: 'list',
			component: MasterListComponent,
		},
		{
			path: 'add',
			component: MasterFormComponent,
		},
		{
			path: 'edit/:id',
			component: MasterFormComponent,
		}
	],
}];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class MasterChildRoute {
}
