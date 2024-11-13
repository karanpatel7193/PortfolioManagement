using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.ScriptView;

namespace PortfolioManagement.Api.Controllers.ScriptView
{
    [Route("scriptView/aboutCompany")]
    [ApiController]
    public class ScriptViewAboutCompanyController : ControllerBase
    {
        [HttpGet]
        [Route("get/{id:int}", Name = "scriptViewAboutCompany.record")]
        [AuthorizeAPI(pageName: "ScriptView", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForAboutCompany(int id)
        {
            Response response;
            try
            {
                ScriptViewAboutCompanyBusiness scriptViewAboutCompanyBusiness = new ScriptViewAboutCompanyBusiness(Startup.Configuration);
                response = new Response(await scriptViewAboutCompanyBusiness.SelectForAboutCompany(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
