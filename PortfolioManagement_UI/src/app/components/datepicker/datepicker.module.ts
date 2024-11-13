import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DatepickerComponent } from './datepicker.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [DatepickerComponent],
  imports: [
    CommonModule,
    NgbModule,
    FormsModule
  ],
  exports: [DatepickerComponent]
})
export class DatepickerModule { }
