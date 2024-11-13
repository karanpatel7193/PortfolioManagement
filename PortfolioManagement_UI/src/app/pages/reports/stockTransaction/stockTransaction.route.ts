
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { StockTransactionReportComponent } from './stock-transaction-report.component';
import { AuthGuard } from 'src/app/interceptors/auth.guard';

const routes: Routes = [
    {
		path: '',
		component: StockTransactionReportComponent,
		children: [
			{
				path: 'list',
				component: StockTransactionReportComponent,
				canActivate : [AuthGuard]
			},
		]
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class StockTransactionRoute {
}
