export class IndexFiiDiiModel {
    date: Date = new Date(0); 
    nifty: number = 0;
    sensex: number = 0;
    fii: number = 0;
    dii: number = 0;
}

export class IndexFiiDiiParameterModel {
    dateRange: string = '';   
    todayDate: Date = new Date(new Date().setUTCHours(0, 0, 0, 0));
}

export class IndexFiiDiiChartModel {
    dates: Date[] = [];
    niftySeriesData: number[] = [];
    sensexSeriesData: number[] = [];
    fiiSeriesData: number[] = [];
    diiSeriesData: number[] = [];
}
