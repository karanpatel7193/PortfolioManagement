
export class ScriptViewOverviewModel {
    public scriptId: number = 0;
    public scriptName: string = "";
    public nseCode: string = "";
    public bseCode: number = 0; 
    public price: number = 0;
    public previousDay: number = 0;
    public open: number = 0;
    public close: number = 0;
    public high: number = 0;
    public low: number = 0;
    public volume: number = 0;
    public value: number = 0;
    public high52Week: number = 0;
    public low52Week: number = 0;
    public faceValue: number = 0; // Fixed capitalization for consistency
    public upperCircuitLimit: number = 0; // Added for UC Limit
    public lowerCircuitLimit: number = 0; // Added for LC Limit
    public allTimeHigh: number = 0; // Added for All Time High
    public allTimeLow: number = 0; // Added for All Time Low
    public avgVolume20D: number = 0; // Added for 20D Avg Volume
    public avgDeliveryPercentage: number = 0; // Added for 20D Avg Delivery(%)
    public bookValuePerShare: number = 0; // Added for Book Value Per Share
    public dividendYield: number = 0; // Added for Dividend Yield
    public ttmEps: number = 0; // Added for TTM EPS
    public epsChange: number = 0; // Added for EPS change percentage
    public ttmPe: number = 0; // Added for TTM PE
    public pb: number = 0; // Added for P/B ratio
    public sectorPe: number = 0; // Added for Sector PE
}
