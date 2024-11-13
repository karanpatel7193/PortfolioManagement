using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.ScriptView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.ScriptView
{
    public class ScriptViewCorporateActionBusiness : CommonBusiness
    {
        ISql sql;
        public ScriptViewCorporateActionBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();

        }
        public async Task<List<ScriptViewCorporateActionBonusEntity>> SelectForBonus(int id)
        {
            sql.AddParameter("Id", id);
            return await sql.ExecuteListAsync<ScriptViewCorporateActionBonusEntity>("ScriptView_CorporateAction_SelectForBonus", CommandType.StoredProcedure);
        }
        public async Task<List<ScriptViewCorporateActionSplitEntity>> SelectForSplit(int id)
        {
            sql.AddParameter("Id", id);
            return await sql.ExecuteListAsync<ScriptViewCorporateActionSplitEntity>("ScriptView_CorporateAction_SelectForSplit", CommandType.StoredProcedure);
        }
    }
}
