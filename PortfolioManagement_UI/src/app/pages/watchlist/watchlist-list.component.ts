import { Component, OnInit } from '@angular/core';
import { WatchlistMainModel,  WatchlistParameterModel, WatchlistScriptModel, WatchlistScriptTabModel } from './watchlist.model';
import { AccessModel } from 'src/app/models/access.model';
import { ToastService } from 'src/app/services/toast.service';
import { Router } from '@angular/router';
import { SessionService } from 'src/app/services/session.service';
import { WatchlistService } from './watchlist.service';
import { ScriptMainModel, ScriptParameterModel } from '../master/script/script.model';
import { ScriptService } from '../master/script/script.service';
import { StockTransactionListModel } from '../transaction/stocktransaction/stocktransaction.model';
import { debounceTime, distinctUntilChanged, filter, map, Observable } from 'rxjs';
import { CommonService } from 'src/app/services/common.service';

@Component({
	selector: 'app-watchlist-list', 
	templateUrl: './watchlist-list.component.html',
	styleUrls: ['./watchlist-list.component.scss'],
})

export class WatchlistListComponent implements OnInit {
	public scriptParameterModel: ScriptParameterModel = new ScriptParameterModel();
	public scriptListModel:StockTransactionListModel = new StockTransactionListModel();
	public watchlist: WatchlistMainModel[] = [];
	public watchlistScriptTabModel: WatchlistScriptTabModel[] = [];
	public watchlistScriptEntity: WatchlistScriptModel = new WatchlistScriptModel();
	public watchlistParameterModel: WatchlistParameterModel = new WatchlistParameterModel();
	public access: AccessModel = new AccessModel();
	public activeTabId: number = 0;
	public refreshInterval: any; 
	sortColumn: string = ''; 
	sortDirection: string = '';

	public selectedScript: ScriptMainModel = new ScriptMainModel();
	formatter = (script: ScriptMainModel) => script.name;
	search = (text$: Observable<string>) =>
		text$.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			filter((term) => term.length >= 2),
			map((term) => this.scriptListModel.scripts.filter((script) => new RegExp(term, 'mi').test(script.name)).slice(0, 10)),
		);

	constructor(
		private watchlistService:WatchlistService,
		private scriptService:ScriptService,
		private sessionService: SessionService,
		private router: Router,
		private toastr: ToastService,
		private commonService: CommonService) { 
		}

		ngOnInit(): void {
			this.setAccess();
			this.fetchData();
			this.fillDropDown();
		}

		ngOnDestroy(): void {
			if (this.refreshInterval) {
			clearInterval(this.refreshInterval);
			}
		}

		public getPriceChangeClass(oldPrice: number, price: number): string {
			if (oldPrice === 0 || oldPrice === price) {
				return 'normal';
			} else if (oldPrice > price) {
				return 'red'; 
			} else {
				return 'green'; 
			}
		}
		public onScriptSelected(event: any) {
			if (event && event.item) {
				this.watchlistScriptEntity.scriptId = event.item.id;
				this.selectedScript = event.item; 
			}
		}

		sortData(column: keyof WatchlistScriptTabModel) {
			this.watchlistScriptTabModel = this.commonService.sortGrid(
				this.watchlistScriptTabModel,
				column,
				this.sortColumn,
				this.sortDirection
			);
		
			if (this.sortColumn === column) {
				this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
			} else {
				this.sortColumn = column;
				this.sortDirection = 'asc';
			}
		}
		

		public fetchData(): void {
			this.watchlistService.getTabRecord().subscribe({
				next: (data) => {
					this.watchlist = data;
					if (this.watchlist.length > 0) {
						this.activeTabId = this.watchlist[0].id;
						this.setActiveTab(this.activeTabId);
					} else {
						this.createDefaultTab();
					}
				},
				error: (err) => {
					console.error('Error fetching watchlist:', err);
				}
			});
		}
		
		public createDefaultTab(): void {
			const defaultWatchlist = new WatchlistMainModel();
			defaultWatchlist.name = 'Default';
		
			this.watchlistService.createWatchlist(defaultWatchlist).subscribe({
				next: (newWatchlist) => {
					this.watchlist.push(newWatchlist);
					this.activeTabId = newWatchlist.id;
					this.setActiveTab(this.activeTabId);
					this.toastr.success('Default watchlist created successfully.');
				},
				error: (err) => {
					console.error('Error creating default watchlist:', err);
					this.toastr.error('Failed to create default watchlist.');
				}
			});
		}

		public scriptSave(): void {
			this.watchlistScriptEntity.watchListId = this.activeTabId;
			if (this.watchlistScriptEntity.scriptId) {
				this.watchlistService.insertScript(this.watchlistScriptEntity).subscribe({
				next: (data) => {
					this.toastr.success('Script added successfully');
				},
				error: (err) => {
					console.error('Error inserting script:', err);
					this.toastr.error('Failed to add script');
				}
				});
			} else {
				this.toastr.warning('Please select a script');
			}
			window.location.reload();
		}

		public add(): void {
			if (!this.access.canInsert) {
				this.toastr.warning('You do not have add access of this page.');
				return;
			}

			this.router.navigate(['/app/watchlist/add']);
		}

		public fillDropDown(): void{
			this.scriptService.getForLOV(this.scriptParameterModel).subscribe(data=>{
					this.scriptListModel.scripts = data ;
			})
		}

		public setActiveTab(tabId: number): void {
			this.watchlistScriptTabModel = [];
			this.activeTabId = tabId;
			this.watchlistParameterModel.watchListId = tabId;

			this.getTabScriptReportData();

			if (this.refreshInterval) {
				clearInterval(this.refreshInterval);
			}
			this.refreshInterval = setInterval(() => {
				this.getTabScriptReportData();
			}, 100000); 
		}

		public getTabScriptReportData(): void {
			this.watchlistService.getTabScriptReport(this.watchlistParameterModel).subscribe(data => {

				
				if (!this.watchlistScriptTabModel  || this.watchlistScriptTabModel.length == 0)
					this.watchlistScriptTabModel = data;

				else {
					for(let i= 0; i < data.length; i++) {
						let records = this.watchlistScriptTabModel.filter(x => x.scriptId == data[i].scriptId);
						if (records.length > 0) {
							records[0].oldPrice = records[0].price;
							records[0].price = data[i].price;
							records[0].name = data[i].name;
							records[0].open = data[i].open;
							records[0].close = data[i].close;
							records[0].high = data[i].high;
							records[0].low = data[i].low;
							records[0].volume = data[i].volume;
							records[0].high52Week = data[i].high52Week;
							records[0].low52Week = data[i].low52Week;
						}
						else 
							this.watchlistScriptTabModel.push(data[i]);
					}
				}
			});
		}

		public delete(id: number): void {
			if (!this.access.canDelete) {
				this.toastr.warning('You do not have delete access to this page.');
				return;
			}
			if (window.confirm('Are you sure you want to delete?')) {
				this.watchlistService.delete(id).subscribe({
					next: () => {
						this.toastr.success('Record deleted successfully.');
						this.setActiveTab(this.activeTabId); // Refresh the list 
					},
					error: (err) => {
						console.error('Error deleting record:', err);
						this.toastr.error('Failed to delete record.');
					}
				});
			}
		}

		public redirect(id: number, symbol:string){
			this.commonService.redirectToPage(id,symbol);
		}
		
		public setAccess(): void {
			this.access = this.sessionService.getAccess("watchlist");
		}
		
}
