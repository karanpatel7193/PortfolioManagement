using PortfolioManagement.Entity.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Transaction
{
    public interface IIndexFiiDiiRepository
    {
        public Task<IndexFiiDiiChartEntity> SelectForChart(IndexFiiDiiParameterEntity indexFiiDiiParameterEntity);

    }
}
