using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.ScriptView;
using PortfolioManagement.Repository.ScriptView;

namespace PortfolioManagement.Api.Controllers.ScriptView
{
    [Route("scriptView/aboutCompany")]
    [ApiController]
    public class ScriptViewAboutCompanyController : ControllerBase
    {
        IScriptViewAboutCompanyRpository scriptViewAboutCompanyRpository;
        public ScriptViewAboutCompanyController(IScriptViewAboutCompanyRpository scriptViewAboutCompanyRpository)
        {
            this.scriptViewAboutCompanyRpository = scriptViewAboutCompanyRpository;
        }

        [HttpGet]
        [Route("get/{id:int}", Name = "scriptViewAboutCompany.record")]
        [AuthorizeAPI(pageName: "ScriptView", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForAboutCompany(int id)
        {
            Response response;
            try
            {
                response = new Response(await scriptViewAboutCompanyRpository.SelectForAboutCompany(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
