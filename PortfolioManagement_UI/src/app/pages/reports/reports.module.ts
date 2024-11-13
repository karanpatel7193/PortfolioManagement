import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportComponent } from './reports.component';
import { ReportsRoute } from './reports.route';



@NgModule({
  declarations: [
    ReportComponent,
  ],
  imports: [
    CommonModule,
    ReportsRoute
  ]
})
export class ReportsModule { }
