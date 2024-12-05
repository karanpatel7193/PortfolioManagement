using PortfolioManagement.Entity.Transaction.StockTransaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Transaction
{
    public interface IStockTransactionRepository
    {
        public StockTransactionEntity MapData(IDataReader reader);
        public Task<StockTransactionEntity> SelectForRecord(int id);
        public Task<List<StockTransactionMainEntity>> SelectForLOV();
        public Task<StockTransactionGridEntity> SelectForGrid(StockTransactionParameterEntity transactionParameterEntity);
        public Task MapGridEntity(int resultSet, StockTransactionGridEntity transactionGridEntity, IDataReader reader);
        public Task<int> Insert(StockTransactionEntity transactionEntity);
        public Task<int> Update(StockTransactionEntity transactionEntity);
        public Task Delete(int id);
        public Task<StockTransactionListEntity> SelectForList(StockTransactionParameterEntity stockTransactionParameterEntity);
        public Task MapSelectForListEntity(int resultSet, StockTransactionListEntity stockTransactionListEntity, IDataReader reader);
        public Task<List<StockTransactionEntity>> SelectForReport(StockTransactionParameterEntity transactionParameterEntity);
        public Task<List<StockTransactionSummaryEntity>> SelectForSummary(StockTransactionParameterEntity transactionParameterEntity);

    }
}
