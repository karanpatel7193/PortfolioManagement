import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WatchlistRoutingModule } from './watchlist-route';
import { FormsModule } from '@angular/forms';
import { WatchlistFormComponent } from './watchlist-form.component';
import { WatchlistListComponent } from './watchlist-list.component';
import { WatchlistService } from './watchlist.service';
import { WatchlistComponent } from './watchlist.component';
import { ScriptService } from '../master/script/script.service';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [
    WatchlistComponent,
    WatchlistListComponent,
    WatchlistFormComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgbNavModule,
    WatchlistRoutingModule,
    NgbTypeaheadModule,
  ],

  providers:[WatchlistService,ScriptService]


})
export class WatchlistModule { }
