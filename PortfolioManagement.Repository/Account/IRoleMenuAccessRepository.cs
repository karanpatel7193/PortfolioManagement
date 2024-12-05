using PortfolioManagement.Entity.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Account
{
    public interface IRoleMenuAccessRepository
    {
        public List<RoleMenuAccessEntity> SelectList();
        public  Task<List<RoleMenuAccessEntity>> SelectListByRoleIdParentId(RoleMenuAccessEntity roleMenuAccessEntity);
        public  Task<int> Bulk(RoleEntity roleEntity);
    }
}
