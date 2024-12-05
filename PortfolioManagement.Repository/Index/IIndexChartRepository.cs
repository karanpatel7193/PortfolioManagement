using PortfolioManagement.Entity.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Index
{
    public interface IIndexChartRepository
    {
        public Task<IndexChartGridEntity> SelectForIndexChart(IndexChartParameterEntity indexChartParameterEntity);
    }
}
