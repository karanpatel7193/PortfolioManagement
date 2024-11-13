using System.Collections.Generic;

namespace PortfolioManagement.Entity.Master
{

    /// <summary>
    /// This class having main entities of table Type
    /// Created By :: Rekansh Patel
    /// Created On :: 10/30/2020
    /// </summary>
    public class TypeMainEntity
	{
		#region Constructor
		/// <summary>
		/// This construction is set properties default value based on its data type in table.
		/// </summary>
		public TypeMainEntity()
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
	/// This class having entities of table Type
	/// Created By :: Rekansh Patel
	/// Created On :: 10/30/2020
	/// </summary>
	public class TypeEntity : TypeMainEntity
	{
		#region Constructor
		/// <summary>
		/// This construction is set properties default value based on its data type in table.
		/// </summary>
		public TypeEntity()
		{
			SetDefaulValue();
		}
		#endregion

		#region Public Properties

		#endregion

		#region Private Methods
		/// <summary>
		/// This function is set properties default value based on its data type in table.
		/// </summary>
		private void SetDefaulValue()
		{

		}
		#endregion
	}

	public class TypeGridEntity
	{
		public List<TypeEntity> Types = new List<TypeEntity>();
		public int TotalRecords { get; set; }
	}

	public class TypeParameterEntity : PagingSortingEntity
	{
		#region Constructor
		/// <summary>
		/// This construction is set properties default value based on its data type in table.
		/// </summary>
		public TypeParameterEntity()
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
