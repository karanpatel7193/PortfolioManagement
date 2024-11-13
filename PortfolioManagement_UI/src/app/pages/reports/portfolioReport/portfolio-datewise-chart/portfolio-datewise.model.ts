export class PortfolioDatewiseModel {
    public brokerId: number = 0;
    public accountId: number = 0;
    public date: Date = new Date(0);
    public investmentAmount: number = 0;
    public unReleasedAmount: number = 0;
}
  
export class PortfolioDatewiseReportModel {
    public date: Date = new Date(0);
    public totalInvestmentAmount: number = 0;
    public totalUnReleasedAmount: number = 0;

    public timeSeries: any[] = []; 
    public investmentSeries: number[] = [];
    public marketValueSeries: number[] = [];
}
  
export class PortfolioDatewiseParameterModel {
    public brokerId: number = 0; 
    public accountId: number = 0;
    public fromDate: Date = new Date();
    public toDate: Date = new Date();
}
  