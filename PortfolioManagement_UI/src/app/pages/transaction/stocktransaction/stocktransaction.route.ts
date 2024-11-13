import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StocktransactionComponent } from './stocktransaction.component';
import { StocktransactionFormComponent } from './stocktransaction-form.component';
import { StocktransactionListComponent } from './stocktransaction-list.component';
import { AuthGuard } from 'src/app/interceptors/auth.guard';

const routes: Routes = [{
	path: '',
	component: StocktransactionComponent,
	children: [
	{
		path:'',
		redirectTo: 'list',
		pathMatch: 'full' 
	},
	{
		path: 'list',
		component: StocktransactionListComponent,
		canActivate : [AuthGuard]

	},
	{
		path: 'add',
		component: StocktransactionFormComponent,
		canActivate : [AuthGuard]

	},
	{
		path: 'edit/:id',
		component: StocktransactionFormComponent,
		canActivate : [AuthGuard]

	},
	],
}];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class StocktransactionRoute {
}
