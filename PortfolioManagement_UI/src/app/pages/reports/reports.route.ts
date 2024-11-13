import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "src/app/interceptors/auth.guard";
import { ReportComponent } from "./reports.component";

const routes: Routes = [
    {
        path: '',
        component: ReportComponent, 
        children: [
            {
                path: 'stockTransactionreport',
                loadChildren: () => import('./stockTransaction/stockTransaction.module').then(m => m.StockTransactionModule),
                canActivate: [AuthGuard]
			},
            {
				path: 'stockTranctionSummary',
				loadChildren: () => import('./stockTranctionSummary/stockSummary.module').then(m => m.StockSummaryModule),
				canActivate: [AuthGuard]
			},
            {
				path: 'portfoiloReport',
				loadChildren: () => import('./portfolioReport/portfolioReport.module').then(m => m.PortfolioReportModule),
				canActivate: [AuthGuard]
			},
            
        ]
    }
]; 

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ReportsRoute {
}
