using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Account;
using PortfolioManagement.Business.Master;
using PortfolioManagement.Entity.Account;
using PortfolioManagement.Entity.Master;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortfolioManagement.Api.Controllers.Account
{
    [Route("Account/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        #region Interface public methods
        /// <summary>
        /// Get all columns values for perticular role record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getRecord/{Id:int}", Name = "account.role.record")]
        [AuthorizeAPI(pageName: "Role", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForRecord(int Id)
        {
            Response response;
            try
            {
                RoleBusiness roleBusiness = new RoleBusiness(Startup.Configuration);
                response = new Response(await roleBusiness.SelectForRecord(Id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get main columns informations for bind role LOV
        /// </summary>
        /// <param name="roleParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getLovValue", Name = "account.role.lovValue")]
        [AuthorizeAPI(pageName: "Role", pageAccess: PageAccessValues.IgnoreAuthorization)]
        public async Task<Response> GetForLOV(RoleParameterEntity roleParameterEntity)
        {
            Response response;
            try
            {
                RoleBusiness roleBusiness = new RoleBusiness(Startup.Configuration);
                response = new Response(await roleBusiness.SelectForLOV(roleParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get role list for bind grid.
        /// </summary>
        /// <param name="roleParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getGridData", Name = "account.role.gridData")]
        [AuthorizeAPI(pageName: "Role", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForGrid(RoleParameterEntity roleParameterEntity)
        {
            Response response;
            try
            {
                RoleBusiness roleBusiness = new RoleBusiness(Startup.Configuration);
                response = new Response(await roleBusiness.SelectForGrid(roleParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Insert record in role table.
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("insert", Name = "account.role.insert")]
        [AuthorizeAPI(pageName: "Role", pageAccess: PageAccessValues.Insert)]
        public async Task<Response> Insert(RoleEntity roleEntity)
        {
            Response response;
            try
            {
                RoleBusiness roleBusiness = new RoleBusiness(Startup.Configuration);
                response = new Response(await roleBusiness.Insert(roleEntity));
                Cache.RoleMenuAccess.Refresh();
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Update record in role table.
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update", Name = "account.role.update")]
        [AuthorizeAPI(pageName: "Role", pageAccess: PageAccessValues.Update)]
        public async Task<Response> Update(RoleEntity roleEntity)
        {
            Response response;
            try
            {
                RoleBusiness roleBusiness = new RoleBusiness(Startup.Configuration);
                response = new Response(await roleBusiness.Update(roleEntity));
                Cache.RoleMenuAccess.Refresh();
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Delete record from role table.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete/{Id:int}", Name = "account.role.delete")]
        [AuthorizeAPI(pageName: "Role", pageAccess: PageAccessValues.Delete)]
        public async Task<Response> Delete(int Id)
        {
            Response response;
            try
            {
                RoleBusiness roleBusiness = new RoleBusiness(Startup.Configuration);
                await roleBusiness.Delete(Id);
                response = new Response();
                Cache.RoleMenuAccess.Refresh();
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
        #endregion
    }
}
