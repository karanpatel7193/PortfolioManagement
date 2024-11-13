import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';
import { AnalysisComponent } from './analysis.component';
import { AnalysisRoutingModule } from './analysis-routing.module';


@NgModule({
  declarations: [AnalysisComponent],
  imports: [
    CommonModule,
    AnalysisRoutingModule,
    FormsModule,

  ]
})
export class AnalyticsModule { }
