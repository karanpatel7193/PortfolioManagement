import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { RoleListComponent } from './role-list.component';
import { RoleFormComponent } from './role-form.component';
import { RoleComponent } from './role.component';
import { RoleAccessComponent } from '../rolemenuaccess/role-access.component';
import { AuthGuard } from 'src/app/interceptors/auth.guard';

const routes: Routes = [ {
    path: '',
    component: RoleComponent,
    children: [
        {
			path:'',
			redirectTo: 'list',
			pathMatch: 'full' 
		},
        {
            path: 'list',
            component: RoleListComponent,
            canActivate : [AuthGuard]
        },
        {
            path: 'add',
            component: RoleFormComponent,
            canActivate : [AuthGuard]

        },
        {
            path: 'edit/:id',
            component: RoleFormComponent,
            canActivate : [AuthGuard]

        },
        {
            path: 'access/:roleId/:roleName',
            component: RoleAccessComponent,
        }
    ]
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RoleRoute {
}

// {
//     path: 'access/:roleId/:roleName',
//     component: RoleAccessComponent,
// },