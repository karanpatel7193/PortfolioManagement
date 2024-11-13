import { PagingSortingModel } from '../../../models/pagingsorting.model';
import { AccountMainModel } from '../../master/account/account.model';
import { BrokerModel } from '../../master/broker/broker.model';
import { ScriptMainModel } from '../../master/script/script.model';

export class StockTransactionMainModel {
	public id: number = 0;
}

export class StockTransactionModel extends StockTransactionMainModel {
	public date: Date = new Date();  // Initialize to epoch or another default value if needed
	public accountName: string = '';
	public transactionTypeName: string = '';
	public scriptName: string = '';
	public qty: number = 0;
	public price: number = 0;
	public brokerName: string = '';
	public brokerage: number = 0;
	public buy: number = 0;
	public sell: number = 0;
	public dividend: number = 0;
	public accountId: number = 0;
	public transactionTypeId: number = 0;
	public scriptId: number = 0;
	public brokerId: number = 0;
}

export class StockTransactionAddModel {
	// Add properties if any needed for creating a new stock transaction
}

export class StockTransactionEditModel extends StockTransactionAddModel {
	public transaction: StockTransactionModel = new StockTransactionModel();
}

export class StockTransactionGridModel {
	public stocks: StockTransactionModel[] = [];
	public totalRecords: number = 0;
	public summery: StockTransactionSummaryModel[] = [];

}

export class StockTransactionListModel extends StockTransactionGridModel {
    public accounts: AccountMainModel[] = [];
    public scripts: ScriptMainModel[] = [];
    public brokers: BrokerModel[] = [];
}

export class StockTransactionParameterModel extends PagingSortingModel {
	public id: number = 0;
	public accountId: number = 0;
	public transactionTypeId: number = 0;
	public scriptId: number = 0;
	public brokerId: number = 0;
	public fromDate?: Date;
	public toDate?: Date;
	public name: any;
}

export class StockTransactionSummaryModel {
	public accountId: number = 0;
	public accountName: string = '';
	public buy: number = 0;
	public sell: number = 0;
	public dividend: number = 0;
}

export class PortfolioReportModel
{
	pmsId: number = 0;
	brokerId: number = 0;
	accountId: number = 0;
	public date: Date = new Date(); 
	investmentAmount: number = 0;
	unReleasedAmount: number = 0;
}
