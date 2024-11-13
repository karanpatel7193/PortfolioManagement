using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.IndexView;
using PortfolioManagement.Entity.ScriptView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.IndexView
{
    public class IndexViewChartBusiness : CommonBusiness
    {
        ISql sql;
        public IndexViewChartBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();

        }

        public async Task<IndexViewChartEntity> SelectForIndexChart(IndexViewParameterEntity indexViewParameterEntity)
        {
            IndexViewChartEntity indexViewChartEntity = new IndexViewChartEntity();
            sql.AddParameter("FromDate", indexViewParameterEntity.FromDate);
            sql.AddParameter("ToDate", indexViewParameterEntity.ToDate);

            var indexData = await sql.ExecuteListAsync<IndexViewEntity>("ScriptView_IndexChart", CommandType.StoredProcedure);

            if (indexData != null && indexData.Any())
            {
                foreach (var index in indexData)
                {
                    FilterIndex(indexViewChartEntity, index);
                }
            }
            return indexViewChartEntity;
        }

        private void FilterIndex(IndexViewChartEntity indexViewChartEntity, IndexViewEntity index)
        {
            indexViewChartEntity.Dates.Add(index.Date);
            indexViewChartEntity.SensexSeriesData.Add(new double[] { index.SensexHigh, index.SensexLow, index.SensexClose, index.SensexOpen });
            indexViewChartEntity.NiftySeriesData.Add(new double[] { index.NiftyHigh, index.NiftyLow, index.NiftyClose, index.NiftyOpen });
            indexViewChartEntity.FiiSeriesData.Add(index.FII);
            indexViewChartEntity.DiiSeriesData.Add(index.DII);
        }
    }
}
