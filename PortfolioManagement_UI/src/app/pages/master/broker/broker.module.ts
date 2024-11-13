import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {  NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrokerComponets } from './broker.components';
import { BrokerFormComponent } from './broker-form.component';
import { BrokerListComponent } from './broker-list.component';
import { BrokerService } from './broker.service';
import { BrokerRoute } from './broker.route';



@NgModule({
  declarations: [
    BrokerComponets,
    BrokerFormComponent,
    BrokerListComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgbModule,

    BrokerRoute

  ],
  providers: [BrokerService]
})
export class BrokerModule { }
