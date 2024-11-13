  import { NgModule } from '@angular/core';
  import { CommonModule } from '@angular/common';
  import { FormsModule } from '@angular/forms';
  import { NgxEchartsModule } from 'ngx-echarts';
  import { BrowserModule } from '@angular/platform-browser';
import { IndexViewChartService } from './index-view-chart.service';


@NgModule({
    declarations: [
    ],
    imports: [
        CommonModule,
        FormsModule,
        BrowserModule,
        NgxEchartsModule.forRoot({
        echarts: () => import('echarts')
        })
    ],
    providers:[IndexViewChartService],
    exports: []
})
export class IndexViewChartModule { }
