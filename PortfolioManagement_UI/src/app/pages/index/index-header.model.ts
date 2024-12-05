export class HeaderModel {
    public sensex: number = 0;
    public sensexPercentage: number = 0;
    public nifty: number = 0;
    public niftyPercentage: number = 0;
    public niftyDiff: number = 0;
    public sensexDiff: number = 0;
}

export class HeaderNifty50Model {
    public nseCode: string = '';
    public price: number = 0;
    public scriptId: number = 0;
    public priceChange: number = 0;
    public pricePercentage: number = 0;
}

export class HeaderGridModel {
    public nifty50: HeaderNifty50Model[] = [];
}
