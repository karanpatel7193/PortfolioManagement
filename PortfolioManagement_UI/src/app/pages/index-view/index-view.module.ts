import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxEchartsModule } from 'ngx-echarts';
import { IndexViewRoute } from './index-view.route';
import { IndexViewComponent } from './index-view.component';
import { IndexViewChartComponent } from './index-view-chart/index-view-chart.component';

@NgModule({
  declarations: [
      IndexViewComponent,
      IndexViewChartComponent
    ],
    imports: [
        CommonModule,
        IndexViewRoute,
        NgxEchartsModule  
    ],
    providers: [
    ]
})
export class IndexViewModule { }
