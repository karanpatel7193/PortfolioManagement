using CommonLibrary.SqlDB;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Index;
using PortfolioManagement.Entity.ScriptView;
using PortfolioManagement.Repository.Index;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Index
{
    public class IndexChartBusiness : CommonBusiness, IIndexChartRepository
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

            var indexData = await sql.ExecuteListAsync<IndexChartEntity>("Index_SelectForChart", CommandType.StoredProcedure);

            if (indexData != null && indexData.Any())
            {
                foreach (var index1 in indexData)
                {
                    string formattedDate = FormatDate(index1.Date, indexChartParameterEntity.DateRange);
                    FilterIndex(indexChartGridEntity, formattedDate, index1);
                }
            }
            return indexChartGridEntity;
        }
        private string FormatDate(DateTime date, string dateRange)
        {
            if (dateRange == "1D")
            {
                TimeZoneInfo indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime istTime = TimeZoneInfo.ConvertTimeFromUtc(date, indianTimeZone);
                return istTime.ToString("HH:mm");
            }
            else
            {
                return date.ToString("yyyy-MM-dd");
            }
        }
        private void FilterIndex(IndexChartGridEntity indexChartGridEntity, string formattedDate, IndexChartEntity index)
        {
          

            indexChartGridEntity.Dates.Add(formattedDate);
            indexChartGridEntity.SensexSeriesData.Add(index.Sensex);
            indexChartGridEntity.NiftySeriesData.Add(index.Nifty);
        }
    }
}
