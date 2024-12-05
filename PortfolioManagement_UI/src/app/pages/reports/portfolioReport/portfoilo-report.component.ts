import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { AccessModel } from 'src/app/models/access.model';
import { StockTransactionGridModel, StockTransactionListModel, StockTransactionModel, StockTransactionParameterModel } from '../../transaction/stocktransaction/stocktransaction.model';
import { MasterValuesModel } from 'src/app/models/mastervalue.model';
import { StocktransactionService } from '../../transaction/stocktransaction/stocktransaction.service';
import { SessionService } from 'src/app/services/session.service';
import { Router } from '@angular/router';
import { ToastService } from 'src/app/services/toast.service';
import { PortfolioDonutChartModel, PortfolioReportModel, PortfolioScriptModel } from './portfolio-report.model';
import { CommonService } from 'src/app/services/common.service';
import { BrokerModel } from '../../master/broker/broker.model';

@Component({
	selector: 'app-portfoilo-report',
	templateUrl: './portfolio-report.component.html',
	styleUrls: ['./portfolio-report.components.scss']
})
export class PortfolioReportComponent implements OnInit {
	public access: AccessModel = new AccessModel();
	public stockTransaction: StockTransactionModel = new StockTransactionModel();
	public transactionParameter: StockTransactionParameterModel = new StockTransactionParameterModel();
	public portfolioReportModels: PortfolioReportModel = new PortfolioReportModel();
	public transactionList: StockTransactionListModel = new StockTransactionListModel();
	public transactionGrid: StockTransactionGridModel = new StockTransactionGridModel();
	public stockTransactionListModel: StockTransactionListModel = new StockTransactionListModel();
	public masterValues: MasterValuesModel[] = [];
	public investmentSectorData: PortfolioDonutChartModel[] = [];
	public marketSectorData: PortfolioDonutChartModel[] = [];
    sortColumn: string = ''; 
    sortDirection: string = '';
	// public filteredBrokers: BrokerModel[] = [];

	showFilters = false;
	@Input() isShowChart: boolean = false ;
	@Input() isPortfolioReport: boolean = true ;

    public myAccountId: number = 0;
    public myBrokerId: number = 0;

	constructor(private transactionService: StocktransactionService,
		private sessionService: SessionService,
		private router: Router,
		private toastr: ToastService,
		private cdr: ChangeDetectorRef,
		private commonService: CommonService) {
		this.setAccess();
	}

	ngOnInit() {
		this.setPageListMode();
		this.fillDropdowns();
		this.loadMasterValues();
	}
	public toggleFilters(): void {
		this.showFilters = !this.showFilters;
	}
    public sortData(column: keyof PortfolioScriptModel) {
        this.portfolioReportModels.scripts = this.commonService.sortGrid(this.portfolioReportModels.scripts, column, this.sortColumn, this.sortDirection);
    
        if (this.sortColumn === column) {
            this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
        } else {
            this.sortColumn = column;
            this.sortDirection = 'asc'; 
        }
    }
	toggleMenu() {
        this.showFilters = !this.showFilters; 
    }
	
	private additionalProcessing(): void {
		if (this.stockTransactionListModel.accounts?.length > 0) {
			this.transactionParameter.accountId = this.stockTransactionListModel.accounts[0].id;
		}

		if (this.stockTransactionListModel.brokers?.length > 0) {
			this.transactionParameter.brokerId = this.stockTransactionListModel.brokers[0].id;
		}
	}

	public loadMasterValues() {
		this.masterValues = this.sessionService.getUser().masterValues.filter(x => x.masterId == 3);
	}

	public getChartData() {
		this.transactionService.getPortfolioReport(this.transactionParameter).subscribe((data) => {
			this.portfolioReportModels = data;
			this.processChartData();
		})
	}

	private processChartData() {
		if (this.portfolioReportModels.investmentSectors) {
			this.investmentSectorData = this.portfolioReportModels.investmentSectors.map(sector => ({
				value: sector.percentage,
				name: sector.sectorName
			}));
		}

		if (this.portfolioReportModels.marketSectors) {
			this.marketSectorData = this.portfolioReportModels.marketSectors.map(sector => ({
				value: sector.percentage,
				name: sector.sectorName
			}));
		}
	}

	private fillDropdowns(): void {
		this.transactionService.getForList(this.transactionParameter).subscribe(
			(data) => {
				this.stockTransactionListModel = data;
				this.additionalProcessing();
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

		this.transactionParameter.accountId = 0;
		this.transactionParameter.brokerId = 0;
		this.fillDropdowns();
		this.search();
	}

	
	public search(): void {
		if (!this.access.canView) {
			this.toastr.warning('You do not have view access of this page.');
			return;
		}
        this.myAccountId = this.transactionParameter.accountId;
        this.myBrokerId = this.transactionParameter.brokerId;
        
		this.transactionService.getPortfolioReport(this.transactionParameter).subscribe(data => {
			this.portfolioReportModels = data;
			this.processChartData();
		});
	}

	public onAccountChange(): void {
		const filterBroker = this.stockTransactionListModel.brokers.filter((item)=>{
			return item.accountId == this.transactionParameter.accountId;
		})

		if (filterBroker.length > 0) {
			this.transactionParameter.brokerId  = filterBroker[0].id
		} else {
			this.transactionParameter.brokerId = 0; 
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

	public redirect(id: number, symbol:string){
		this.commonService.redirectToPage(id,symbol);
	}

	public setAccess(): void {
		const currentUrl = this.router.url.substring(0, this.router.url.indexOf('/list'));
		this.access = this.sessionService.getAccess(currentUrl);
	}

}
