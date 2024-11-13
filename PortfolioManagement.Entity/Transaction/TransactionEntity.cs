using PortfolioManagement.Entity.Master;
using System;
using System.Collections.Generic;

namespace PortfolioManagement.Entity.Transaction
{
	/// <summary>
	/// This class having entities of table Transaction
	/// Created By :: Rekansh Patel
	/// Created On :: 10/30/2020
	/// </summary>
	public class TransactionEntity
	{
		#region Constructor
		/// <summary>
		/// This construction is set properties default value based on its data type in table.
		/// </summary>
		public TransactionEntity()
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

		/// <summary>
		/// Get & Set Account Id
		/// </summary>
		public int AccountId { get; set; }

		/// <summary>
		/// Get & Set Type Id
		/// </summary>
		public int TypeId { get; set; }

		/// <summary>
		/// Get & Set Script Id
		/// </summary>
		public int ScriptId { get; set; }

		/// <summary>
		/// Get & Set Qty
		/// </summary>
		public int Qty { get; set; }

		/// <summary>
		/// Get & Set Price
		/// </summary>
		public double Price { get; set; }

		/// <summary>
		/// Get & Set Brokerage
		/// </summary>
		public double Brokerage { get; set; }

		/// <summary>
		/// Get & Set Cr
		/// </summary>
		public double Cr { get; set; }

		/// <summary>
		/// Get & Set Dr
		/// </summary>
		public double Dr { get; set; }

		/// <summary>
		/// Get & Set Balance
		/// </summary>
		public double Balance { get; set; }

		/// <summary>
		/// Get & Set Description
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Get & Set Invester Id
		/// </summary>
		public int InvesterId { get; set; }

		#endregion

		#region Private Methods
		/// <summary>
		/// This function is set properties default value based on its data type in table.
		/// </summary>
		private void SetDefaulValue()
		{
			Id = 0;
			Date = DateTime.MinValue;
			AccountId = 0;
			TypeId = 0;
			ScriptId = 0;
			Qty = 0;
			Price = 0;
			Brokerage = 0;
			Cr = 0;
			Dr = 0;
			Balance = 0;
			Description = string.Empty;
			InvesterId = 0;
		}
		#endregion
	}

	public class TransactionAddEntity
	{
		public List<AccountMainEntity> Accounts = new List<AccountMainEntity>();
		public List<TypeMainEntity> Types = new List<TypeMainEntity>();
		public List<ScriptMainEntity> Scripts = new List<ScriptMainEntity>();
		public List<InvesterMainEntity> Investers = new List<InvesterMainEntity>();
	}

	public class TransactionEditEntity : TransactionAddEntity
	{
		public TransactionEntity Transaction = new TransactionEntity();
	}

	public class TransactionGridEntity
	{
		public List<TransactionEntity> Transactions = new List<TransactionEntity>();
		public int TotalRecords { get; set; }
	}

	public class TransactionListEntity : TransactionGridEntity
	{
		public List<AccountMainEntity> Accounts = new List<AccountMainEntity>();
		public List<TypeMainEntity> Types = new List<TypeMainEntity>();
		public List<ScriptMainEntity> Scripts = new List<ScriptMainEntity>();
		public List<InvesterMainEntity> Investers = new List<InvesterMainEntity>();
	}

	public class TransactionParameterEntity : PagingSortingEntity
	{
		#region Constructor
		/// <summary>
		/// This construction is set properties default value based on its data type in table.
		/// </summary>
		public TransactionParameterEntity()
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

		/// <summary>
		/// Get & Set Account Id
		/// </summary>
		public int AccountId { get; set; }

		/// <summary>
		/// Get & Set Type Id
		/// </summary>
		public int TypeId { get; set; }

		/// <summary>
		/// Get & Set Script Id
		/// </summary>
		public int ScriptId { get; set; }

		/// <summary>
		/// Get & Set Invester Id
		/// </summary>
		public int InvesterId { get; set; }
		#endregion

		#region Private Methods
		/// <summary>
		/// This function is set properties default value based on its data type in table.
		/// </summary>
		private void SetDefaulValue()
		{
			Id = 0;
			Date = DateTime.MinValue;
			AccountId = 0;
			TypeId = 0;
			ScriptId = 0;
			InvesterId = 0;
		}
		#endregion
	}
}
