import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PortfolioReportComponent } from './portfoilo-report.component';
import { AuthGuard } from 'src/app/interceptors/auth.guard';

const routes: Routes = [
  {
    path: '', 
      component: PortfolioReportComponent,
      children: [
        {
          path: 'list',
          component: PortfolioReportComponent,
          canActivate : [AuthGuard]
        },
      ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PortfolioReportRoutingModule { }
