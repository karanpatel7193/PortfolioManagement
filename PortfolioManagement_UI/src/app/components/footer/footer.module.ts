import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FooterComponent } from './footer.component';
import { ModalModule } from '../modal/modal.module';

@NgModule({
  declarations: [
    FooterComponent
  ],
  imports: [
    CommonModule,
    ModalModule
  ],
  exports: [
    FooterComponent
  ]
})
export class FooterModule { }
