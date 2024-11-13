using CommonLibrary;
using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Transaction;
using PortfolioManagement.Entity.Transaction.StockTransaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Transaction
{
    /// <summary>
    /// This class having protfolio report methods.
    /// Created By :: Rekansh Patel
    /// Created On :: 12/14/2020
    /// </summary>
    public class ProtfolioBusiness : CommonBusiness
    {
        ISql sql;

        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public ProtfolioBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }
        #endregion

        #region Public Override Methods
        /// <summary>
        /// This function returns main informations for select protofolio report.
        /// </summary>
        /// <param name="protfolioEntity">Filter criteria by Entity</param>
        /// <returns>Main Entitys</returns>
		public async Task<List<ProtfolioEntity>> Select(ProtfolioParameterEntity protfolioParameterEntity)
        {
            if (protfolioParameterEntity.AccountId != 0)
                sql.AddParameter("AccountId", protfolioParameterEntity.AccountId);
            if (protfolioParameterEntity.ScriptId != 0)
                sql.AddParameter("ScriptId", protfolioParameterEntity.ScriptId);
            if (protfolioParameterEntity.FromDate != DateTime.MinValue)
                sql.AddParameter("FromDate", protfolioParameterEntity.FromDate);
            if (protfolioParameterEntity.ToDate != DateTime.MinValue)
                sql.AddParameter("ToDate", protfolioParameterEntity.ToDate);
            sql.AddParameter("GroupByScript", protfolioParameterEntity.GroupByScript);

            return await sql.ExecuteListAsync<ProtfolioEntity>("Protfolio_Select", CommandType.StoredProcedure);
        }

        public async Task<PortfolioReportEntity> SelectPortfolioReport(StockTransactionParameterEntity transactionParameterEntity)
        {
            PortfolioReportEntity portfolioReportEntity = new PortfolioReportEntity();
            if (transactionParameterEntity.AccountId != 0)
                sql.AddParameter("AccountId", transactionParameterEntity.AccountId);
            if (transactionParameterEntity.BrokerId != 0)
                sql.AddParameter("BrokerId", transactionParameterEntity.BrokerId);
            sql.AddParameter("PmsId", transactionParameterEntity.PmsId);
       
            portfolioReportEntity.Scripts = await sql.ExecuteListAsync<PortfolioScriptEntity>("Portfolio_Report", CommandType.StoredProcedure, mapPortfolioReportEntity);
            
            fillInvestmentSector(portfolioReportEntity);
            
            fillMarketSector(portfolioReportEntity);
            
            fillPortfolioSummary(portfolioReportEntity);
            
            return portfolioReportEntity;
        }
          

        private PortfolioScriptEntity mapPortfolioReportEntity(IDataReader reader)
        {
            PortfolioScriptEntity record = sql.MapDataDynamically<PortfolioScriptEntity>(reader);
            record.InvestmentAmount = Math.Round(record.Qty * record.CostPrice, 2);
            record.MarketValue = Math.Round(record.Qty * record.CurrentPrice, 2);
            record.OverallGLAmount = Math.Round(record.MarketValue - record.InvestmentAmount, 2);
            record.OverallGLPercentage = record.InvestmentAmount > 0 && record.OverallGLAmount != 0? Math.Round(record.OverallGLAmount * 100 / record.InvestmentAmount, 2):0;
            record.DayGLAmount = Math.Round((record.CurrentPrice - record.PreviousDayPrice) * record.Qty, 2);
            record.DayGLPercentage = record.InvestmentAmount > 0 && record.DayGLAmount != 0 ? Math.Round((record.DayGLAmount * 100) / record.InvestmentAmount, 2):0;
            record.InvestmentAmount = Math.Round(record.Qty * record.CostPrice, 2);
            return record;
        }

        private void fillInvestmentSector(PortfolioReportEntity portfolioReportEntity)
        {
            portfolioReportEntity.InvestmentSectors = portfolioReportEntity.Scripts.GroupBy(d => d.IndustryName)
                                                        .Select(
                                                            g => new PortfolioSectorEntity
                                                            {
                                                                SectorName = g.Key,
                                                                Amount = g.Sum(s => s.InvestmentAmount)
                                                            }).ToList();
            portfolioReportEntity.PortfolioSummary.TotalInvestmentAmount = portfolioReportEntity.InvestmentSectors.Sum(x => x.Amount);
            foreach (var item in portfolioReportEntity.InvestmentSectors)
                item.Percentage = portfolioReportEntity.PortfolioSummary.TotalInvestmentAmount > 0 && item.Amount != 0 ? Math.Round(100 * item.Amount / portfolioReportEntity.PortfolioSummary.TotalInvestmentAmount,2):0;
        }

        private void fillMarketSector(PortfolioReportEntity portfolioReportEntity)
        {
            portfolioReportEntity.MarketSectors = portfolioReportEntity.Scripts.GroupBy(d => d.IndustryName)
                                                                    .Select(
                                                                        g => new PortfolioSectorEntity
                                                                        {
                                                                            SectorName = g.Key,
                                                                            Amount = g.Sum(s => s.MarketValue)
                                                                        }).ToList();

            portfolioReportEntity.PortfolioSummary.TotalMarketAmount = portfolioReportEntity.MarketSectors.Sum(x => x.Amount);
            foreach (var item in portfolioReportEntity.MarketSectors)
                item.Percentage = portfolioReportEntity.PortfolioSummary.TotalMarketAmount > 0 && item.Amount!=0?  Math.Round(100 * item.Amount / portfolioReportEntity.PortfolioSummary.TotalMarketAmount,2):0;
        }

        private  void fillPortfolioSummary(PortfolioReportEntity portfolioReportEntity)
        {
            portfolioReportEntity.PortfolioSummary.OverallGLAmount = Math.Round(portfolioReportEntity.PortfolioSummary.TotalMarketAmount - portfolioReportEntity.PortfolioSummary.TotalInvestmentAmount, 2);
            portfolioReportEntity.PortfolioSummary.DayGLPercentage = portfolioReportEntity.PortfolioSummary.TotalInvestmentAmount > 0 && portfolioReportEntity.PortfolioSummary.OverallGLAmount != 0? Math.Round(portfolioReportEntity.PortfolioSummary.DayGLAmount * 100 / portfolioReportEntity.PortfolioSummary.TotalInvestmentAmount, 2): 0;
            portfolioReportEntity.PortfolioSummary.DayGLAmount = Math.Round(portfolioReportEntity.Scripts.Sum(x => x.DayGLAmount), 2);
            portfolioReportEntity.PortfolioSummary.DayGLPercentage = portfolioReportEntity.PortfolioSummary.TotalInvestmentAmount > 0 && portfolioReportEntity.PortfolioSummary.OverallGLAmount != 0 ? Math.Round(portfolioReportEntity.PortfolioSummary.DayGLAmount * 100 / portfolioReportEntity.PortfolioSummary.TotalInvestmentAmount, 2) : 0;
            portfolioReportEntity.PortfolioSummary.ReleasedProfit = Math.Round(portfolioReportEntity.Scripts.Sum(x => x.ReleasedProfit), 2);
        }
        #endregion



    }

}
