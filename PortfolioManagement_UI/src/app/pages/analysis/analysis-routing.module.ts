import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/interceptors/auth.guard';
import { VolumeComponent } from './volume/volume.component';

const routes: Routes = [
  {
      path: '',
      component: VolumeComponent, 
      children: [
        {
          path: 'stockVolume',
          loadChildren: () => import('../analysis/volume/volume.module').then(m => m.VolumeModule),
          canActivate: [AuthGuard]
        },
          
      ]
  }
]; 

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AnalysisRoutingModule { }


