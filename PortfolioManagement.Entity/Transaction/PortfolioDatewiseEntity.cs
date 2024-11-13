using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.Transaction
{
    public class PortfolioDatewiseEntity
    {
        [JsonIgnore]
        public int PmsId { get; set; } = 0;
        public short BrokerId { get; set; } = 0;
        public short AccountId { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.MinValue;
        public double InvestmentAmount { get; set; } = 0;
        public double UnReleasedAmount { get; set; } = 0;
    }
    public class PortfolioDatewiseReportEntity
    {
        public DateTime Date { get; set; } = DateTime.MinValue;
        public double TotalInvestmentAmount { get; set; } = 0;
        public double TotalUnReleasedAmount { get; set; } = 0;

        public List<string> TimeSeries { get; set; } = new List<string>();
        public List<double> InvestmentSeries { get; set; } = new List<double>();
        public List<double> MarketValueSeries{ get; set; } = new List<double>();
    }
    public class PortfolioDatewiseParameterEntity
    {
        [JsonIgnore]
        public int PmsId { get; set; } = 0;
        public short BrokerId { get; set; } = 0;
        public short AccountId { get; set; } = 0;
        public DateTime FromDate { get; set; } = DateTime.MinValue;
        public DateTime ToDate { get; set; } = DateTime.MinValue;
    }
}
