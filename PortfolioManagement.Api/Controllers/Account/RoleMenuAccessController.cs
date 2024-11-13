using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Account;
using PortfolioManagement.Entity.Account;

namespace PortfolioManagement.Api.Controllers.Account
{
    [Route("account/[controller]")]
    [ApiController]
    public class RoleMenuAccessController : Controller
    {
        /// <summary>
        /// Get role menu access by role id and parent menu id.
        /// </summary>
        /// <param name="roleMenuAccessEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getByRoleIdParentId", Name = "account.roleMenuAccess.getByRoleIdParentId")]
        [AuthorizeAPI(pageName: "Role", pageAccess: PageAccessValues.IgnoreAuthorization)]
        public async Task<Response> GetByRoleIdParentId(RoleMenuAccessEntity roleMenuAccessEntity)
        {
            Response response;
            try
            {
                RoleMenuAccessBusiness roleMenuAccessBusiness = new RoleMenuAccessBusiness(Startup.Configuration);
                response = new Response(await roleMenuAccessBusiness.SelectListByRoleIdParentId(roleMenuAccessEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Bulk insert or update role menu access by role id.
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("bulkOperation", Name = "account.roleMenuAccess.bulkOperation")]
        [AuthorizeAPI(pageName: "Role", pageAccess: PageAccessValues.Insert)]
        public async Task<Response> BulkOperation(RoleEntity roleEntity)
        {
            Response response;
            try
            {
                RoleMenuAccessBusiness roleMenuAccessBusiness = new RoleMenuAccessBusiness(Startup.Configuration);
                await roleMenuAccessBusiness.Bulk(roleEntity);
                response = new Response();
                Cache.RoleMenuAccess.Refresh();
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
