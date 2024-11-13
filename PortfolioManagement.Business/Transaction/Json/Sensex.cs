using System;

namespace StockMarketBusiness.Transaction.Json
{
    public class Sensex
    {
        public Realtime[] RealTime { get; set; }
        public ASON[] ASON { get; set; }
        public EOD[] EOD { get; set; }
    }

    public class Realtime
    {
        public int ScripFlagCode { get; set; }
        public string INDX_CD { get; set; }
        public string IndexName { get; set; }
        public double I_open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Curvalue { get; set; }
        public double Prev_Close { get; set; }
        public double Chg { get; set; }
        public double ChgPer { get; set; }
        public double Week52High { get; set; }
        public double Week52Low { get; set; }
        public double MKTCAP { get; set; }
        public double MktcapPerc { get; set; }
        public double NET_TURN { get; set; }
        public double TurnoverPerc { get; set; }
        public DateTime DT_TM { get; set; }
        public string WebURL { get; set; }
    }

    public class ASON
    {
        public int ScripFlagCode { get; set; }
        public string INDX_CD { get; set; }
        public string IndexName { get; set; }
        public double I_open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Curvalue { get; set; }
        public double Prev_Close { get; set; }
        public double Chg { get; set; }
        public double ChgPer { get; set; }
        public double Week52High { get; set; }
        public double Week52Low { get; set; }
        public double MKTCAP { get; set; }
        public double MktcapPerc { get; set; }
        public double NET_TURN { get; set; }
        public double TurnoverPerc { get; set; }
        public DateTime DT_TM { get; set; }
        public string WebURL { get; set; }
    }

    public class EOD
    {
        public string IndicesWatchName { get; set; }
        public string INDEX_CODE { get; set; }
        public double Curvalue { get; set; }
        public double PrevDayClose { get; set; }
        public double CHNG { get; set; }
        public double CHNGPER { get; set; }
        public DateTime DT_TM { get; set; }
        public string WebURL { get; set; }
        public double IndexSrNo { get; set; }
    }
}
