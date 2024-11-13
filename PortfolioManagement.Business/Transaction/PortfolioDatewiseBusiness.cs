using CommonLibrary;
using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Business.Master;
using PortfolioManagement.Entity.Account;
using PortfolioManagement.Entity.ScriptView;
using PortfolioManagement.Entity.Transaction;
using System.Data;

namespace PortfolioManagement.Business.Transaction
{
    public class PortfolioDatewiseBusiness : CommonBusiness
    {
        ISql sql;
        IConfiguration config;
        public PortfolioDatewiseBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
            this.config = config;
        }

        //for insert process PortfolioDatewise  
        public async Task ProcessPortfolioDatewise(DateTime current, DateTime portfolioDateTime)
        {
            if (current >= portfolioDateTime.AddMinutes(-5) && current <= portfolioDateTime.AddMinutes(5))
            {
                AccountBrokerBusiness accountBrokerBusiness = new AccountBrokerBusiness(config);
                List<PortfolioDatewiseEntity> portfolioDatewiseEntitys = await accountBrokerBusiness.SelectForPortfolioDatewiseReport();

                foreach (PortfolioDatewiseEntity portfolioDatewiseEntity in portfolioDatewiseEntitys)
                {
                    portfolioDatewiseEntity.Date = DateTime.UtcNow;

                    try
                    {
                        Log.Write($"Start Portfolio process for PMS Id:{portfolioDatewiseEntity.PmsId} AccountId:{portfolioDatewiseEntity.AccountId}, BrokerId:{portfolioDatewiseEntity.BrokerId}");
                        await selectForPortfolioDatewiseProcess(portfolioDatewiseEntity);

                        await insert(portfolioDatewiseEntity);
                        Log.Write($"End Portfolio process for PMS Id:{portfolioDatewiseEntity.PmsId} AccountId:{portfolioDatewiseEntity.AccountId}, BrokerId:{portfolioDatewiseEntity.BrokerId}");
                    }
                    catch (Exception ex)
                    {
                        Log.Write($"Error while Portfolio process for PMS Id:{portfolioDatewiseEntity.PmsId} AccountId:{portfolioDatewiseEntity.AccountId}, BrokerId:{portfolioDatewiseEntity.BrokerId}\n{ex.Message}");
                        await ex.WriteLogFileAsync();
                    }
                }
            }
        }

        private async Task selectForPortfolioDatewiseProcess(PortfolioDatewiseEntity portfolioDatewiseEntity)
        {
            sql.AddParameter("PmsId", portfolioDatewiseEntity.PmsId);
            sql.AddParameter("BrokerId", portfolioDatewiseEntity.BrokerId);
            sql.AddParameter("AccountId", portfolioDatewiseEntity.AccountId);
            PortfolioDatewiseEntity portfolioDatewiseEntityTemp = await sql.ExecuteRecordAsync<PortfolioDatewiseEntity>("Portfolio_SelectForPortfolioDatewiseProcess", CommandType.StoredProcedure);
            portfolioDatewiseEntity.InvestmentAmount = portfolioDatewiseEntityTemp.InvestmentAmount;
            portfolioDatewiseEntity.UnReleasedAmount = portfolioDatewiseEntityTemp.UnReleasedAmount;
        }

        private async Task<long> insert(PortfolioDatewiseEntity portfolioDatewiseEntity)
        {
            sql.AddParameter("PmsId", portfolioDatewiseEntity.PmsId);
            sql.AddParameter("BrokerId", portfolioDatewiseEntity.BrokerId);
            sql.AddParameter("AccountId", portfolioDatewiseEntity.AccountId);
            sql.AddParameter("Date", DbType.DateTime, ParameterDirection.Input, portfolioDatewiseEntity.Date);
            sql.AddParameter("InvestmentAmount", portfolioDatewiseEntity.InvestmentAmount);
            sql.AddParameter("UnReleasedAmount", portfolioDatewiseEntity.UnReleasedAmount);
            return MyConvert.ToLong(await sql.ExecuteScalarAsync("PortfolioDateWise_Insert", CommandType.StoredProcedure));
        }

        //public async Task<List<PortfolioDatewiseReportEntity>> SelectForPortfolioDatewiseData(PortfolioDatewiseParameterEntity portfolioDatewiseParameterEntity)
        //{
        //    sql.AddParameter("PmsId", portfolioDatewiseParameterEntity.PmsId);
        //    if (portfolioDatewiseParameterEntity.BrokerId != 0)
        //        sql.AddParameter("BrokerId", portfolioDatewiseParameterEntity.BrokerId);
        //    if (portfolioDatewiseParameterEntity.AccountId != 0)
        //        sql.AddParameter("AccountId", portfolioDatewiseParameterEntity.AccountId);
        //    sql.AddParameter("FromDate", DbType.DateTime, ParameterDirection.Input, portfolioDatewiseParameterEntity.FromDate);
        //    sql.AddParameter("ToDate", DbType.DateTime, ParameterDirection.Input, portfolioDatewiseParameterEntity.ToDate);
        //    return await sql.ExecuteListAsync<PortfolioDatewiseReportEntity>("PortfolioDatewise_SelectForPortfolioDatewiseReport", CommandType.StoredProcedure);
        //}

        public async Task<List<PortfolioDatewiseReportEntity>> SelectForPortfolioDatewiseData(PortfolioDatewiseParameterEntity portfolioDatewiseParameterEntity)
        {
            List<PortfolioDatewiseReportEntity> portfolioDatewiseReportEntity = new List<PortfolioDatewiseReportEntity>();

            sql.AddParameter("PmsId", portfolioDatewiseParameterEntity.PmsId);
            if (portfolioDatewiseParameterEntity.BrokerId != 0)
                sql.AddParameter("BrokerId", portfolioDatewiseParameterEntity.BrokerId);
            if (portfolioDatewiseParameterEntity.AccountId != 0)
                sql.AddParameter("AccountId", portfolioDatewiseParameterEntity.AccountId);
            sql.AddParameter("FromDate", DbType.DateTime, ParameterDirection.Input, portfolioDatewiseParameterEntity.FromDate);
            sql.AddParameter("ToDate", DbType.DateTime, ParameterDirection.Input, portfolioDatewiseParameterEntity.ToDate);

            portfolioDatewiseReportEntity = await sql.ExecuteListAsync<PortfolioDatewiseReportEntity>("PortfolioDatewise_SelectForPortfolioDatewiseReport", CommandType.StoredProcedure);

            List<PortfolioDatewiseReportEntity> updatedReportEntities = new List<PortfolioDatewiseReportEntity>();

            List<string> allTimeSeries = new List<string>();
            List<double> allInvestmentSeriesData = new List<double>();
            List<double> allMarketValueSeriesData = new List<double>();

            foreach (var reportEntity in portfolioDatewiseReportEntity)
            {
                
                allTimeSeries.Add(reportEntity.Date.ToString("yyyy-MM-dd"));

                allInvestmentSeriesData.Add(reportEntity.TotalInvestmentAmount);
                allMarketValueSeriesData.Add(reportEntity.TotalUnReleasedAmount);
            }

            PortfolioDatewiseReportEntity finalReportEntity = new PortfolioDatewiseReportEntity
            {
                TimeSeries = allTimeSeries,
                InvestmentSeries= allInvestmentSeriesData,
                MarketValueSeries= allMarketValueSeriesData
            };

            return new List<PortfolioDatewiseReportEntity> { finalReportEntity };
        }

    }
}
