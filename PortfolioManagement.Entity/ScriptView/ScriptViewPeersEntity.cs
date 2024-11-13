using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.ScriptView
{
    public class ScriptViewPeersEntity
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        public decimal PercentageChange { get; set; } = 0;
        public decimal MarketCap { get; set; } = 0;
        public decimal TTMPE { get; set; } = 0;
        public decimal PB { get; set; } = 0;
        public decimal ROE { get; set; } = 0;        
        public decimal OneYearPerformance { get; set; } = 0; 
        public decimal NetProfit { get; set; } = 0; 
        public decimal NetSales { get; set; } = 0; 
        public decimal DebtToEquity { get; set; } = 0;       

    }

}
