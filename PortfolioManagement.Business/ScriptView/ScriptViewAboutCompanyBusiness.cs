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
    public class ScriptViewAboutCompanyBusiness:CommonBusiness
    {
        ISql sql;
        public ScriptViewAboutCompanyBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();

        }
        public async Task<ScriptViewAboutCompanyEntity> SelectForAboutCompany(int id)
        {
            sql.AddParameter("Id", id);
            return await sql.ExecuteRecordAsync<ScriptViewAboutCompanyEntity>("ScriptView_SelectForAboutCompany", CommandType.StoredProcedure);
        }
    }
}
