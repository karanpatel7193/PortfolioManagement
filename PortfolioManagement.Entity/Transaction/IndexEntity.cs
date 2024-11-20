using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioManagement.Entity.Transaction
{
	/// <summary>
	/// This class having entities of table Index
	/// Created By :: Rekansh Patel
	/// Created On :: 11/22/2020
	/// </summary>
	public class IndexEntity : IndexFiiDiiEntity
    {
		#region Constructor
		/// <summary>
		/// This construction is set properties default value based on its data type in table.
		/// </summary>
		public IndexEntity()
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
		/// Get & Set Date
		/// </summary>
		public DateTime Date { get; set; }

        public double Nifty { get; set; }
		public double Sensex { get; set; } 

        /// <summary>
        /// Get & Set Sensex Previous Day
        /// </summary>
        public double SensexPreviousDay { get; set; }

		/// <summary>
		/// Get & Set Sensex Open
		/// </summary>
		public double SensexOpen { get; set; }

		/// <summary>
		/// Get & Set Sensex Close
		/// </summary>
		public double SensexClose { get; set; }

		/// <summary>
		/// Get & Set Sensex High
		/// </summary>
		public double SensexHigh { get; set; }

		/// <summary>
		/// Get & Set Sensex Low
		/// </summary>
		public double SensexLow { get; set; }

		/// <summary>
		/// Get & Set Nifty Previous Day
		/// </summary>
		public double NiftyPreviousDay { get; set; }

		/// <summary>
		/// Get & Set Nifty Open
		/// </summary>
		public double NiftyOpen { get; set; }

		/// <summary>
		/// Get & Set Nifty Close
		/// </summary>
		public double NiftyClose { get; set; }

		/// <summary>
		/// Get & Set Nifty High
		/// </summary>
		public double NiftyHigh { get; set; }

		/// <summary>
		/// Get & Set Nifty Low
		/// </summary>
		public double NiftyLow { get; set; }

		/// <summary>
		/// Get & Set FII
		/// </summary>
		public double FII { get; set; }

		/// <summary>
		/// Get & Set DII
		/// </summary>
		public double DII { get; set; }
		#endregion

		#region Private Methods
		/// <summary>
		/// This function is set properties default value based on its data type in table.
		/// </summary>
		private void SetDefaulValue()
		{
			Id = 0;
			Date = DateTime.MinValue;
			Sensex = 0;
			Nifty = 0;
			SensexPreviousDay = 0;
			SensexOpen = 0;
			SensexClose = 0;
			SensexHigh = 0;
			SensexLow = 0;
			NiftyPreviousDay = 0;
			NiftyOpen = 0;
			NiftyClose = 0;
			NiftyHigh = 0;
			NiftyLow = 0;
			FII = 0;
			DII = 0;
		}
		#endregion
	}

	public class IndexParameterEntity
	{
		#region Constructor
		/// <summary>
		/// This construction is set properties default value based on its data type in table.
		/// </summary>
		public IndexParameterEntity()
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
			Id = 0;
			Date = DateTime.MinValue;
		}
		#endregion
	}
}
