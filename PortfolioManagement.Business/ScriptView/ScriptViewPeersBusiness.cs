using CommonLibrary;
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
    public class ScriptViewPeersBusiness :CommonBusiness
    {
        ISql sql;
        public ScriptViewPeersBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }

        public async Task<List<ScriptViewPeersEntity>> SelectForPeers(int id)
        {
            sql.AddParameter("Id", id);
            return await sql.ExecuteListAsync<ScriptViewPeersEntity>("ScriptView_SelectForPeers", CommandType.StoredProcedure);
        }
    }
}
