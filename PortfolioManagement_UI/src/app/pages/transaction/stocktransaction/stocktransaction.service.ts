import { Injectable } from '@angular/core';
import { HttpService } from '../../../services/http.service';
import { map, Observable } from 'rxjs';
import { StockTransactionGridModel, StockTransactionListModel, StockTransactionMainModel, StockTransactionModel, StockTransactionParameterModel, StockTransactionSummaryModel} from './stocktransaction.model';
import { PortfolioReportModel } from '../../reports/portfolioReport/portfolio-report.model';

@Injectable()
export class StocktransactionService {
	constructor(private http: HttpService) {
	}

	public getRecord(id: number): Observable<StockTransactionModel> {
		return this.http.get('stockTransaction/stockTransaction/getRecord/' + id).pipe(
			map((response: StockTransactionModel) => {
				return response;
			})
		)
	}

	public getForLOV(transactionParameter: StockTransactionParameterModel): Observable<StockTransactionModel[]> {
		return this.http.post('stockTransaction/stockTransaction/getLovValue', transactionParameter).pipe(
			map((response: StockTransactionModel[]) => {
				return response;
			}),
		);
	}
	

	public getForGrid(transactionParameter: StockTransactionParameterModel): Observable<StockTransactionGridModel> {
		return this.http.post('stockTransaction/stockTransaction/getGridData', transactionParameter).pipe(
			map((response: StockTransactionGridModel) => {
				return response;
			}),
		);
	}

	

	public save(transaction: StockTransactionModel): Observable<number> {
		if (transaction.id === 0)
			return this.http.post('stockTransaction/stockTransaction/insert', transaction).pipe(
				map((response: number) => {
					return response;
				}),
			);
		else
			return this.http.post('stockTransaction/stockTransaction/update', transaction).pipe(
				map((response: number) => {
					return response;
				}),
			);
	}
	
	public delete(id: number): Observable<void> {
		return this.http.post('stockTransaction/stockTransaction/delete/' + id, null).pipe(
			map((response: void) => {
				return response;
			}),
		);
	}

    public getForList(transactionParameter: StockTransactionParameterModel): Observable<StockTransactionListModel> {
		return this.http.post('stockTransaction/stockTransaction/getForList', transactionParameter).pipe(
			map((response: StockTransactionListModel) => {
				return response;
			}),
		);
	}

    public getForReport(transactionParameter: StockTransactionParameterModel): Observable<StockTransactionModel[]> {
		return this.http.post('stockTransaction/stockTransaction/getReportData', transactionParameter).pipe(
			map((response: StockTransactionModel[]) => {
				return response;
			}),
		);
	}

	public getForSummery(transactionParameter: StockTransactionParameterModel): Observable<StockTransactionSummaryModel[]> {
		return this.http.post('stockTransaction/stockTransaction/getSummaryData', transactionParameter).pipe(
			map((response: StockTransactionSummaryModel[]) => {
				return response;
			}),
		);
	}
	
	public getPortfolioReport(transactionParameter: StockTransactionParameterModel): Observable<PortfolioReportModel> {
		return this.http.post('portfolio/portfolio/getPortfolioReport', transactionParameter).pipe(
			map((response: PortfolioReportModel) => {
				return response;
			}),
		);
	}

	

}
