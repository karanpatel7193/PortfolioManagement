using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Master;
using PortfolioManagement.Entity.Master;

namespace PortfolioManagement.Api.Controllers.Master
{
    /// <summary>
    /// This API use for script related operation like list, insert, update, delete script from database etc.
    /// </summary>
	[Route("master/script")]
    [ApiController]
    public class ScriptController : ControllerBase
    {
        #region Interface public methods
        /// <summary>
        /// Get all columns information for particular script record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getRecord/{id:int}", Name = "master.script.record")]
        [AuthorizeAPI(pageName: "Script", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForRecord(int id)
        {
            Response response;
            try
            {
                ScriptBusiness scriptBusiness = new ScriptBusiness(Startup.Configuration);
                response = new Response(await scriptBusiness.SelectForRecord(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get main columns informations for bind script LOV
        /// </summary>
        /// <param name="scriptParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getLovValue", Name = "master.script.lovValue")]
        [AuthorizeAPI(pageName: "Script", pageAccess: PageAccessValues.IgnoreAuthorization)]
        public async Task<Response> GetForLOV(ScriptParameterEntity scriptParameterEntity)
        {
            Response response;
            try
            {
                ScriptBusiness scriptBusiness = new ScriptBusiness(Startup.Configuration);
                response = new Response(await scriptBusiness.SelectForLOV(scriptParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get script list for bind grid.
        /// </summary>
        /// <param name="scriptParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getGridData", Name = "master.script.gridData")]
        [AuthorizeAPI(pageName: "Script", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForGrid(ScriptParameterEntity scriptParameterEntity)
        {
            Response response;
            try
            {
                ScriptBusiness scriptBusiness = new ScriptBusiness(Startup.Configuration);
                response = new Response(await scriptBusiness.SelectForGrid(scriptParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Insert record in script table.
        /// </summary>
        /// <param name="scriptEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("insert", Name = "master.script.insert")]
        [AuthorizeAPI(pageName: "Script", pageAccess: PageAccessValues.Insert)]
        public async Task<Response> Insert(ScriptEntity scriptEntity)
        {
            Response response;
            try
            {
                ScriptBusiness scriptBusiness = new ScriptBusiness(Startup.Configuration);
                response = new Response(await scriptBusiness.Insert(scriptEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Update record in script table.
        /// </summary>
        /// <param name="scriptEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update", Name = "master.script.update")]
        [AuthorizeAPI(pageName: "Script", pageAccess: PageAccessValues.Update)]
        public async Task<Response> Update(ScriptEntity scriptEntity)
        {
            Response response;
            try
            {
                ScriptBusiness scriptBusiness = new ScriptBusiness(Startup.Configuration);
                response = new Response(await scriptBusiness.Update(scriptEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Delete record from script table.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete/{id:int}", Name = "master.script.delete")]
        [AuthorizeAPI(pageName: "Script", pageAccess: PageAccessValues.Delete)]
        public async Task<Response> Delete(int id)
        {
            Response response;
            try
            {
                ScriptBusiness scriptBusiness = new ScriptBusiness(Startup.Configuration);
                await scriptBusiness.Delete(id);
                response = new Response();
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
