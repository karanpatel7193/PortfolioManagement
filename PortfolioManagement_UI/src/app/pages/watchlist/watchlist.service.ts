import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/services/http.service';
import { WatchlistModel, WatchlistScriptModel, WatchlistScriptTabModel, WatchlistParameterModel, WatchlistMainModel } from './watchlist.model';
import {  map, Observable } from 'rxjs';

@Injectable({
providedIn: 'root'
})
export class WatchlistService {

	constructor(private http: HttpService) { }

    public getTabRecord(): Observable<WatchlistMainModel[]> {
        return this.http.post('watchlist/getTabValue',null).pipe(
            map((response: WatchlistMainModel[]) => response)
        );
    }
	public createWatchlist(watchlist: WatchlistMainModel): Observable<WatchlistMainModel> {
		return this.http.post('watchlist/insert', watchlist).pipe(
			map((response: WatchlistMainModel) => response)
		);
	}

	public getTabScriptReport(watchlistParameterModel: WatchlistParameterModel): Observable<WatchlistScriptTabModel[]> {
		return this.http.post('watchlist/getTabScriptData', watchlistParameterModel).pipe(
			map((response: WatchlistScriptTabModel[]) => {
				return response;
			}),
		);
	}
	
	

	public save (watchlist:WatchlistModel):Observable<number>{
		return this.http.post('watchlist/insert', watchlist).pipe(
			map((response: number)=>{
				return response;
			})
		)
	}

	public delete(id: number): Observable<void> {
		return this.http.post('watchlist/deleteScript/'+ id, null).pipe(
			map((response: void) => {
				return response;
			})
		);
	}

	public insertScript(watchlistScriptModel: WatchlistScriptModel): Observable<number> {
		return this.http.post('watchlist/insertscript', watchlistScriptModel).pipe(
			map((response: number) => {
				return response;
			}),
		);
};
}
