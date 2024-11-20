import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgxEchartsModule } from 'ngx-echarts';
import { IndexChartService } from './index-chart.service';
import { IndexChartComponent } from './index-chart.component';

@NgModule({
  declarations: [
    IndexChartComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgxEchartsModule.forRoot({
      echarts: () => import('echarts')  // Lazy load echarts
    })
  ],
  providers: [IndexChartService],
  exports: [IndexChartComponent]  // Export the component to use it in other modules
})
export class IndexChartModule { }
