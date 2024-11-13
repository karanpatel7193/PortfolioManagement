using System.Collections.Generic;
namespace PortfolioManagement.Entity.Master
{
    /// <summary>
    /// This class having main entities of table Script
    /// Created By :: Rekansh Patel
    /// Created On :: 10/30/2020
    /// </summary>
    public class ScriptMainEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public ScriptMainEntity()
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
    /// This class having entities of table Script
    /// Created By :: Rekansh Patel
    /// Created On :: 10/30/2020
    /// </summary>
    public class ScriptEntity : ScriptMainEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public ScriptEntity()
        {
            SetDefaulValue();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Get & Set Bse Code
        /// </summary>
        public decimal BseCode { get; set; }

        /// <summary>
        /// Get & Set Nse Code
        /// </summary>
        public string NseCode { get; set; }

        /// <summary>
        /// Get & Set Icici Code
        /// </summary>
        public string IciciCode { get; set; }

        public string IndustryName { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public int FaceValue { get; set; } = 0;
        public double Price {  get; set; } = 0;

        /// Get & Set ISINCode
        /// </summary>
        public string ISINCode { get; set; }

        /// <summary>
        /// Get & Set Money Control URL
        /// </summary>
        public string MoneyControlURL { get; set; }

        /// <summary>
        /// Get & Set Fetch URL
        /// </summary>
        public string FetchURL { get; set; }

        /// <summary>
        /// Get & Set Is Fetch
        /// </summary>
        public bool IsFetch { get; set; }

        /// <summary>
        /// Get & Set Is Active
        /// </summary>
        public bool IsActive { get; set; }

        

        #endregion

        #region Private Methods
        /// <summary>
        /// This function is set properties default value based on its data type in table.
        /// </summary>
        private void SetDefaulValue()
        {
            //BseCode = 0;
            //NseCode = "";
            IciciCode = string.Empty;
            ISINCode = string.Empty;
            MoneyControlURL = string.Empty;
            FetchURL = string.Empty;
            IsFetch = false;
            IsActive = false;
            //IndustryName = string.Empty;
            //Group = string.Empty;
            //FaceValue = 0;
        }
        #endregion
    }

    public class ScriptGridEntity
    {
        public List<ScriptEntity> Scripts { get; set; } = new List<ScriptEntity>();
        public int TotalRecords { get; set; }
    }

    public class ScriptParameterEntity : PagingSortingEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public ScriptParameterEntity()
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
        public decimal BseCode { get; set; }
        public string NseCode { get; set; }

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
