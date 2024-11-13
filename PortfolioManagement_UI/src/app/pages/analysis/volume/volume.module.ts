import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';
import { VolumeComponent } from './volume.component';
import { VolumeRoutingModule } from './volumerouting';


@NgModule({
  declarations: [
    VolumeComponent,
  ],
  imports: [
    CommonModule,
    VolumeRoutingModule,
    FormsModule,

  ]
})
export class VolumeModule { }
