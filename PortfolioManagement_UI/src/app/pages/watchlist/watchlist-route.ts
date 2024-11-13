import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WatchlistComponent } from './watchlist.component';
import { WatchlistListComponent } from './watchlist-list.component';
import { WatchlistFormComponent } from './watchlist-form.component';
import { AuthGuard } from 'src/app/interceptors/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: WatchlistComponent,
    children: [
      
      {
          path:'',
          redirectTo: 'list',
          pathMatch: 'full' 
      },
      {
          path: 'list',
          component: WatchlistListComponent,
          canActivate : [AuthGuard]
      },
      {
        path: 'add',
        component: WatchlistFormComponent,
        canActivate : [AuthGuard]
      },
      {
        path: 'edit/:id',
        component: WatchlistFormComponent,
        canActivate : [AuthGuard]
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WatchlistRoutingModule { }
