namespace StockMarketBusiness.Transaction.Json
{
    public class Quote
    {
        public Info info { get; set; }
        public QuoteMetadata metadata { get; set; }
        public Securityinfo securityInfo { get; set; }
        public Priceinfo priceInfo { get; set; }
        public Preopenmarket preOpenMarket { get; set; }
        public Surveillance surveillance { get; set; }

    }

    public class Info
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string industry { get; set; }
        public string[] activeSeries { get; set; }
        public object[] debtSeries { get; set; }
        public object[] tempSuspendedSeries { get; set; }
        public bool isFNOSec { get; set; }
        public bool isCASec { get; set; }
        public bool isSLBSec { get; set; }
        public bool isDebtSec { get; set; }
        public bool isSuspended { get; set; }
        public bool isETFSec { get; set; }
        public bool isDelisted { get; set; }
        public string isin { get; set; }
        public bool isTop10 { get; set; }
        public string identifier { get; set; }
    }

    public class QuoteMetadata
    {
        public string series { get; set; }
        public string symbol { get; set; }
        public string isin { get; set; }
        public string status { get; set; }
        public string listingDate { get; set; }
        public string industry { get; set; }
        public string lastUpdateTime { get; set; }
        public object pdSectorPe { get; set; }
        public object pdSymbolPe { get; set; }
        public string pdSectorInd { get; set; }
    }

    public class Securityinfo
    {
        public string boardStatus { get; set; }
        public string tradingStatus { get; set; }
        public string tradingSegment { get; set; }
        public string sessionNo { get; set; }
        public string slb { get; set; }
        public string classOfShare { get; set; }
        public string derivatives { get; set; }
        //public object surveillance { get; set; }
        public int faceValue { get; set; }
        public long issuedCap { get; set; }
    }
    public class Surveillance
    {
        public bool surv { get; set; }
        public bool desc { get; set; }

    }
    public class Priceinfo
    {
        public double lastPrice { get; set; }
        public double change { get; set; }
        public double pChange { get; set; }
        public double previousClose { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public double vwap { get; set; }
        public string lowerCP { get; set; }
        public string upperCP { get; set; }
        public string pPriceBand { get; set; }
        public double basePrice { get; set; }
        public Intradayhighlow intraDayHighLow { get; set; }
        public Weekhighlow weekHighLow { get; set; }
    }

    public class Intradayhighlow
    {
        public double min { get; set; }
        public double max { get; set; }
        public double value { get; set; }
    }

    public class Weekhighlow
    {
        public double min { get; set; }
        public string minDate { get; set; }
        public double max { get; set; }
        public string maxDate { get; set; }
        public double value { get; set; }
    }

    public class Preopenmarket
    {
        public Preopen[] preopen { get; set; }
        public Ato ato { get; set; }
        public double IEP { get; set; }
        public object totalTradedVolume { get; set; }
        public double finalPrice { get; set; }
        public int finalQuantity { get; set; }
        public string lastUpdateTime { get; set; }
        public int totalBuyQuantity { get; set; }
        public int totalSellQuantity { get; set; }
        public int atoBuyQty { get; set; }
        public int atoSellQty { get; set; }
    }

    public class Ato
    {
        public int buy { get; set; }
        public int sell { get; set; }
    }

    public class Preopen
    {
        public double price { get; set; }
        public int buyQty { get; set; }
        public int sellQty { get; set; }
        public bool iep { get; set; }
    }

}
