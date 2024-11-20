using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.Index
{
    public class IndexChartEntity
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public double Sensex { get; set; } = 0;
        public double Nifty { get; set; } = 0;
    }
    public class IndexChartGridEntity
    {
        public List<DateTime> Dates { get; set; } = new List<DateTime>();
        public List<double> SensexSeriesData { get; set; } = new List<double>();
        public List<double> NiftySeriesData { get; set; } = new List<double>();
    }
    public class IndexChartParameterEntity
    {
        public string DateRange { get; set; } = string.Empty;
        public DateTime TodayDate { get; set; } = DateTime.UtcNow;

    }
}
