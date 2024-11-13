using CommonLibrary;
using System;

namespace PortfolioManagement.Entity.Transaction
{
    /// <summary>
    /// This class having entities of table ScriptDaySummary
    /// Created By :: Rekansh Patel
    /// Created On :: 11/22/2020
    /// </summary>
    public class ScriptDaySummaryEntity
	{
		#region Constructor
		/// <summary>
		/// This construction is set properties default value based on its data type in table.
		/// </summary>
		public ScriptDaySummaryEntity()
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
		/// Get & Set Script Id
		/// </summary>
		public int ScriptId { get; set; }

		/// <summary>
		/// Get & Set Script Name
		/// </summary>
		public string ScriptName { get; set; }

		/// <summary>
		/// Get & Set Date
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Get & Set Previous Day
		/// </summary>
		public double PreviousDay { get; set; }

		/// <summary>
		/// Get & Set Open
		/// </summary>
		public double Open { get; set; }

		/// <summary>
		/// Get & Set Close
		/// </summary>
		public double Close { get; set; }

		/// <summary>
		/// Get & Set High
		/// </summary>
		public double High { get; set; }

		/// <summary>
		/// Get & Set Low
		/// </summary>
		public double Low { get; set; }

		/// <summary>
		/// Get & Set Date Time
		/// </summary>
		public DateTime DateTime { get; set; }

		/// <summary>
		/// Get & Set Price
		/// </summary>
		public double Price { get; set; }

		/// <summary>
		/// Get & Set Volume
		/// </summary>
		public long Volume { get; set; }

        /// <summary>
        /// Get & Set Value
        /// </summary>
        public double Value { get; set; }

		/// <summary>
		/// Get & Set High52Week
		/// </summary>
		public double High52Week { get; set; }

		/// <summary>
		/// Get & Set Low52Week
		/// </summary>
		public double Low52Week { get; set; }
		#endregion

		#region Private Methods
		/// <summary>
		/// This function is set properties default value based on its data type in table.
		/// </summary>
		private void SetDefaulValue()
		{
			Id = 0;
			ScriptId = 0;
			ScriptName = string.Empty;
			Date = DateTime.MinValue;
			PreviousDay = 0;
			Open = 0;
			Close = 0;
			High = 0;
			Low = 0;
			DateTime = DateTime.MinValue;
			Price = 0;
			Volume = 0;
			Value = 0;
			High52Week = 0;
			Low52Week = 0;
		}
		#endregion
	}

	public class ScriptDayParameterEntity
	{
		#region Constructor
		/// <summary>
		/// This construction is set properties default value based on its data type in table.
		/// </summary>
		public ScriptDayParameterEntity()
		{
			SetDefaulValue();
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Get & Set Script Id
		/// </summary>
		public int ScriptId { get; set; }

		/// <summary>
		/// Get & Set Date
		/// </summary>
		public DateTime Date { get; set; }

		#endregion

		#region Private Methods
		/// <summary>
		/// This function is set properties default value based on its data type in table.
		/// </summary>
		private void SetDefaulValue()
		{
			ScriptId = 0;
			Date = DateTime.MinValue;
		}
		#endregion
	}
}
