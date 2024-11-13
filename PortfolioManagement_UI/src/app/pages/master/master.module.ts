import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MasterRoute } from './master.route';
import { MasterComponent } from './master.component';



@NgModule({
  declarations: [
    MasterComponent
  ],
  imports: [
    CommonModule,
    MasterRoute
  ]
})
export class MasterModule { }
