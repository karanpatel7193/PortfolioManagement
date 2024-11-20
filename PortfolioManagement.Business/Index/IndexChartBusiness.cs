using CommonLibrary.SqlDB;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Index;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Index
{
    public class IndexChartBusiness : CommonBusiness
    {
        ISql sql;
        public IndexChartBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }
        public async Task<IndexChartGridEntity> SelectForIndexChart(IndexChartParameterEntity indexChartParameterEntity)
        {
            IndexChartGridEntity indexChartGridEntity = new IndexChartGridEntity();
            sql.AddParameter("DateRange", indexChartParameterEntity.DateRange);
            sql.AddParameter("TodayDate", DbType.DateTime, ParameterDirection.Input, indexChartParameterEntity.TodayDate);

            //await sql.ExecuteEnumerableMultipleAsync<IndexChartGridEntity>("Index_SelectForChart", CommandType.StoredProcedure, 1, indexChartGridEntity, MapIndexChartEntity);
            var indexData = await sql.ExecuteListAsync<IndexChartEntity>("Index_SelectForChart", CommandType.StoredProcedure);


            if (indexData != null && indexData.Any())
            {
                foreach (var index1 in indexData)
                {
                    FilterIndex(indexChartGridEntity, index1);
                }
            }
            return indexChartGridEntity;
        }

        private void FilterIndex(IndexChartGridEntity indexChartGridEntity, IndexChartEntity index)
        {
            indexChartGridEntity.Dates.Add(index.Date);
            indexChartGridEntity.SensexSeriesData.Add( index.Sensex );
            indexChartGridEntity.NiftySeriesData.Add(index.Nifty);
        }

        //public async Task MapIndexChartEntity(int resultSet, IndexChartGridEntity indexChartGridEntity, IDataReader reader)
        //{
        //    switch (resultSet)
        //    {
        //        case 0:
        //            indexChartGridEntity.NiftySensex.Add(await sql.MapDataDynamicallyAsync<IndexChartEntity>(reader));
        //            break;
        //    }
        //}
    }
}
