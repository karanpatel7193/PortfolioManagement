import { Component, OnDestroy, OnInit } from '@angular/core';
import { AccessModel } from 'src/app/models/access.model';
import { StockTransactionAddModel, StockTransactionEditModel, StockTransactionListModel, StockTransactionModel, StockTransactionParameterModel } from './stocktransaction.model';
import { SessionService } from 'src/app/services/session.service';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { ToastService } from 'src/app/services/toast.service';
import { StocktransactionService } from './stocktransaction.service';
import { MasterValuesModel } from 'src/app/models/mastervalue.model';
import { BrokerMainModel, BrokerModel } from '../../master/broker/broker.model';
import { transactionType } from 'src/app/enum';
import { DropdownSettings } from 'src/app/components/ngi-select/multiselect-dropdown-lib/src/lib/ngiselect.interface';
import { ScriptMainModel } from '../../master/script/script.model';
import { debounceTime, distinctUntilChanged, filter, map, Observable } from 'rxjs';

@Component({
	selector: 'app-transaction-form',
	templateUrl: './stocktransaction-form.component.html',
})
export class StocktransactionFormComponent implements OnInit, OnDestroy {
	public access: AccessModel = new AccessModel();
	public stockTransaction: StockTransactionModel = new StockTransactionModel();
	public transactionAdd: StockTransactionAddModel = new StockTransactionAddModel();
	public transactionEdit: StockTransactionEditModel = new StockTransactionEditModel();
	public sciptTypeConfig: DropdownSettings = {} as DropdownSettings;
	public sciptStatusConfig: DropdownSettings = {} as DropdownSettings;
	public transactionParameter: StockTransactionParameterModel = new StockTransactionParameterModel();
	public stockTransactionListModel: StockTransactionListModel = new StockTransactionListModel();
	public masterValues: MasterValuesModel[] = [];
	public hasAccess: boolean = false;
	public mode: string = '';
	public id: number = 0;
	private sub: any;
	public selectedScript: ScriptMainModel = new ScriptMainModel();
	public filteredBrokers: BrokerModel[] = [];

	formatter = (script: ScriptMainModel) => script.name;
	search = (text$: Observable<string>) =>
		text$.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			filter((term) => term.length >= 2),
			map((term) => this.stockTransactionListModel.scripts.filter((script) => new RegExp(term, 'mi').test(script.name)).slice(0, 10)),
		);

		isBuyDisabled: boolean = false;
		isSellDisabled: boolean = false;
		isDividendDisabled: boolean = false;

	constructor(
		private stockTransactionService: StocktransactionService,
		private sessionService: SessionService,
		private router: Router,
		private route: ActivatedRoute,
		private toastr: ToastService,
	) {
		this.setAccess();
	}


	ngOnInit(): void {
		this.getRouteData();
		this.fillDropdowns();
		this.loadMasterValues();
	}

	ngOnDestroy() {
		this.sub.unsubscribe();
	}

	onTransactionTypeChange(transactionTypeId: any) {
		if(transactionTypeId == transactionType.Buy){
				this.isBuyDisabled = false;
				this.isSellDisabled = true;
				this.isDividendDisabled = true;
		}
		else if(transactionTypeId == transactionType.Sell){
			this.isBuyDisabled = true;
				this.isSellDisabled = false;
				this.isDividendDisabled = true;
		}
		else if(transactionTypeId == transactionType.Dividened){
				this.isBuyDisabled = true;
				this.isSellDisabled = true;
				this.isDividendDisabled = false;
		}
	}
	
	onItemSelect(item: any) {
		console.log(item);
		console.log(this.stockTransaction.scriptId);
	}

	OnItemDeSelect(item: any) {
		console.log(item);
		console.log(this.stockTransaction.scriptId);
	}
	onSelectAll(items: any) {
		console.log(items);
	}
	onDeSelectAll(items: any) {
		console.log(items);
	}

	
	public onScriptSelected(event: any) {
		if (event) {
			this.stockTransaction.scriptId = event.item.id;
		}
	}

	public loadMasterValues() {
		this.masterValues = this.sessionService.getUser().masterValues.filter(x => x.masterId == 3);
	}

	private fillDropdowns(): void {
		this.stockTransactionService.getForList(this.transactionParameter).subscribe(
			(data) => {
				this.stockTransactionListModel = data;
				if (this.mode === 'Edit') {
					this.selectedScript = this.stockTransactionListModel.scripts.find(x => x.id == this.stockTransaction.scriptId) || new ScriptMainModel();
					this.onAccountChange();
				}
			},
			error => {
				console.error('Failed to load roles:', error);
			}
		);
	}

	public getRouteData(): void {
		this.sub = this.route.params.subscribe(params => {
			const segments: UrlSegment[] = this.route.snapshot.url;
			if (segments.toString().toLowerCase() === 'add')
				this.id = 0;
			else
				this.id = +params['id']; // (+) converts string 'id' to a number
			this.setPageMode();
		});
	}

	public clearModels(): void {
		this.stockTransaction = new StockTransactionModel();
	}

	public setPageMode(): void {
		if (this.id === undefined || this.id === 0)
			this.setPageAddMode();
		else
			this.setPageEditMode();

		if (this.hasAccess) {
		}
	}

	public setPageAddMode(): void {
		if (!this.access.canInsert) {
			this.toastr.warning('You do not have add access of this page.');
			return;
		}
		this.hasAccess = true;
		this.mode = 'Add';
		this.clearModels();
	}

	public setPageEditMode(): void {
		if (!this.access.canUpdate) {
			this.toastr.warning('You do not have update access of this page.');
			return;
		}
		this.hasAccess = true;
		this.mode = 'Edit';

		this.stockTransactionService.getRecord(this.id).subscribe(data => {
			this.stockTransaction = data;
			this.onAccountChange();
		});
	}

	public onAccountChange(): void {
		this.filteredBrokers = this.stockTransactionListModel.brokers.filter(
			broker => broker.accountId == this.stockTransaction.accountId
		);
		if (this.filteredBrokers.length > 0) {
			this.stockTransaction.brokerId = this.filteredBrokers[0].id; 
		} else {
			this.stockTransaction.brokerId = 0; 
		}

	}

	public save(isFormValid: boolean | null): void {
		if (isFormValid) {
			if (!this.access.canInsert && !this.access.canUpdate) {
				this.toastr.warning('You do not have add or edit access of this page.');
				return;
			}
			this.stockTransactionService.save(this.stockTransaction).subscribe(data => {
				if (data === 0) {
					this.toastr.warning('Record is already exist.');
				}
				else if (data > 0) {
					this.toastr.success('Record submitted successfully.');
					this.cancel();
				}
			});
		} else {
			this.toastr.warning('Please provide valid input.');
		}
	}

	public cancel(): void {
		this.router.navigate(['app/transaction/stocktransaction/list']);
	}

	public calculateAmount(isBrokerageUpdate: boolean = false) {
		const selectedBrokerer: BrokerModel = this.stockTransactionListModel.brokers.filter(x => x.id == this.stockTransaction.brokerId)[0];
		if (selectedBrokerer) {
			if (this.stockTransaction.transactionTypeId == transactionType.Buy) {
				if (!isBrokerageUpdate)
				this.stockTransaction.brokerage = parseFloat(((this.stockTransaction.qty * this.stockTransaction.price) * (selectedBrokerer.buyBrokerage / 100)).toFixed(2));
				this.stockTransaction.buy = (this.stockTransaction.qty * this.stockTransaction.price) + this.stockTransaction.brokerage;
				this.stockTransaction.sell = 0;
				this.stockTransaction.dividend = 0;
			}
			else if (this.stockTransaction.transactionTypeId == transactionType.Sell) {
				if (!isBrokerageUpdate)
				this.stockTransaction.brokerage = parseFloat(((this.stockTransaction.qty * this.stockTransaction.price) * (selectedBrokerer.sellBrokerage / 100)).toFixed(2));
				this.stockTransaction.sell = (this.stockTransaction.qty * this.stockTransaction.price) - this.stockTransaction.brokerage;
				this.stockTransaction.buy = 0;
				this.stockTransaction.dividend = 0;
			}
			else {
				if (!isBrokerageUpdate)
				this.stockTransaction.brokerage = parseFloat(((this.stockTransaction.qty * this.stockTransaction.price) * (selectedBrokerer.sellBrokerage / 100)).toFixed(2));
				this.stockTransaction.dividend = (this.stockTransaction.qty * this.stockTransaction.price) - this.stockTransaction.brokerage
				this.stockTransaction.buy = 0
				this.stockTransaction.sell = 0
			}
		}
	}

	public calculateBrokerage() {
		const selectedBrokerer: BrokerModel = this.stockTransactionListModel.brokers.filter(x => x.id == this.stockTransaction.brokerId)[0];

		if (selectedBrokerer) {
			if (this.stockTransaction.transactionTypeId == transactionType.Buy) {
				const totalValue = this.stockTransaction.qty * this.stockTransaction.price;
				this.stockTransaction.brokerage = parseFloat((this.stockTransaction.buy - totalValue).toFixed(2)) ;
			}
			else if (this.stockTransaction.transactionTypeId == transactionType.Sell) {
				const totalValue = this.stockTransaction.qty * this.stockTransaction.price;
				this.stockTransaction.brokerage = parseFloat((totalValue - this.stockTransaction.sell).toFixed(2));
			}
		}
	}


	public setAccess(): void {
		this.access = this.sessionService.getAccess('transaction/stocktransaction');
	}
}
