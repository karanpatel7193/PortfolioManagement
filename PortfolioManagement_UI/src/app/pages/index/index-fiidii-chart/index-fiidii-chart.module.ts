import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgxEchartsModule } from 'ngx-echarts';
import { IndexFiidiiChartComponent } from './index-fiidii-chart.component';
import { IndexFiidiiChartService } from './index-fiidii-chart.service';

@NgModule({
  declarations: [IndexFiidiiChartComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgxEchartsModule.forRoot({
      echarts: () => import('echarts') 
    })
  ],
  providers: [IndexFiidiiChartService],
  exports: [IndexFiidiiChartComponent]
})
export class IndexFiidiiChartModule {}
