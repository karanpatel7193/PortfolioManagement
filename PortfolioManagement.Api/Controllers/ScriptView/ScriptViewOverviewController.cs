using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Master;
using PortfolioManagement.Business.ScriptView;
using PortfolioManagement.Repository.ScriptView;

namespace PortfolioManagement.Api.Controllers.ScriptView
{
    [Route("scriptView/overview")]
    [ApiController]
    public class ScriptViewOverviewController : ControllerBase
    {
        IScriptViewOverviewRepository ScriptViewOverviewRepository;

        public ScriptViewOverviewController(IScriptViewOverviewRepository ScriptViewOverviewRepository)
        {
            this.ScriptViewOverviewRepository = ScriptViewOverviewRepository;
        }
        [HttpGet]
        [Route("get/{id:int}", Name = "ScriptView.record")]
        [AuthorizeAPI(pageName: "ScriptView", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForOverview(int id)
        {
            Response response;
            try
            {
                response = new Response(await ScriptViewOverviewRepository.SelectForOverview(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
