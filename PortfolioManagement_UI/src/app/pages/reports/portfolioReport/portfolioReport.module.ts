import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PortfolioReportRoutingModule } from './portfolioReport-routing.module';
import { PortfolioReportComponent } from './portfoilo-report.component';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { StocktransactionService } from '../../transaction/stocktransaction/stocktransaction.service';
import { NgxEchartsModule } from 'ngx-echarts';
import { PortfolioReportChartComponent } from './portfolioReport-chart/portfolio-report-chart.component';
import { PortfolioReportSummaryComponent } from './portfolio-report-summary/portfolio-report-summary.component';
import { PortfolioDatewiseChartComponent } from './portfolio-datewise-chart/portfolio-datewise-chart.component';


@NgModule({
  declarations: [
    PortfolioReportComponent,
    PortfolioReportChartComponent,
    PortfolioReportSummaryComponent,
    PortfolioDatewiseChartComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgbModule, 
    PortfolioReportRoutingModule,
    NgxEchartsModule.forRoot({
      echarts: () => import('echarts')
      })
  ],
  exports: [
    PortfolioReportComponent
  ],
  providers: 
    [
        StocktransactionService
    ],
})
export class PortfolioReportModule { }
