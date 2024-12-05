using PortfolioManagement.Entity.Account;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Account
{
    public interface IRoleRepository
    {
        public  Task<RoleEntity> SelectForRecord(int Id);
        public  Task<List<RoleMainEntity>> SelectForLOV(RoleParameterEntity roleParameterEntity);
        public  Task<RoleGridEntity> SelectForGrid(RoleParameterEntity roleParameterEntity);
        public  Task MapGridEntity(int resultSet, RoleGridEntity roleGridEntity, IDataReader reader);
        public  Task<int> Insert(RoleEntity roleEntity);
        public  Task<int> Update(RoleEntity roleEntity);
        public  Task Delete(int Id);
        public List<RoleEntity> SelectList();
    }
}
