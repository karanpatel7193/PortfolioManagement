using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.Index
{
    public class HeaderEntity
    {
        public double Sensex { get; set; } = 0;
        public double SensexPercentage { get; set; } = 0;
        public double Nifty { get; set; } = 0;
        public double NiftyPercentage { get; set; } = 0;
        public double NiftyDiff { get; set; } = 0;
        public double SensexDiff { get; set; } = 0;

    }
    public class HeaderNifty50Entity
    {
        public string NseCode { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        public double PriceChange { get; set; } = 0;
        public double PricePercentage { get; set; } = 0;
        public int ScriptId { get; set; } = 0;
    }
    public class HeaderGridEntity
    {
        public List<HeaderNifty50Entity> Nifty50 { get; set; }  = new List<HeaderNifty50Entity>();
    }
}
