using PortfolioManagement.Entity.Master;
using PortfolioManagement.Entity.Transaction.StockTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.Watchlist
{
    public class WatchlistMainEntity
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
    }

    public class WatchlistEntity : WatchlistMainEntity
    {
        [JsonIgnore]
        public int PmsId { get; set; } = 0;
    }
    public class WatchlistScriptEntity
    {
        public int Id { get; set; } = 0;
        public int WatchlistId { get; set; } = 0;
        public int ScriptId { get; set; } = 0;
    
    }

    public class WatchlistScriptTabEntity : WatchlistScriptEntity
    {
        public string Name { get; set; } = string.Empty;
        public string NseCode { get; set; } = string.Empty;
        public double Open { get; set; } = 0;
        public double Close { get; set; } = 0;
        public double High { get; set; } = 0;
        public double Low { get; set; } = 0;
        public int Volume { get; set; } = 0;
        public double Price { get; set; } = 0;
        public double High52Week { get; set; } = 0;
        public double Low52Week { get; set; } = 0;

    }

    public class WatchlistParameterEntity
    {
        public int Id { get; set; } = 0;
        public int WatchListId { get; set; } = 0;
        public int ScriptId { get; set; } = 0;

        [JsonIgnore]
        public int PmsId { get; set; } = 0;
    }


}

