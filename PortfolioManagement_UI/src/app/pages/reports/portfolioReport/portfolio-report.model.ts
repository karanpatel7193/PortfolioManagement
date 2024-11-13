export class PortfolioSummaryModel {
    totalInvestmentAmount: number = 0;
    totalMarketAmount: number = 0;
    overallGLAmount: number = 0;
    overallGLPercentage: number = 0;
    dayGLAmount: number = 0;
    dayGLPercentage: number = 0;
    releasedProfit: number = 0;
}

export class PortfolioScriptModel {
    accountId: number = 0; 
    accountName: string = '';
    scriptId: number = 0; 
    scriptName: string = '';
    industryName: string = '';
    brokerId: number = 0; 
    brokerName: string = '';
    qty: number = 0;
    costPrice: number = 0;
    currentPrice: number = 0;
    previousDayPrice: number = 0;
    investmentAmount: number = 0;
    marketValue: number = 0;
    overallGLAmount: number = 0;
    overallGLPercentage: number = 0;
    dayGLAmount: number = 0;
    dayGLPercentage: number = 0;
    releasedProfit: number = 0;
}

export class PortfolioSectorModel {
    sectorName: string = '';
    percentage: number = 0;
    amount: number = 0; 
}

export class PortfolioReportModel {
    portfolioSummary: PortfolioSummaryModel = new PortfolioSummaryModel();
    scripts: PortfolioScriptModel[] = [];
    investmentSectors: PortfolioSectorModel[] = [];
    marketSectors: PortfolioSectorModel[] = [];
}

export class PortfolioDateWiseModel {
    pmsId: number = 0;
    brokerId: number = 0; 
    accountId: number = 0;
    date: Date = new Date();
    investmentAmount: number = 0;
    unReleasedAmount: number = 0;
}

export class PortfolioDonutChartModel{
    value: number = 0;
    name: string = '';
}
