using PortfolioManagement.Entity.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Master
{
    public interface IMasterRepositoroy
    {
        public MasterEntity MapData(IDataReader reader);
        public  Task<int> Insert(MasterEntity masterEntity);
        public  Task<int> Update(MasterEntity masterEntity);
        public  Task Delete(int id);
        public  Task<List<MasterEntity>> SelectForGrid();
        public  Task<MasterEntity> SelectForRecord(short Id);

    }
}
