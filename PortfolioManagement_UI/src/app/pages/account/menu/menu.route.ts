import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { MenuListComponent } from './menu-list.component';
import { MenuFormComponent } from './menu-form.component';
import { MenuComponent } from './menu.component';
import { AuthGuard } from '../../../interceptors/auth.guard';

const routes: Routes = [
    {
        path: '',
        component: MenuComponent,
        children: [
            {
                path:'',
                redirectTo: 'list',
                pathMatch: 'full' 
            },
            {
                path: 'list',
                component: MenuListComponent,
                canActivate : [AuthGuard]
            },
            {
                path: 'add',
                component: MenuFormComponent,
                canActivate : [AuthGuard]

            },
            {
                path: 'edit/:id',
                component: MenuFormComponent,
                canActivate : [AuthGuard]

            },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MenuRoute {
}
