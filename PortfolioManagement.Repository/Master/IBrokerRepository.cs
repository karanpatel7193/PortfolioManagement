using PortfolioManagement.Entity.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Master
{
    public interface IBrokerRepository
    {
        public BrokerEntity MapData(IDataReader reader);
        public  Task<BrokerEntity> SelectForRecord(int id);
        public  Task<List<BrokerMainEntity>> SelectForLOV(BrokerParameterEntity brokerParameterEntity);
        public  Task<BrokerGridEntity> SelectForGrid(BrokerParameterEntity brokerParameterEntity);
        public  Task MapGridEntity(int resultSet, BrokerGridEntity brokerGridEntity, IDataReader reader);
        public  Task<int> Insert(BrokerEntity brokerEntity);
        public  Task<int> Update(BrokerEntity brokerEntity);
        public  Task Delete(int id);
    }
}
