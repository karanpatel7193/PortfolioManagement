import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { StocktransactionService} from './stocktransaction.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { StocktransactionComponent } from './stocktransaction.component';
import { StocktransactionListComponent } from './stocktransaction-list.component';
import { StocktransactionFormComponent } from './stocktransaction-form.component';
import { StocktransactionRoute } from './stocktransaction.route';
import { NgiSelectModule } from 'src/app/components/ngi-select/multiselect-dropdown-lib/src/public-api';

@NgModule({
	imports: [
    FormsModule,
    CommonModule,
    NgbModule,
	NgiSelectModule,
    StocktransactionRoute,
	
],
	declarations: [
		StocktransactionComponent,
		StocktransactionListComponent,
  		StocktransactionFormComponent
	],
	providers: [
		StocktransactionService
	]
})
export class StocktransactionModule { }
