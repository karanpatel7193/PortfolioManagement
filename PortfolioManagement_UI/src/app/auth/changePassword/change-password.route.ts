import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { AuthGuard } from 'src/app/interceptors/auth.guard';
import { ChangePasswordComponent } from './change-password.component';

const routes: Routes = [
    {
        path: '',
        //component: ChangePasswordComponent,
        canActivate: [AuthGuard],
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DashboardRoute {
}
