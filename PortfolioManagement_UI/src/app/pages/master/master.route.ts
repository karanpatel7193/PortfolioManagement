import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "src/app/interceptors/auth.guard";
import { MasterComponent } from "./master.component";

const routes: Routes = [
    {
        path: '',
        component: MasterComponent, 
        children: [
            {
                path: 'account',
                loadChildren: () => import('./account/account.module').then(m => m.AccountModule),
                canActivate: [AuthGuard]
			},
            {
                path: 'broker',
                loadChildren: () => import('./broker/broker.module').then(m => m.BrokerModule),
                canActivate: [AuthGuard]
			},
            {
                path: 'script',
                loadChildren: () => import('./script/script.module').then(m => m.ScriptModule),
                canActivate: [AuthGuard]
			},
            {
                path: 'master-values',
                loadChildren: () => import('./master/master.module').then(m => m.MasterModule),
                canActivate: [AuthGuard]
			},
            {
                path: 'split',
                loadChildren: () => import('./splitBonus/splitBonus.module').then(m => m.SplitModule),
                canActivate: [AuthGuard]
			}
        ]
        
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MasterRoute {
}
