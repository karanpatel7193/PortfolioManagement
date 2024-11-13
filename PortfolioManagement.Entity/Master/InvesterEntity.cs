using System.Collections.Generic;

namespace PortfolioManagement.Entity.Master
{

    /// <summary>
    /// This class having main entities of table Invester
    /// Created By :: Rekansh Patel
    /// Created On :: 10/30/2020
    /// </summary>
    public class InvesterMainEntity
	{
		#region Constructor
		/// <summary>
		/// This construction is set properties default value based on its data type in table.
		/// </summary>
		public InvesterMainEntity()
		{
			SetDefaulValue();
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Get & Set Id
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Get & Set Name
		/// </summary>
		public string Name { get; set; }

		#endregion

		#region Private Methods
		/// <summary>
		/// This function is set properties default value based on its data type in table.
		/// </summary>
		private void SetDefaulValue()
		{
			Id = 0;
			Name = string.Empty;

		}
		#endregion
	}

	/// <summary>
	/// This class having entities of table Invester
	/// Created By :: Rekansh Patel
	/// Created On :: 10/30/2020
	/// </summary>
	public class InvesterEntity : InvesterMainEntity
	{
		#region Constructor
		/// <summary>
		/// This construction is set properties default value based on its data type in table.
		/// </summary>
		public InvesterEntity()
		{
			SetDefaulValue();
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Get & Set Amount
		/// </summary>
		public double Amount { get; set; }

		#endregion

		#region Private Methods
		/// <summary>
		/// This function is set properties default value based on its data type in table.
		/// </summary>
		private void SetDefaulValue()
		{
			Amount = 0;
		}
		#endregion
	}

	public class InvesterGridEntity
	{
		public List<InvesterEntity> Investers = new List<InvesterEntity>();
		public int TotalRecords { get; set; }
	}

	public class InvesterParameterEntity : PagingSortingEntity
	{
		#region Constructor
		/// <summary>
		/// This construction is set properties default value based on its data type in table.
		/// </summary>
		public InvesterParameterEntity()
		{
			SetDefaulValue();
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Get & Set Id
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Get & Set Name
		/// </summary>
		public string Name { get; set; }

		#endregion

		#region Private Methods
		/// <summary>
		/// This function is set properties default value based on its data type in table.
		/// </summary>
		private void SetDefaulValue()
		{
			Id = 0;
			Name = string.Empty;

		}
		#endregion
	}

}
