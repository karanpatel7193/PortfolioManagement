using System;
using System.Text.Json.Serialization;

namespace PortfolioManagement.Entity.Transaction
{
    /// <summary>
    /// This class having entities of table Protfolio
    /// Created By :: Rekansh Patel
    /// Created On :: 12/14/2020
    /// </summary>
    public class ProtfolioEntity
	{
		#region Constructor
		/// <summary>
		/// This construction is set properties default value based on its data type in table.
		/// </summary>
		public ProtfolioEntity()
		{
			SetDefaulValue();
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Get & Set Id
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Get & Set Account Id
		/// </summary>
		public int AccountId { get; set; }

		/// <summary>
		/// Get & Set Script Id
		/// </summary>
		public int ScriptId { get; set; }

		/// <summary>
		/// Get & Set Buy Qty
		/// </summary>
		public int BuyQty { get; set; }

		/// <summary>
		/// Get & Set Buy Price
		/// </summary>
		public double BuyPrice { get; set; }

		/// <summary>
		/// Get & Set Buy Brokrage
		/// </summary>
		public double BuyBrokrage { get; set; }

		/// <summary>
		/// Get & Set Sell Qty
		/// </summary>
		public int SellQty { get; set; }

		/// <summary>
		/// Get & Set Sell Price
		/// </summary>
		public double SellPrice { get; set; }

		/// <summary>
		/// Get & Set Sell Brokrage
		/// </summary>
		public double SellBrokrage { get; set; }

		/// <summary>
		/// Get & Set Profit Lost
		/// </summary>
		public double ProfitLost { get; set; }

		public string NseCode { get; set; }
		public string ScriptName { get; set; }
		public double BuyTotal { get; set; }
		public double SellTotal { get; set; }

		public long TransactionId { get; set; }

		public DateTime BuyDate { get; set; }
		#endregion

		#region Private Methods
		/// <summary>
		/// This function is set properties default value based on its data type in table.
		/// </summary>
		private void SetDefaulValue()
		{
			Id = 0;
			AccountId = 0;
			ScriptId = 0;
			BuyQty = 0;
			BuyPrice = 0;
			BuyBrokrage = 0;
			SellQty = 0;
			SellPrice = 0;
			SellBrokrage = 0;
			ProfitLost = 0;
			NseCode = string.Empty;
			ScriptName = string.Empty;
			BuyTotal = 0;
			SellTotal = 0;
			TransactionId = 0;
			BuyDate = DateTime.MinValue;
		}
		#endregion
	}

	public class ProtfolioParameterEntity : PagingSortingEntity
	{
		#region Constructor
		/// <summary>
		/// This construction is set properties default value based on its data type in table.
		/// </summary>
		public ProtfolioParameterEntity()
		{
			SetDefaulValue();
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Get & Set Id
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Get & Set Account Id
		/// </summary>
		public int AccountId { get; set; }

		/// <summary>
		/// Get & Set Script Id
		/// </summary>
		public int ScriptId { get; set; }

		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public bool GroupByScript { get; set; }

		#endregion

		#region Private Methods
		/// <summary>
		/// This function is set properties default value based on its data type in table.
		/// </summary>
		private void SetDefaulValue()
		{
			Id = 0;
			AccountId = 0;
			ScriptId = 0;
			FromDate = DateTime.MinValue;
			ToDate = DateTime.MinValue;
			GroupByScript = false;
		}
		#endregion
	}

    #region Portfolio Report Entities

    public class PortfolioSummaryEntity
    {
        public double TotalInvestmentAmount { get; set; } = 0;
        public double TotalMarketAmount { get; set; } = 0;
        public double OverallGLAmount { get; set; } = 0;
        public double OverallGLPercentage { get; set; } = 0;
        public double DayGLAmount { get; set; } = 0;
        public double DayGLPercentage { get; set; } = 0;
        public double ReleasedProfit { get; set; } = 0;


    }

    public class PortfolioScriptEntity
    {
        public byte AccountId { get; set; } = 0;
        public string AccountName { get; set; } = string.Empty;
        public short ScriptId { get; set; } = 0;
        public string ScriptName { get; set; } = string.Empty;
        public string IndustryName { get; set; } = string.Empty;
        public byte BrokerId { get; set; } = 0;
        public string BrokerName { get; set; } = string.Empty;
        public int Qty { get; set; } = 0;
        public double CostPrice { get; set; } = 0;
        public double CurrentPrice { get; set; } = 0;
        public double PreviousDayPrice { get; set; } = 0;
        public double InvestmentAmount { get; set; } = 0;
        public double MarketValue { get; set; } = 0;
        public double OverallGLAmount { get; set; } = 0;
        public double OverallGLPercentage { get; set; } = 0;
        public double DayGLAmount { get; set; } = 0;
        public double DayGLPercentage { get; set; } = 0;
        public double ReleasedProfit { get; set; } = 0;
        
    }

    public class PortfolioSectorEntity
	{
		public string SectorName { get; set; } = string.Empty;
		public double Percentage { get; set; } = 0;
		[JsonIgnore]
		public double Amount { get; set; } = 0;
	}

    public class PortfolioReportEntity
	{
		public PortfolioSummaryEntity PortfolioSummary { get; set; } = new PortfolioSummaryEntity();
        public List<PortfolioScriptEntity> Scripts { get; set; } = new List<PortfolioScriptEntity>();
		public List<PortfolioSectorEntity> InvestmentSectors { get; set; } = new List<PortfolioSectorEntity>();
		public List<PortfolioSectorEntity> MarketSectors { get; set; } = new List<PortfolioSectorEntity>();

    }
    #endregion Portfolio Report Entities
}
