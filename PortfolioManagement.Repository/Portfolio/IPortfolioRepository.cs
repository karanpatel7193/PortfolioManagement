using PortfolioManagement.Entity.Transaction;
using PortfolioManagement.Entity.Transaction.StockTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Portfolio
{
    public interface IPortfolioRepository
    {
        public Task<List<ProtfolioEntity>> Select(ProtfolioParameterEntity protfolioParameterEntity);
        public Task<PortfolioReportEntity> SelectPortfolioReport(StockTransactionParameterEntity transactionParameterEntity);

    }
}
