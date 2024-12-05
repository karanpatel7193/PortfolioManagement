using PortfolioManagement.Entity.Index;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Index
{
    public interface IHeaderRepository
    {
        public  Task<HeaderGridEntity> SelectForGrid();
        public  Task MapGridEntity(int resultSet, HeaderGridEntity headerGridEntity, IDataReader reader);
        public Task<HeaderEntity> SelectForIndex();
    }
}
