using CommonLibrary;
using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Master;
using PortfolioManagement.Entity.ScriptView;
using PortfolioManagement.Repository.ScriptView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.ScriptView
{
    public class ScriptViewOverviewBusiness :CommonBusiness, IScriptViewOverviewRepository
    {
        ISql sql;
        public ScriptViewOverviewBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();

        }
        public async Task<ScriptViewOverviewEntity> SelectForOverview(int id)
        {
            sql.AddParameter("Id", id);
            return await sql.ExecuteRecordAsync<ScriptViewOverviewEntity>("ScriptViewOverview_SelectForOverview", CommandType.StoredProcedure);
        }
    }
}
