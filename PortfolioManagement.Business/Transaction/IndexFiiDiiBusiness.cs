using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Index;
using PortfolioManagement.Entity.Master;
using PortfolioManagement.Entity.ScriptView;
using PortfolioManagement.Entity.Transaction;
using PortfolioManagement.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Transaction
{
    public class IndexFiiDiiBusiness : CommonBusiness, IIndexFiiDiiRepository
    {
        ISql sql;
        public IndexFiiDiiBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();

        }

        public async Task<IndexFiiDiiChartEntity> SelectForChart(IndexFiiDiiParameterEntity indexFiiDiiParameterEntity)
        {
            IndexFiiDiiChartEntity indexFiiDiiChartEntity = new IndexFiiDiiChartEntity();
            sql.AddParameter("DateRange", indexFiiDiiParameterEntity.DateRange);
            sql.AddParameter("TodayDate", DbType.DateTime, ParameterDirection.Input, indexFiiDiiParameterEntity.TodayDate);

            var indexData = await sql.ExecuteListAsync<IndexFiiDiiEntity>("Index_SelectForFiiDiiChart", CommandType.StoredProcedure);

            if (indexData != null && indexData.Any())
            {
                foreach (var index in indexData)
                {
                    FilterIndex(indexFiiDiiChartEntity, index);
                }
            }
            return indexFiiDiiChartEntity;
        }

        private void FilterIndex(IndexFiiDiiChartEntity indexFiiDiiChartEntity, IndexFiiDiiEntity index)
        {
            indexFiiDiiChartEntity.Dates.Add(index.Date);
            indexFiiDiiChartEntity.NiftySeriesData.Add(index.Nifty);
            indexFiiDiiChartEntity.SensexSeriesData.Add(index.Sensex);
            indexFiiDiiChartEntity.FIISeriesData.Add(index.FII);
            indexFiiDiiChartEntity.DIISeriesData.Add(index.DII);
        }

    }
}
