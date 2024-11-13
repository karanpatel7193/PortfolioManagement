export class ScriptViewRangeModel {
    public scriptId: number = 0;
    public scriptName: string = '';
    public nseCode: string = '';
    public industryName: string = '';
    public price: number = 0;
    public priceChange: number = 0;
    public pricePercentage: number = 0;
    public previousDay: number = 0;
    public high: number = 0;
    public low: number = 0;
    public volume: number = 0;
    public value: number = 0;
    public high52Week: number = 0;
    public low52Week: number = 0;
    public bid: number = 0;
    public ask: number = 0;
    public dateTime: Date = new Date(0); 
    public currentDate: Date = new Date(); 
}

export class ScriptViewRangeChartModel {
    public script: ScriptViewRangeModel = new ScriptViewRangeModel();
    public dayPrices: ScriptViewRangeModel[] = [];

    public timeSeries: any[] = []; 
    public priceSeriesData: number[] = [];
    public volumeSeriesData: number[] = [];
}
