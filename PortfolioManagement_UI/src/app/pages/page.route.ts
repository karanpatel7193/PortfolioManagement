import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { AppLayoutComponent } from '../layouts/app-layout/app-layout.component';
import { AuthGuard } from '../interceptors/auth.guard';

const routes: Routes = [
    {
        path: '',
        component: AppLayoutComponent, 
        children: [
            {
                path: 'dashboard',
                loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule),
                canActivate: [AuthGuard]
			},
			{
				path: 'master',
				loadChildren: () => import('./master/master.module').then(m => m.MasterModule),
				canActivate: [AuthGuard]
			},
            {
				path: 'account',
				loadChildren: () => import('./account/account.module').then(m => m.AccountModule),
				canActivate: [AuthGuard]
			},
            {
				path: 'transaction',
				loadChildren: () => import('./transaction/transaction.module').then(m => m.TransactionModule),
				canActivate: [AuthGuard]
			},
            {
				path: 'report',
				loadChildren: () => import('./reports/reports.module').then(m => m.ReportsModule),
				canActivate: [AuthGuard]
			},
			{
				path: 'watchlist',
				loadChildren: () => import('./watchlist/watchlist.module').then(m => m.WatchlistModule),
				canActivate: [AuthGuard]
			},
			{
				path: 'scriptView/:id',
				loadChildren: () => import('./script-view/script-view.module').then(m => m.ScriptViewModule),
				canActivate: [AuthGuard]
			},
			{
				path: 'indexView',
				loadChildren: () => import('./index-view/index-view.module').then(m => m.IndexViewModule),
				canActivate: [AuthGuard]
			},
			{
				path: 'analysis',
				loadChildren: () => import('./analysis/analysis.module').then(m => m.AnalyticsModule),
				canActivate: [AuthGuard]
			},
			
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PageRoute {
}
