import { NgModule } from '@angular/core';
import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { BrokerListComponent } from './broker-list.component';
import { BrokerFormComponent } from './broker-form.component';
import { BrokerComponets } from './broker.components';
import { AuthGuard } from 'src/app/interceptors/auth.guard';

const routes: Routes = [{
	path: '',
	component: BrokerComponets,
	children: [
		{
			path:'',
			redirectTo: 'list',
			pathMatch: 'full' 
		},
		{
			path: 'list',
			component: BrokerListComponent,
			canActivate : [AuthGuard]
		},
		{
			path: 'add',
			component: BrokerFormComponent,
			canActivate : [AuthGuard]

		},
		{
			path: 'edit/:id',
			component: BrokerFormComponent,
			canActivate : [AuthGuard]

		},
	]
}];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class BrokerRoute {
}
