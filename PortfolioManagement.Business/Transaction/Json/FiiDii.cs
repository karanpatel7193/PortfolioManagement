namespace StockMarketBusiness.Transaction.Json
{
    public class FiiDii
    {
        public FiiDiiRecord[] fiiDiiRecords { get; set; }
    }

    public class FiiDiiRecord
    {
        public string category { get; set; }
        public string date { get; set; }
        public string buyValue { get; set; }
        public string sellValue { get; set; }
        public string netValue { get; set; }
    }

}
