import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { ToastService } from '../../../services/toast.service';
import { StockTransactionGridModel, StockTransactionListModel, StockTransactionModel, StockTransactionParameterModel } from '../../transaction/stocktransaction/stocktransaction.model';
import { StocktransactionService } from '../../transaction/stocktransaction/stocktransaction.service';
import { MasterValuesModel } from 'src/app/models/mastervalue.model';
import { CommonService } from 'src/app/services/common.service';
import { ScriptMainModel } from '../../master/script/script.model';
import { debounceTime, distinctUntilChanged, filter, map, Observable } from 'rxjs';
import { BrokerModel } from '../../master/broker/broker.model';

@Component({
	selector: 'app-stock-transaction-report',
	templateUrl: './stock-transaction-report.component.html',
	styleUrls: ['./stock-transaction-report.component.scss']
})
export class StockTransactionReportComponent implements OnInit {
    public access: AccessModel = new AccessModel();
	public stockTransaction: StockTransactionModel = new StockTransactionModel();

	public transactionParameter: StockTransactionParameterModel = new StockTransactionParameterModel();
	public transactionGrid: StockTransactionGridModel = new StockTransactionGridModel();
	public transactionList: StockTransactionListModel = new StockTransactionListModel();
    public stocks: StockTransactionModel[] = [];
    public stockTransactionListModel: StockTransactionListModel = new StockTransactionListModel();
    public masterValues: MasterValuesModel[] = [];
	public filteredBrokers: BrokerModel[] = [];


	public selectedScript: ScriptMainModel = new ScriptMainModel();
	formatter = (script: ScriptMainModel) => script.name;
	searchScript = (text$: Observable<string>) =>
		text$.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			filter((term) => term.length >= 2),
			map((term) => this.stockTransactionListModel.scripts.filter((script) => new RegExp(term, 'mi').test(script.name)).slice(0, 10)),
		);


	constructor(private transactionService: StocktransactionService,
		private sessionService: SessionService,
		private router: Router,
		private toastr: ToastService,
		private commonService: CommonService) {
		this.setAccess();
	}

	ngOnInit() {
		this.setPageListMode();
        this.fillDropdowns();
        this.loadMasterValues();
		console.log('All Brokers:', this.stockTransactionListModel.brokers);
	}

	

	public onScriptSelected(event: any) {
		if (event) {
			this.transactionParameter.scriptId = event.item.id;
		}
	}

	public loadMasterValues() {
        this.masterValues = this.sessionService.getUser().masterValues.filter(x => x.masterId == 3);
    }

    private fillDropdowns(): void {
        this.transactionService.getForList(this.transactionParameter).subscribe(
        (data) => {
            this.stockTransactionListModel = data;
        },
        error => {
            console.error('Failed to load roles:', error);
        }
        );
    }

	public reset(): void {
		this.transactionParameter = new StockTransactionParameterModel();
		this.transactionParameter.sortExpression = 'Id';
		this.transactionParameter.sortDirection = 'asc';
		this.selectedScript = new ScriptMainModel();
		this.search();
	}

	public search(): void {
		if (!this.access.canView) {
			this.toastr.warning('You do not have view access of this page.');
			return;
		}
		this.transactionService.getForReport(this.transactionParameter).subscribe(data => {
			this.stocks = data;
		});
	}

	public sort(sortExpression: string): void {
		if (sortExpression === this.transactionParameter.sortExpression) {
			this.transactionParameter.sortDirection = this.transactionParameter.sortDirection === 'asc' ? 'desc' : 'asc';
		}
		else {
			this.transactionParameter.sortExpression = sortExpression;
			this.transactionParameter.sortDirection = 'asc';
		}
		this.search();
	}

	public add(): void {
		if (!this.access.canInsert) {
			this.toastr.warning('You do not have add access of this page.');
			return;
		}

		//this.stateParam.push('transactionParameter', this.transactionParameter);
		this.router.navigate(['/app/transaction-stocktransaction/add']);
	}

	public edit(id: number): void {
		if (!this.access.canUpdate) {
			this.toastr.warning('You do not have edit access of this page.');
			return;
		}

		//this.stateParam.push('transactionParameter', this.transactionParameter);
		this.router.navigate(['/app/transaction-stocktransaction/edit', id]);
	}

	public delete(id: number): void {
		if (!this.access.canDelete) {
			this.toastr.warning('You do not have delete access of this page.');
			return;
		}

		if (window.confirm('Are you sure you want to delete?')) {
			this.transactionService.delete(id).subscribe(data => {
				this.toastr.success('Record deleted successfully.');
				this.search();
			});
		}
	}

	public setPageListMode(): void {
		if (!this.access.canView) {
			this.toastr.warning('You do not have view access of this page.');
			return;
		}

		this.transactionParameter.sortExpression = 'Id';
		this.setParameterByStateParam();

		this.search();

	}

	public setParameterByStateParam(): void {
		//let params = this.stateParam.popTill('transactionParameter').params;
		//if (params != '') {
		//	this.transactionParameter = params;
		//	this.transactionGrid.totalRecords = +params.totalRecords;
		//}
	}

	public redirect(id: number, symbol:string){
		this.commonService.redirectToPage(id,symbol);
	}

	public onAccountChange(): void {
		this.filteredBrokers = this.stockTransactionListModel.brokers.filter(
			broker => broker.accountId == this.transactionParameter.accountId
		);
		if (this.filteredBrokers.length > 0) {
			this.transactionParameter.brokerId = this.filteredBrokers[0].id; 
		} else {
			this.transactionParameter.brokerId = 0; 
		}
	}

	public setAccess(): void {
		//const currentUrl = this.router.url.substring(0, this.router.url.indexOf('/list'));
		this.access = this.sessionService.getAccess('report/stockTransactionreport');
	}
}
