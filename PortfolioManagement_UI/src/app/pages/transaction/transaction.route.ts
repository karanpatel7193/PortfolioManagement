import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "src/app/interceptors/auth.guard";
import { TransactionComponent } from "./transaction.component";

const routes: Routes = [
    {
        path: '',
        component: TransactionComponent, 
        children: [
            {
                path: 'stocktransaction',
                loadChildren: () => import('./stocktransaction/stocktransaction.module').then(m => m.StocktransactionModule),
                canActivate: [AuthGuard]
			},
        ]
        
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TransactionRoute {
}
