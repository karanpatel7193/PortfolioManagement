using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Transaction;
using System.Data;

namespace PortfolioManagement.Business.Master
{
    public class AccountBrokerBusiness : CommonBusiness
    {
        ISql sql;

        public AccountBrokerBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }

        public async Task<List<PortfolioDatewiseEntity>> SelectForPortfolioDatewiseReport()
        {
            return await sql.ExecuteListAsync<PortfolioDatewiseEntity>("AccountBroker_SelectForPortfolioDatewiseReport", CommandType.StoredProcedure);
        }
    }
}
