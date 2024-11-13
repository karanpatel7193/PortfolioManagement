import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { IndexViewComponent } from "./index-view.component";

const routes: Routes = [
    {
        path: '',
        component: IndexViewComponent 
    }
]; 

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class IndexViewRoute {
}
