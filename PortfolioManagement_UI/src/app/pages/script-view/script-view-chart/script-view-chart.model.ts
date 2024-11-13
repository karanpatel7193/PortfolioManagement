import { ScriptMainModel } from "../../master/script/script.model";

export class ScriptViewPriceModel {
    public date: Date = new Date(0);
    public price: number = 0;
    public previousDay: number= 0 ;
    public open: number = 0;
    public close: number = 0;
    public high: number = 0;
    public low: number = 0;
    public volume: number = 0;
    public value: number = 0;
}

export class ScriptViewParameterModel {
    public fromDate: Date = new Date();
    public toDate: Date = new Date();
    public scriptId: number =  0;
}

export class ScriptViewChartModel
{
    public script: ScriptMainModel  = new ScriptMainModel();
    public prices: ScriptViewPriceModel[]  = [];

    public dates: Date[] = [];
    public candelSeriesData: number[][] = []; 
    public priceSeriesData: number[] = []; 
    public volumeSeriesData: number[] = []; 

}
