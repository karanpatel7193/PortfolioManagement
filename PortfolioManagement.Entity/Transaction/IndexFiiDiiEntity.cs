using PortfolioManagement.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.Transaction
{
    public class IndexFiiDiiEntity
    {
        public DateTime Date { get; set; } = DateTime.MinValue;
        public double Nifty { get; set; } = 0;
        public double Sensex { get; set; }= 0;
        public double FII { get; set; } = 0;
        public double DII { get; set; } = 0;
    }

    public class IndexFiiDiiParameterEntity
    {
        public string DateRange { get; set; } = string.Empty;
        public DateTime TodayDate { get; set; } = DateTime.UtcNow;

    }
    public class IndexFiiDiiChartEntity
    {
        public List<DateTime> Dates { get; set; } = new List<DateTime>();
        public List<double> NiftySeriesData { get; set; } = new List<double>();
        public List<double> SensexSeriesData { get; set; } = new List<double>();
        public List<double> FIISeriesData { get; set; } = new List<double>();
        public List<double> DIISeriesData { get; set; } = new List<double>();

    }
}
