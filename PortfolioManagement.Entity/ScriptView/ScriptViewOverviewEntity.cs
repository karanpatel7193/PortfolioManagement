using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.ScriptView
{
    public class ScriptViewOverviewEntity
    {
        public int ScriptId { get; set; } = 0;
        public string ScriptName { get; set; }=string.Empty;
        public string NseCode { get; set; } = string.Empty;
        public decimal BseCode { get; set; } = 0;
        public double Price { get; set; } = 0;
        public double PreviousDay { get; set; } = 0;
        public double Open { get; set; } = 0;
        public double Close { get; set; } = 0;
        public double High { get; set; } = 0;
        public double Low { get; set; } = 0;
        public double Volume { get; set; } = 0;
        public double Value { get; set; } = 0;
        public double High52Week { get; set; } = 0;
        public double Low52Week { get; set; } = 0;

        // New properties based on the provided data
        public double FaceValue { get; set; } = 0; 
        public double UpperCircuitLimit { get; set; } = 0; 
        public double LowerCircuitLimit { get; set; } = 0; 
        public double AllTimeHigh { get; set; } = 0; 
        public double AllTimeLow { get; set; } = 0; 
        public double AvgVolume20D { get; set; } = 0; 
        public double AvgDeliveryPercentage { get; set; } = 0; 
        public double BookValuePerShare { get; set; } = 0; 
        public double DividendYield { get; set; } = 0; 
        public double TtmEps { get; set; } = 0; 
        public double EpsChange { get; set; } = 0; 
        public double TtmPe { get; set; } = 0; 
        public double Pb { get; set; } = 0; 
        public double SectorPe { get; set; } = 0;
    }
}
