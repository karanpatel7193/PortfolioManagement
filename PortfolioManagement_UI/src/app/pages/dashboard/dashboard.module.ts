import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DashboardComponent } from './dashboard.component';
import { DashboardRoute } from './dashboard.route';
import { StocktransactionService } from '../transaction/stocktransaction/stocktransaction.service';
import { PortfolioReportModule } from '../reports/portfolioReport/portfolioReport.module';

@NgModule({
  declarations: [
    DashboardComponent,

  ],
  imports: [
    CommonModule,
    FormsModule,
    DashboardRoute,
    PortfolioReportModule
  ],
  providers:[
    StocktransactionService
  ]
})
export class DashboardModule { }
