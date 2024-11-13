using PortfolioManagement.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.ScriptView
{
    public class ScriptViewRangeEntity
    {
        public int ScriptId { get; set; } = 0;
        public string ScriptName { get; set; } = string.Empty;
        public string NseCode { get; set; } = string.Empty;
        public string IndustryName { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        public double PriceChange { get; set; } = 0;
        public double PricePercentage { get; set; } = 0;
        public double PreviousDay { get; set; } = 0;
        public double High { get; set; } = 0;
        public double Low { get; set; } = 0;
        public double Volume { get; set; } = 0;
        public double Value { get; set; } = 0;
        public double High52Week { get; set; } = 0;
        public double Low52Week { get; set; } = 0;
        public double Bid { get; set; } = 0;
        public double Ask { get; set; } = 0;
        public DateTime DateTime { get; set; } = DateTime.MinValue;
        public DateTime CurrentDate { get; set; } = DateTime.UtcNow;

    }
 
    public class ScriptViewRangeChartEntity
    {
        public ScriptViewRangeEntity Script { get; set; } = new ScriptViewRangeEntity();
        public List<ScriptViewRangeEntity> DayPrices { get; set; } = new List<ScriptViewRangeEntity>();


        public List<object[]> TimeSeries { get; set; } = new List<object[]>();
        public List<double> PriceSeriesData { get; set; } = new List<double>();
        public List<double> VolumeSeriesData { get; set; } = new List<double>();
    }
}
