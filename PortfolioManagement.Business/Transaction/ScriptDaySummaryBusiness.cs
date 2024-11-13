using CommonLibrary;
using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Transaction;
using System.Data;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Transaction
{
    /// <>
	/// This class having crud operation function of table ScriptDay
	/// Created By :: Rekansh Patel
	/// Created On :: 11/22/2020
	/// </>
	public class ScriptDaySummaryBusiness : CommonBusiness
    {
        ISql sql;

        #region Constructor
        /// <>
        /// This construction is set properties default value based on its data type in table.
        /// </>
        public ScriptDaySummaryBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }
        #endregion

        #region Public Methods
        /// <>
        /// This function insert record in ScriptDay table.
        /// </>
        /// <returns>Identity / AlreadyExist = 0</returns>
        public async Task<long> Insert(ScriptDaySummaryEntity scriptDaySummaryEntity)
        {
            sql.AddParameter("ScriptId", scriptDaySummaryEntity.ScriptId);
            sql.AddParameter("Date", DbType.Date, ParameterDirection.Input, scriptDaySummaryEntity.Date);
            sql.AddParameter("PreviousDay", scriptDaySummaryEntity.PreviousDay);
            sql.AddParameter("Open", scriptDaySummaryEntity.Open);
            sql.AddParameter("Close", scriptDaySummaryEntity.Close);
            sql.AddParameter("High", scriptDaySummaryEntity.High);
            sql.AddParameter("Low", scriptDaySummaryEntity.Low);
            sql.AddParameter("DateTime", DbType.DateTime, ParameterDirection.Input, scriptDaySummaryEntity.DateTime);
            sql.AddParameter("Price", scriptDaySummaryEntity.Price);
            sql.AddParameter("Volume", scriptDaySummaryEntity.Volume);
            sql.AddParameter("Value", scriptDaySummaryEntity.Value);
            sql.AddParameter("High52Week", scriptDaySummaryEntity.High52Week);
            sql.AddParameter("Low52Week", scriptDaySummaryEntity.Low52Week);

            return MyConvert.ToLong(await sql.ExecuteScalarAsync("ScriptDaySummary_Insert", CommandType.StoredProcedure));
        }

        #endregion
    }

}
