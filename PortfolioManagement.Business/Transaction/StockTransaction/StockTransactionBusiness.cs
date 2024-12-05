using CommonLibrary;
using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PortfolioManagement.Entity.Account;
using PortfolioManagement.Entity.Analysis;
using PortfolioManagement.Entity.Master;
using PortfolioManagement.Entity.Transaction.StockTransaction;
using PortfolioManagement.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Transaction.StockTransaction
{
    public class StockTransactionBusiness : CommonBusiness, IStockTransactionRepository
    {
        ISql sql;
        public StockTransactionBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }

        public StockTransactionEntity MapData(IDataReader reader)
        {
            StockTransactionEntity transaction = new StockTransactionEntity();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "Id":
                        transaction.Id = MyConvert.ToLong(reader["Id"]);
                        break;
                    case "Date":
                        transaction.Date = MyConvert.ToDateTime(reader["Date"]);
                        break;
                    case "AccountId":
                        transaction.AccountId = MyConvert.ToByte(reader["AccountId"]);
                        break;
                    case "TransactionTypeId":
                        transaction.TransactionTypeId = MyConvert.ToByte(reader["TransactionTypeId"]);
                        break;
                    case "ScriptId":
                        transaction.ScriptId = MyConvert.ToByte(reader["ScriptId"]);
                        break;
                    case "Price":
                        transaction.Price = MyConvert.ToFloat(reader["Price"]);
                        break;
                    case "BrokerId":
                        transaction.BrokerId = MyConvert.ToByte(reader["BrokerId"]);
                        break;
                    case "Brokerage":
                        transaction.Brokerage = MyConvert.ToFloat(reader["Brokerage"]);
                        break;
                    case "Buy":
                        transaction.Buy = MyConvert.ToFloat(reader["Buy"]);
                        break;
                    case "Sell":
                        transaction.Sell = MyConvert.ToFloat(reader["Sell"]);
                        break;
                    case "Dividend":
                        transaction.Dividend = MyConvert.ToFloat(reader["Dividend"]);
                        break;
                }
            }
            return transaction;

        }

        //select record for update 1
        public async Task<StockTransactionEntity> SelectForRecord(int id)
        {
            sql.AddParameter("Id", id);
            return await sql.ExecuteRecordAsync<StockTransactionEntity>("StockTransaction_SelectForRecord", CommandType.StoredProcedure);
        }

        //select dropdown for 2
        public async Task<List<StockTransactionMainEntity>> SelectForLOV()
        {
            return await sql.ExecuteListAsync<StockTransactionMainEntity>("StockTransaction_SelectForLOV", CommandType.StoredProcedure);
        }

        //print data in list 3
        public async Task<StockTransactionGridEntity> SelectForGrid(StockTransactionParameterEntity transactionParameterEntity)
        {
            StockTransactionGridEntity transactionGridEntity = new StockTransactionGridEntity();
            if (transactionParameterEntity.Id != 0)
                sql.AddParameter("Id", transactionParameterEntity.Id);
            if (transactionParameterEntity.AccountId != 0)
                sql.AddParameter("AccountId", transactionParameterEntity.AccountId);
            if (transactionParameterEntity.ScriptId != 0)
                sql.AddParameter("ScriptId", transactionParameterEntity.ScriptId);
            if (transactionParameterEntity.BrokerId != 0)
                sql.AddParameter("BrokerId", transactionParameterEntity.BrokerId);
            if (transactionParameterEntity.TransactionTypeId != 0)
                sql.AddParameter("TransactionTypeId", transactionParameterEntity.TransactionTypeId);
            if (transactionParameterEntity.FromDate != DateTime.MinValue)
                sql.AddParameter("FromDate", DbType.DateTime, ParameterDirection.Input, transactionParameterEntity.FromDate);
            if (transactionParameterEntity.ToDate != DateTime.MinValue)
                sql.AddParameter("ToDate", DbType.DateTime, ParameterDirection.Input, transactionParameterEntity.ToDate);
            sql.AddParameter("PmsId", transactionParameterEntity.PmsId);
            sql.AddParameter("SortExpression", transactionParameterEntity.SortExpression);
            sql.AddParameter("SortDirection", transactionParameterEntity.SortDirection);
            sql.AddParameter("PageIndex", transactionParameterEntity.PageIndex);
            sql.AddParameter("PageSize", transactionParameterEntity.PageSize);
            await sql.ExecuteEnumerableMultipleAsync<StockTransactionGridEntity>("StockTransaction_SelectForGrid", CommandType.StoredProcedure, 2, transactionGridEntity, MapGridEntity);
            return transactionGridEntity;
        }

        //for mapping
        public async Task MapGridEntity(int resultSet, StockTransactionGridEntity transactionGridEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    transactionGridEntity.Stocks.Add(await sql.MapDataDynamicallyAsync<StockTransactionEntity>(reader));
                    break;
                case 1:
                    transactionGridEntity.TotalRecords = MyConvert.ToInt(reader["TotalRecords"]);
                    break;
            }
        }

        //insert 4
        public async Task<int> Insert(StockTransactionEntity transactionEntity)
        {
            if (transactionEntity.Date != DateTime.MinValue)
                sql.AddParameter("Date", DbType.DateTime, ParameterDirection.Input, transactionEntity.Date);
            if (transactionEntity.AccountId != 0)
                sql.AddParameter("AccountId", transactionEntity.AccountId);
            if (transactionEntity.TransactionTypeId != 0)
                sql.AddParameter("TransactionTypeId", transactionEntity.TransactionTypeId);
            if (transactionEntity.ScriptId != 0)
                sql.AddParameter("ScriptId", transactionEntity.ScriptId);
            if (transactionEntity.Qty != 0)
                sql.AddParameter("Qty", transactionEntity.Qty);
            if (transactionEntity.Price != 0)
                sql.AddParameter("Price", transactionEntity.Price);
            sql.AddParameter("BrokerId", transactionEntity.BrokerId);
            sql.AddParameter("Brokerage", transactionEntity.Brokerage);
            sql.AddParameter("Buy", transactionEntity.Buy);
            sql.AddParameter("Sell", transactionEntity.Sell);
            sql.AddParameter("Dividend", transactionEntity.Dividend);
            sql.AddParameter("PmsId", transactionEntity.PmsId);
            sql.CommandTimeout = 120;
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("StockTransaction_Insert", CommandType.StoredProcedure));
        }

        //Update 5
        public async Task<int> Update(StockTransactionEntity transactionEntity)
        {
            if (transactionEntity.Id != 0)
                sql.AddParameter("Id", transactionEntity.Id);
            if (transactionEntity.Date != DateTime.MinValue)
                sql.AddParameter("Date", DbType.DateTime, ParameterDirection.Input, transactionEntity.Date);
            if (transactionEntity.AccountId != 0)
                sql.AddParameter("AccountId", transactionEntity.AccountId);
            if (transactionEntity.TransactionTypeId != 0)
                sql.AddParameter("TransactionTypeId", transactionEntity.TransactionTypeId);
            if (transactionEntity.ScriptId != 0)
                sql.AddParameter("ScriptId", transactionEntity.ScriptId);
            if (transactionEntity.Qty != 0)
                sql.AddParameter("Qty", transactionEntity.Qty);
            if (transactionEntity.Price != 0)
                sql.AddParameter("Price", transactionEntity.Price);
            sql.AddParameter("BrokerId", transactionEntity.BrokerId);
            sql.AddParameter("Brokerage", transactionEntity.Brokerage);
            sql.AddParameter("Buy", transactionEntity.Buy);
            sql.AddParameter("Sell", transactionEntity.Sell);
            sql.AddParameter("Dividend", transactionEntity.Dividend);
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("StockTransaction_Update", CommandType.StoredProcedure));
        }

        //Delete 6
        public async Task Delete(int id)
        {
            sql.AddParameter("Id", id);
            await sql.ExecuteNonQueryAsync("StockTransaction_Delete", CommandType.StoredProcedure);
        }

        public async Task<StockTransactionListEntity> SelectForList(StockTransactionParameterEntity stockTransactionParameterEntity)
        {
            StockTransactionListEntity stockTransactionListEntity = new StockTransactionListEntity();
            sql.AddParameter("PmsId", stockTransactionParameterEntity.PmsId);
            await sql.ExecuteEnumerableMultipleAsync<StockTransactionListEntity>("StockTransaction_SelectForList", CommandType.StoredProcedure, 3, stockTransactionListEntity, MapSelectForListEntity);
            return stockTransactionListEntity;
        }
        public async Task MapSelectForListEntity(int resultSet, StockTransactionListEntity stockTransactionListEntity, IDataReader reader)
        {
            switch (resultSet)
            {

                case 0:
                    stockTransactionListEntity.Accounts.Add(await sql.MapDataDynamicallyAsync<AccountEntity>(reader));
                    break;
                case 1:
                    stockTransactionListEntity.Brokers.Add(await sql.MapDataDynamicallyAsync<BrokerEntity>(reader));
                    break;
                case 2:
                    stockTransactionListEntity.Scripts.Add(await sql.MapDataDynamicallyAsync<ScriptEntity>(reader));
                    break;
            }
        }

        public async Task<List<StockTransactionEntity>> SelectForReport(StockTransactionParameterEntity transactionParameterEntity)
        {
            if (transactionParameterEntity.AccountId != 0)
                sql.AddParameter("AccountId", transactionParameterEntity.AccountId);
            if (transactionParameterEntity.ScriptId != 0)
                sql.AddParameter("ScriptId", transactionParameterEntity.ScriptId);
            if (transactionParameterEntity.BrokerId != 0)
                sql.AddParameter("BrokerId", transactionParameterEntity.BrokerId);
            if (transactionParameterEntity.TransactionTypeId != 0)
                sql.AddParameter("TransactionTypeId", transactionParameterEntity.TransactionTypeId);
            if (transactionParameterEntity.FromDate != DateTime.MinValue)
                sql.AddParameter("FromDate", DbType.DateTime, ParameterDirection.Input, transactionParameterEntity.FromDate);
            if (transactionParameterEntity.ToDate != DateTime.MinValue)
                sql.AddParameter("ToDate", DbType.DateTime, ParameterDirection.Input, transactionParameterEntity.ToDate);
            sql.AddParameter("PmsId", transactionParameterEntity.PmsId);

            return await sql.ExecuteListAsync<StockTransactionEntity>("StockTransaction_SelectForReport", CommandType.StoredProcedure);
        }

        //for summary
        public async Task<List<StockTransactionSummaryEntity>> SelectForSummary(StockTransactionParameterEntity transactionParameterEntity)
        {
            if (transactionParameterEntity.AccountId != 0)
                sql.AddParameter("AccountId", transactionParameterEntity.AccountId);
            if (transactionParameterEntity.ScriptId != 0)
                sql.AddParameter("ScriptId", transactionParameterEntity.ScriptId);
            if (transactionParameterEntity.BrokerId != 0)
                sql.AddParameter("BrokerId", transactionParameterEntity.BrokerId);
            if (transactionParameterEntity.TransactionTypeId != 0)
                sql.AddParameter("TransactionTypeId", transactionParameterEntity.TransactionTypeId);
            if (transactionParameterEntity.FromDate != DateTime.MinValue)
                sql.AddParameter("FromDate", transactionParameterEntity.FromDate);
            if (transactionParameterEntity.ToDate != DateTime.MinValue)
                sql.AddParameter("ToDate", transactionParameterEntity.ToDate);
            sql.AddParameter("PmsId", transactionParameterEntity.PmsId);

            return await sql.ExecuteListAsync<StockTransactionSummaryEntity>("StockTransaction_SelectForSummary", CommandType.StoredProcedure);
        }

        



    }
}
