import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { StockTransactionRoute } from './stockTransaction.route';
import { StockTransactionReportComponent } from './stock-transaction-report.component';
import { StocktransactionService } from '../../transaction/stocktransaction/stocktransaction.service';
// import { StocktransactionService } from '../../transaction/stocktransaction/stocktransaction.service';

@NgModule({
declarations: 
    [
        StockTransactionReportComponent
    ],
imports: 
    [   
        CommonModule,
        FormsModule,
        NgbModule, 
        StockTransactionRoute
    ],
providers: 
    [
        StocktransactionService
    ],
})
export class StockTransactionModule {}
