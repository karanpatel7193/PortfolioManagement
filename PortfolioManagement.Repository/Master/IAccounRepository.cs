using CommonLibrary;
using PortfolioManagement.Entity.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Master
{
    public interface IAccounRepository
    {
        public AccountEntity MapData(IDataReader reader);
        public  Task<AccountEntity> SelectForRecord(int id);
        public  Task<List<AccountMainEntity>> SelectForLOV(AccountParameterEntity accountParameterEntity);
        public  Task<AccountGridEntity> SelectForGrid(AccountParameterEntity accountParameterEntity);
        public  Task MapGridEntity(int resultSet, AccountGridEntity accountGridEntity, IDataReader reader);
        public  Task<AccountAddEntity> SelectForAdd(AccountParameterEntity accountParameterEntity);
        public  Task MapAddEntity(int resultSet, AccountAddEntity accountAddEntity, IDataReader reader);
        public  Task<AccountEditEntity> SelectForEdit(AccountParameterEntity accountParameterEntity);
        public  Task MapEditEntity(int resultSet, AccountEditEntity accountEditEntity, IDataReader reader);
        public  Task<int> Insert(AccountEntity accountEntity);
        public  Task<int> Update(AccountEntity accountEntity);
        public Task Delete(int id);

    }
}
