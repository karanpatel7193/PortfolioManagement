import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {  StockTransactionSummaryComponent } from './stock-tranction-summary.component';
import { AuthGuard } from 'src/app/interceptors/auth.guard';

const routes: Routes = [
  { 
      path: '', 
      component: StockTransactionSummaryComponent,
      children: [
        {
          path: 'list',
          component: StockTransactionSummaryComponent,
          canActivate : [AuthGuard]
        },
      ]
    } 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StockSummaryRoutingModule { }
