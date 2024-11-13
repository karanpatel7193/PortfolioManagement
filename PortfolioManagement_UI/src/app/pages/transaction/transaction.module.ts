import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {TransactionComponent } from './transaction.component';
import { TransactionRoute } from './transaction.route';



@NgModule({
  declarations: [
    TransactionComponent
  ],
  imports: [
    CommonModule,
    TransactionRoute
    
  ]
})
export class TransactionModule { }
