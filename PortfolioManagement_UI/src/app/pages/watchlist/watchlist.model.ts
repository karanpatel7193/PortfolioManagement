import { PagingSortingModel } from '../../models/pagingsorting.model';
import { ScriptMainModel } from '../master/script/script.model';
import { StockTransactionGridModel } from '../transaction/stocktransaction/stocktransaction.model';


export class WatchlistMainModel{
    public id: number = 0;
    public name: string = '';
}

export class WatchlistModel extends WatchlistMainModel {
}
export class WatchlistScriptModel{
    id: number = 0;
    watchListId: number = 0;
    scriptId: number = 0;
}
export class WatchlistScriptTabModel {
    public id: number = 0;
    public watchListId: number = 0;
    public scriptId: number = 0;
    public name: string = '';
    public nseCode: string = '';
    public open: number = 0;
    public close: number = 0;
    public high: number = 0;
    public low: number = 0;
    public volume: number = 0;
    public price: number = 0;
    public oldPrice: number = 0;
    public high52Week: number = 0;
    public low52Week: number = 0;
}

export class WatchlistParameterModel  {
    public id: number = 0;
    public scriptId: number = 0;
    public watchListId: number = 0;
}

export class WatchlistListModel extends StockTransactionGridModel {
    public scripts: ScriptMainModel[] = [];
}

