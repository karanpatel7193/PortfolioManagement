import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DashboardComponent } from './dashboard.component';
import { DashboardRoute } from './dashboard.route';
import { StocktransactionService } from '../transaction/stocktransaction/stocktransaction.service';
import { PortfolioReportModule } from '../reports/portfolioReport/portfolioReport.module';
import { IndexChartModule } from '../index/indexChart/index-chart.module';
import { IndexFiidiiChartModule } from "../index/index-fiidii-chart/index-fiidii-chart.module";
@NgModule({
  declarations: [
    DashboardComponent,
    
  ],
  imports: [
    CommonModule,
    FormsModule,
    DashboardRoute,
    PortfolioReportModule,
    IndexChartModule,
    IndexFiidiiChartModule
],
  providers:[
    StocktransactionService
  ]
})
export class DashboardModule { }
