using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Master;
using System.Data;

namespace PortfolioManagement.Business.Master
{
    /// <summary>
    /// This class having crud operation function of table MasterValue
    /// Created By :: Rekansh Patel
    /// Created On :: 03/23/2018
    /// </summary>
    public class MasterValueBusiness : CommonBusiness
    {
        ISql sql;

        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public MasterValueBusiness(IConfiguration configuration) : base(configuration)
        {
            sql = CreateSqlInstance();
        }
        #endregion

        #region Public Override Methods
        /// <summary>
        /// This function returns informations of MasterValue for cache refresh
        /// </summary>
        /// <param name="objMasterValueEAL">Filter criteria by EAL</param>
        /// <returns>Main EALs</returns>
		public List<MasterValueEntity> SelectForCache()
        {
            return sql.ExecuteList<MasterValueEntity>("MasterValue_SelectForCache", CommandType.StoredProcedure);
        }
        #endregion
    }
}
