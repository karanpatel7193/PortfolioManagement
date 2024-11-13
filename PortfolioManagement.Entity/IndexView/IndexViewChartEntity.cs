using PortfolioManagement.Entity.ScriptView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.IndexView
{
    public class IndexViewEntity
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public double SensexPreviousDay { get; set; } = 0;
        public double SensexOpen { get; set; } = 0;
        public double SensexClose { get; set; } = 0;
        public double SensexHigh { get; set; } = 0;
        public double SensexLow { get; set; } = 0;

        public double NiftyPreviousDay { get; set; } = 0;
        public double NiftyOpen { get; set; } = 0;
        public double NiftyClose { get; set; } = 0;
        public double NiftyHigh { get; set; } = 0;
        public double NiftyLow { get; set; } = 0;

        public double FII { get; set; } = 0;
        public double DII { get; set; } = 0;

        public double Sensex { get; set; } = 0;
        public double Nifty { get; set; } = 0;
    }
    public class IndexViewChartEntity
    {
        public List<DateTime> Dates { get; set; } = new List<DateTime>();
        public List<double[]> SensexSeriesData { get; set; } = new List<double[]>();
        public List<double[]> NiftySeriesData { get; set; } = new List<double[]>();
        public List<double> FiiSeriesData { get; set; } = new List<double>();
        public List<double> DiiSeriesData { get; set; } = new List<double>();
    }
}
