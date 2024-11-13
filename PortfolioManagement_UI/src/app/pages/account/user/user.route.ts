import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { UserListComponent } from './user-list.component';
import { UserFormComponent } from './user-form.component';
import { UserComponent } from './user.component';
// import { RoleGuardService } from '../../../services/role-guard.service';
import { AuthGuard } from 'src/app/interceptors/auth.guard';

const routes: Routes = [ {
    path: '',
    component: UserComponent,
    children: [
        {
			path:'',
			redirectTo: 'list',
			pathMatch: 'full' 
		},
        {
            path: 'list',
            component: UserListComponent,
            canActivate : [AuthGuard]
        },
        {
            path: 'add',
            component: UserFormComponent,
            canActivate : [AuthGuard]
        },
        {
            path: 'edit/:id',
            component: UserFormComponent,
            canActivate : [AuthGuard]

        },
    ]
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class UserRoute {
}
