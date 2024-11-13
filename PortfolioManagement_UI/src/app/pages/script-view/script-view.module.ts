import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScriptViewChartComponent } from './script-view-chart/script-view-chart.component';
import { ScriptViewOverviewComponent } from './script-view-overview/script-view-overview.component';
import { ScriptViewPeersComponent } from './script-view-peers/script-view-peers.component';
import { ScriptViewRangeComponent } from './script-view-range/script-view-range.component';
import { ScriptViewRoute } from './script-view.route';
import { ScriptViewComponent } from './script-view.component';
import { NgxEchartsModule } from 'ngx-echarts';
import { ScriptViewAboutCompanyComponent } from './script-view-about-company/script-view-about-company.component';
import { ScriptViewCorporateActionComponent } from './script-view-corporateAction/script-view-corporateAction.component';

@NgModule({
  declarations: [
    ScriptViewComponent,
    ScriptViewChartComponent,
    ScriptViewOverviewComponent,
    ScriptViewPeersComponent,
    ScriptViewRangeComponent,
    ScriptViewAboutCompanyComponent,
    ScriptViewCorporateActionComponent,
    ],
    imports: [
        CommonModule,
        ScriptViewRoute,
        NgxEchartsModule  
    ],
    providers: [
    ]
})
export class ScriptViewModule { }
