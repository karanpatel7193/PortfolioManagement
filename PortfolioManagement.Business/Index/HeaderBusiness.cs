using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Index;
using PortfolioManagement.Entity.ScriptView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Index
{
    public class HeaderBusiness : CommonBusiness
    {
        ISql sql;
        public HeaderBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }
        public async Task<HeaderGridEntity> SelectForGrid()
        {
            HeaderGridEntity headerGridEntity = new HeaderGridEntity();
            await sql.ExecuteEnumerableMultipleAsync<HeaderGridEntity>("Index_SelectForNifty50", CommandType.StoredProcedure, 1, headerGridEntity, MapGridEntity);
            return headerGridEntity;
        }
        public async Task MapGridEntity(int resultSet, HeaderGridEntity headerGridEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    headerGridEntity.Nifty50.Add(await sql.MapDataDynamicallyAsync<HeaderNifty50Entity>(reader));
                    break;
            }
        }
        public async Task<HeaderEntity> SelectForIndex()
        {
            return await sql.ExecuteRecordAsync<HeaderEntity>("Index_SelectForHeader", CommandType.StoredProcedure);
        }
    }
}
