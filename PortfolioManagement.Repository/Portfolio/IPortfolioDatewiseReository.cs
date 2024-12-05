using PortfolioManagement.Entity.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Portfolio
{
    public interface IPortfolioDatewiseReository
    {
        public Task ProcessPortfolioDatewise(DateTime current, DateTime portfolioDateTime);
        public Task<List<PortfolioDatewiseReportEntity>> SelectForPortfolioDatewiseData(PortfolioDatewiseParameterEntity portfolioDatewiseParameterEntity);
    }
}
