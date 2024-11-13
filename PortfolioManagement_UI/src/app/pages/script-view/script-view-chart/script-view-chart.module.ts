  import { NgModule } from '@angular/core';
  import { CommonModule } from '@angular/common';
  import { ScriptViewChartService } from './script-view-chart.service';
  import { FormsModule } from '@angular/forms';
  import { NgxEchartsModule } from 'ngx-echarts';
  import { BrowserModule } from '@angular/platform-browser';
import { ScriptViewChartComponent } from './script-view-chart.component';


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
    providers:[ScriptViewChartService],
    exports: []
})
export class ScriptViewChartModule { }
