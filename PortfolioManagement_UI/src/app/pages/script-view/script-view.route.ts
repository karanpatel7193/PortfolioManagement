import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ScriptViewComponent } from "./script-view.component";

const routes: Routes = [
    {
        path: '',
        component: ScriptViewComponent 
    }
]; 

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ScriptViewRoute {
}
