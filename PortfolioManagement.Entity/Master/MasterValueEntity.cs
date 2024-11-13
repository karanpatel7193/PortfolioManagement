namespace PortfolioManagement.Entity.Master
{
    /// <summary>
    /// This class having entities of table MasterValueLanguage
    /// Created By :: Rekansh Patel
    /// Created On :: 03/23/2018
    /// </summary>
    public class MasterValueEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public MasterValueEntity()
        {
            SetDefaulValue();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Get & Set Master Id
        /// </summary>
        public int MasterId { get; set; }

        /// <summary>
        /// Get & Set Value
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Get & Set Value Text
        /// </summary>
        public string ValueText { get; set; }
        #endregion

        #region Private Methods
        /// <summary>
        /// This function is set properties default value based on its data type in table.
        /// </summary>
        private void SetDefaulValue()
        {
            MasterId = 0;
            Value = 0;
            ValueText = string.Empty;
        }
        #endregion
    }
}
