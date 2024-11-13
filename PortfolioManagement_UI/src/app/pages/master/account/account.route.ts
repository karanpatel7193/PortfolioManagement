import { NgModule } from '@angular/core';
import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { AccountListComponent } from './account-list.component';
import { AccountFormComponent } from './account-form.component';
import { AccountComponent } from './account.componets';
import { AuthGuard } from 'src/app/interceptors/auth.guard';

const routes: Routes = [{
	path: '',
	component: AccountComponent,
	children: [
		{
			path:'',
			redirectTo: 'list',
			pathMatch: 'full' 
		},
		{
			path: 'list',
			component: AccountListComponent,
			canActivate : [AuthGuard]
		},
		{
			path: 'add',
			component: AccountFormComponent,
			canActivate : [AuthGuard]

		},
		{
			path: 'edit/:id',
			component: AccountFormComponent,
			canActivate : [AuthGuard]

		},
	]
}];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class AccountRoute {
}
