import { NgModule } from '@angular/core';
import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { SplitFormComponent } from './splitBonus-form.component';
import { SplitListComponent } from './splitBonus-list.component';
import { SplitComponent } from './splitBonus.component';


const routes: Routes = [{
	path: '',
	component: SplitComponent,
	children: [
		{
			path:'',
			redirectTo: 'list',
			pathMatch: 'full' 
		},
		{
			path: 'list',
			component: SplitListComponent,
		},
		{
			path: 'add',
			component: SplitFormComponent,
		},
		{
			path: 'edit/:id',
			component: SplitFormComponent,
		},
	],
}];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class SplitRoute {
}
