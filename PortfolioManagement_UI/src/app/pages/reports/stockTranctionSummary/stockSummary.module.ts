import { NgModule } from '@angular/core';

import { StockSummaryRoutingModule } from './stock-summary-routing.module';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { StockTransactionSummaryComponent } from './stock-tranction-summary.component';
import { StocktransactionService } from '../../transaction/stocktransaction/stocktransaction.service';


@NgModule({
  declarations: [
    StockTransactionSummaryComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgbModule, 
    StockSummaryRoutingModule
  ],
  providers: 
    [
        StocktransactionService
    ],
})
export class StockSummaryModule { }
