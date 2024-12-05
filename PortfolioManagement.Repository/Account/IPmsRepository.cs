using PortfolioManagement.Entity.Account;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Account
{
    public interface IPmsRepository
    {
        public  Task<PmsEntity> SelectForRecord(int Id);

        public  Task<PmsGridEntity> SelectForGrid(PmsParameterEntity pmsParameterEntity);
        public  Task MapGridEntity(int resultSet, PmsGridEntity pmsGridEntity, IDataReader reader);
        public  Task<int> Insert(PmsEntity pmsEntity);
        public  Task<int> Update(PmsEntity pmsEntity);
        public  Task Delete(int Id);
    }
}
