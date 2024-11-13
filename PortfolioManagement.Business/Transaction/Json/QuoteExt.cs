namespace StockMarketBusiness.Transaction.Json
{
    public class QuoteExt
    {
        public bool noBlockDeals { get; set; }
        public Bulkblockdeal[] bulkBlockDeals { get; set; }
        public Marketdeptorderbook marketDeptOrderBook { get; set; }
        public Securitywisedp securityWiseDP { get; set; }
    }

    public class Marketdeptorderbook
    {
        public int totalBuyQuantity { get; set; }
        public int totalSellQuantity { get; set; }
        public Bid[] bid { get; set; }
        public Ask[] ask { get; set; }
        public Tradeinfo tradeInfo { get; set; }
        public Valueatrisk valueAtRisk { get; set; }
    }

    public class Tradeinfo
    {
        public double totalTradedVolume { get; set; }
        public double totalTradedValue { get; set; }
        public double totalMarketCap { get; set; }
        public double ffmc { get; set; }
        public double impactCost { get; set; }
    }

    public class Valueatrisk
    {
        public double securityVar { get; set; }
        public int indexVar { get; set; }
        public double varMargin { get; set; }
        public double extremeLossMargin { get; set; }
        public int adhocMargin { get; set; }
        public double applicableMargin { get; set; }
    }

    public class Bid
    {
        public double price { get; set; }
        public int quantity { get; set; }
    }

    public class Ask
    {
        public double price { get; set; }
        public int quantity { get; set; }
    }

    public class Securitywisedp
    {
        public int quantityTraded { get; set; }
        public int deliveryQuantity { get; set; }
        public double deliveryToTradedQuantity { get; set; }
        public object seriesRemarks { get; set; }
        public string secWiseDelPosDate { get; set; }
    }

    public class Bulkblockdeal
    {
        public string name { get; set; }
    }
}
