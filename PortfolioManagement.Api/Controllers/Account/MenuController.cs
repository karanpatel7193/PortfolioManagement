using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Account;
using PortfolioManagement.Entity.Account;

namespace PortfolioManagement.Api.Controllers.Account
{
    [Route("account/menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        #region Interface public methods
        /// <summary>
        /// Get all columns values for perticular menu record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getRecord/{Id:int}", Name = "account.menu.record")]
        [AuthorizeAPI(pageName: "Menu", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForRecord(int Id)
        {
            Response response;
            try
            {
                MenuBusiness menuBusiness = new MenuBusiness(Startup.Configuration);
                response = new Response(await menuBusiness.SelectForRecord(Id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get menu's page all LOV data when menu page open in add mode.
        /// </summary>
        /// <param name="menuParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getAddMode", Name = "account.menu.addMode")]
        [AuthorizeAPI(pageName: "Menu", pageAccess: PageAccessValues.IgnoreAuthentication)]

        public async Task<Response> GetForAdd(MenuParameterEntity menuParameterEntity)
        {
            Response response;
            try
            {
                MenuBusiness menuBusiness = new MenuBusiness(Startup.Configuration);
                response = new Response(await menuBusiness.SelectForAdd(menuParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get menu's page all LOV data and all columns information for edit record when menu page open in edit mode.
        /// </summary>
        /// <param name="menuParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getEditMode", Name = "account.menu.editMode")]
        [AuthorizeAPI(pageName: "Menu", pageAccess: PageAccessValues.IgnoreAuthentication)]

        public async Task<Response> GetForEdit(MenuParameterEntity menuParameterEntity)
        {
            Response response;
            try
            {
                MenuBusiness menuBusiness = new MenuBusiness(Startup.Configuration);
                response = new Response(await menuBusiness.SelectForEdit(menuParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get menu list for bind grid.
        /// </summary>
        /// <param name="menuParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getGridData", Name = "account.menu.gridData")]
        [AuthorizeAPI(pageName: "Menu", pageAccess: PageAccessValues.View)]

        public async Task<Response> GetForGrid(MenuParameterEntity menuParameterEntity)
        {
            Response response;
            try
            {
                MenuBusiness menuBusiness = new MenuBusiness(Startup.Configuration);
                response = new Response(await menuBusiness.SelectForGrid(menuParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Insert record in menu table.
        /// </summary>
        /// <param name="menuEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("insert", Name = "account.menu.insert")]
        [AuthorizeAPI(pageName: "Menu", pageAccess: PageAccessValues.Insert)]

        public async Task<Response> Insert(MenuEntity menuEntity)
        {
            Response response;
            try
            {
                MenuBusiness menuBusiness = new MenuBusiness(Startup.Configuration);
                response = new Response(await menuBusiness.Insert(menuEntity));
                Cache.Menu.Refresh();
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Update record in menu table
        /// </summary>
        /// <param name="menuEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update", Name = "account.menu.update")]
        [AuthorizeAPI(pageName: "Menu", pageAccess: PageAccessValues.Update)]

        public async Task<Response> Update(MenuEntity menuEntity)
        {
            Response response;
            try
            {
                MenuBusiness menuBusiness = new MenuBusiness(Startup.Configuration);
                response = new Response(await menuBusiness.Update(menuEntity));
                Cache.Menu.Refresh();
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Delete record from menu table.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete/{Id:int}", Name = "account.menu.delete")]
        [AuthorizeAPI(pageName: "Menu", pageAccess: PageAccessValues.Delete)]

        public async Task<Response> Delete(int Id)
        {
            Response response;
            try
            {
                MenuBusiness menuBusiness = new MenuBusiness(Startup.Configuration);
                await menuBusiness.Delete(Id);
                response = new Response();
                Cache.Menu.Refresh();
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
        #endregion

        #region Interface other methods
        /// <summary>
        /// Get only parent menu list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getParent", Name = "account.menu.getParent")]
        [AuthorizeAPI(pageName: "Menu", pageAccess: PageAccessValues.View)]

        public async Task<Response> GetParent()
        {
            Response response;
            try
            {
                MenuBusiness menuBusiness = new MenuBusiness(Startup.Configuration);
                response = new Response(await menuBusiness.SelectParent());
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get child menu list by parent menu id.
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getChild/{ParentId:int}", Name = "account.menu.getChild")]
        [AuthorizeAPI(pageName: "Menu", pageAccess: PageAccessValues.View)]

        public async Task<Response> GetChildByParentId(int ParentId)
        {
            Response response;
            try
            {
                MenuBusiness menuBusiness = new MenuBusiness(Startup.Configuration);
                response = new Response(await menuBusiness.SelectChild(ParentId));
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
