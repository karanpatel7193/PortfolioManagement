using CommonLibrary;
using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Business;
using PortfolioManagement.Entity.Transaction;
using System.Data;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Transaction
{
    /// <summary>
    /// This class having crud operation function of table Index
    /// Created By :: Rekansh Patel
    /// Created On :: 11/22/2020
    /// </summary>
    public class IndexBusiness : CommonBusiness
    {
        ISql sql;

        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public IndexBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }
        #endregion

        #region Public Override Methods
        /// <summary>
        /// This function insert record in Index table.
        /// </summary>
        /// <returns>Identity / AlreadyExist = 0</returns>
        public async Task<long> Insert(IndexEntity indexEntity)
        {
            sql.AddParameter("DateTime", DbType.DateTime, ParameterDirection.Input, indexEntity.Date);
            sql.AddParameter("SensexPreviousDay", indexEntity.SensexPreviousDay);
            sql.AddParameter("SensexOpen", indexEntity.SensexOpen);
            sql.AddParameter("SensexClose", indexEntity.SensexClose);
            sql.AddParameter("SensexHigh", indexEntity.SensexHigh);
            sql.AddParameter("SensexLow", indexEntity.SensexLow);
            sql.AddParameter("NiftyPreviousDay", indexEntity.NiftyPreviousDay);
            sql.AddParameter("NiftyOpen", indexEntity.NiftyOpen);
            sql.AddParameter("NiftyClose", indexEntity.NiftyClose);
            sql.AddParameter("NiftyHigh", indexEntity.NiftyHigh);
            sql.AddParameter("NiftyLow", indexEntity.NiftyLow);
            sql.AddParameter("Sensex", indexEntity.SensexClose);
            sql.AddParameter("Nifty", indexEntity.NiftyClose);
            return MyConvert.ToLong(await sql.ExecuteScalarAsync("Index_Insert", CommandType.StoredProcedure));
        }

        /// <summary>
        /// This function update Fii & Dii data in index table.
        /// </summary>
        /// <returns>Identity / AlreadyExist = 0</returns>
        public async Task<long> UpdateFiiDii(IndexEntity indexEntity)
        {
            sql.AddParameter("Date", DbType.DateTime, ParameterDirection.Input, indexEntity.Date);
            sql.AddParameter("FII", indexEntity.FII);
            sql.AddParameter("DII", indexEntity.DII);
            return MyConvert.ToLong(await sql.ExecuteScalarAsync("Index_UpdateFiiDii", CommandType.StoredProcedure));
        }
        #endregion
    }
}
