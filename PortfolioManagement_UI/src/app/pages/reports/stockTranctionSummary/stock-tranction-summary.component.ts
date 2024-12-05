import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { ToastService } from '../../../services/toast.service';
import { StockTransactionGridModel, StockTransactionListModel, StockTransactionModel, StockTransactionParameterModel, StockTransactionSummaryModel } from '../../transaction/stocktransaction/stocktransaction.model';
import { StocktransactionService } from '../../transaction/stocktransaction/stocktransaction.service';
import { MasterValuesModel } from 'src/app/models/mastervalue.model';
import { ScriptMainModel } from '../../master/script/script.model';
import { debounceTime, distinctUntilChanged, filter, map, Observable } from 'rxjs';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-stock-transaction-summary',
  templateUrl: './stock-tranction-summary.component.html',
  styleUrls: ['./stock-tranction-summary.component.scss']
})
export class StockTransactionSummaryComponent implements OnInit {
public access: AccessModel = new AccessModel();
	public stockTransaction: StockTransactionModel = new StockTransactionModel();
	public transactionParameter: StockTransactionParameterModel = new StockTransactionParameterModel();
	public stockTransactionSummaryes: StockTransactionSummaryModel[] = [];
	public transactionList: StockTransactionListModel = new StockTransactionListModel();
	public transactionGrid: StockTransactionGridModel = new StockTransactionGridModel();
    public stockTransactionListModel: StockTransactionListModel = new StockTransactionListModel();
	public masterValues: MasterValuesModel[] = [];
    public selectedScript: ScriptMainModel = new ScriptMainModel();
	sortColumn: string = ''; 
	sortDirection: string = '';
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
		private commonService: CommonService,
		private router: Router,
		private toastr: ToastService) {
		this.setAccess();
	}

	ngOnInit() {
		this.setPageListMode();
        this.fillDropdowns();
		this.loadMasterValues();

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
		this.transactionService.getForSummery(this.transactionParameter).subscribe(data => {
			this.stockTransactionSummaryes = data;
		});
	}

	sortData(column: keyof StockTransactionSummaryModel | 'profit' | 'investment') {
		if (column === 'profit' || column === 'investment') {
			this.stockTransactionSummaryes.sort((a, b) => {
				const valueA = this.getInvestmentAndProfit(a)[column];
				const valueB = this.getInvestmentAndProfit(b)[column];
				return this.sortDirection === 'asc' ? valueA - valueB : valueB - valueA;
			});
		} else {
			this.stockTransactionSummaryes = this.commonService.sortGrid(
				this.stockTransactionSummaryes,
				column,
				this.sortColumn,
				this.sortDirection
			);
		}
	
		if (this.sortColumn === column) {
			this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
		} else {
			this.sortColumn = column;
			this.sortDirection = 'asc';
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

	}

	public getInvestmentAndProfit(item: { buy: number; sell: number; dividend: number }): { investment: number; profit: number } {
		const profit = (item.sell + item.dividend) - item.buy;
		const investment = item.buy;
		return { investment, profit };
	}

	public setAccess(): void {
		const currentUrl = this.router.url.substring(0, this.router.url.indexOf('/list'));
		this.access = this.sessionService.getAccess(currentUrl);
	}

	public getTotalSums(): { totalBuy: number; totalSell: number; totalDividend: number; totalProfit: number; totalInvestment: number } {
		let totalBuy = 0;
		let totalSell = 0;
		let totalDividend = 0;
		let totalProfit = 0;
		let totalInvestment = 0;
	
		this.stockTransactionSummaryes.forEach(item => {
			totalBuy += item.buy;
			totalSell += item.sell;
			totalDividend += item.dividend;
			const { profit, investment } = this.getInvestmentAndProfit(item);
			totalProfit += profit;
			totalInvestment += investment;
		});
	
		return { totalBuy, totalSell, totalDividend, totalProfit, totalInvestment };
	}
	
}
