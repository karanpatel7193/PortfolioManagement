using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.ScriptView;

namespace PortfolioManagement.Api.Controllers.ScriptView
{
    [Route("scriptView/CorporateAction")]
    [ApiController]
    public class ScriptViewCorporateActionController : ControllerBase
    {
        [HttpGet]
        [Route("bonus/{id:int}", Name = "ScriptView.bonus")] 
        [AuthorizeAPI(pageName: "ScriptView", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForBonus(int id)
        {
            Response response;
            try
            {
                ScriptViewCorporateActionBusiness scriptViewCorporateActionBusiness = new ScriptViewCorporateActionBusiness(Startup.Configuration);
                response = new Response(await scriptViewCorporateActionBusiness.SelectForBonus(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpGet]
        [Route("split/{id:int}", Name = "ScriptView.split")]
        [AuthorizeAPI(pageName: "ScriptView", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForSplit(int id)
        {
            Response response;
            try
            {
                ScriptViewCorporateActionBusiness scriptViewCorporateActionBusiness = new ScriptViewCorporateActionBusiness(Startup.Configuration);
                response = new Response(await scriptViewCorporateActionBusiness.SelectForSplit(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
