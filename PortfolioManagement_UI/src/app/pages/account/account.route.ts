import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { AuthGuard } from 'src/app/interceptors/auth.guard';
import { AccountComponent } from './account.component';

const routes: Routes = [
    {
        path: '',
        component:AccountComponent,
        children: [
            {
                path: 'menu',
                loadChildren: () => import('./menu/menu.module').then(m => m.MenuModule),
                canActivate: [AuthGuard]
			},
            {
                path: 'role',
                loadChildren: () => import('./role/role.module').then(m => m.RoleModule),
                canActivate: [AuthGuard]
			},
            {
                path: 'user',
                loadChildren: () => import('./user/user.module').then(m => m.UserModule),
                canActivate: [AuthGuard]
			},
            {
                path: 'pms',
                loadChildren: () => import('./pms/pms.module').then(m => m.PmsModule),
                canActivate: [AuthGuard]
			},
        ],
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AccountRoute {
}
