export class IndexChartModel {
    date: Date = new Date();  
    sensex: number = 0;      
    nifty: number = 0;       
  }
  
  export class IndexChartGridModel {
    dates: Date[] = [];
    sensexSeriesData: number[] = [];
    niftySeriesData: number[] = [];
  }
  
  export class IndexChartParameterModel {
    dateRange: string = '';   
    todayDate: Date = new Date();
  }
  