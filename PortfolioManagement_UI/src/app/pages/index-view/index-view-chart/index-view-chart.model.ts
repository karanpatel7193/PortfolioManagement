export class IndexViewModel {
    date: Date = new Date();
    sensexPreviousDay: number = 0;
    sensexOpen: number = 0;
    sensexClose: number = 0;
    sensexHigh: number = 0;
    sensexLow: number = 0;
    niftyPreviousDay: number = 0;
    niftyOpen: number = 0;
    niftyClose: number = 0;
    niftyHigh: number = 0;
    niftyLow: number = 0;
    fii: number = 0;
    dii: number = 0;
    sensex: number = 0;
    nifty: number = 0;
}

export class IndexViewParameterModel {
    public fromDate: Date = new Date('2024-08-08');
    // public fromDate: Date = new Date(0);
    public toDate: Date = new Date('2024-12-12');
    // public toDate: Date = new Date(0);
}

export class IndexViewChartModel {
    indexes: IndexViewModel[] = [];
    dates: Date[] = [];
    sensexSeriesData: number[][] = [];
    niftySeriesData: number[][] = [];
    fiiSeriesData: number[] = [];
    diiSeriesData: number[] = [];
}
