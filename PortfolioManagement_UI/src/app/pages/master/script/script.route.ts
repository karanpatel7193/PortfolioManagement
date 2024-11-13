import { NgModule } from '@angular/core';
import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { ScriptListComponent } from './script-list.component';
import { ScriptFormComponent } from './script-form.component';
import { ScriptComponent } from './script.component';

const routes: Routes = [{
	path: '',
	component: ScriptComponent,
	children: [
		{
			path:'',
			redirectTo: 'list',
			pathMatch: 'full' 
		},
		{
			path: 'list',
			component: ScriptListComponent,
		},
		{
			path: 'add',
			component: ScriptFormComponent,
		},
		{
			path: 'edit/:id',
			component: ScriptFormComponent,
		},
	],
}];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class ScriptRoute {
}
