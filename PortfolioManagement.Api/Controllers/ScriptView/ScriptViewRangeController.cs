using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Business.ScriptView;
using PortfolioManagement.Repository.ScriptView;

namespace PortfolioManagement.Api.Controllers.ScriptView
{
    [Route("scriptView/range")]
    [ApiController]
    public class ScriptViewRangeController : ControllerBase
    {
        IScriptViewRangeRepository scriptViewRangeRepository;

        public ScriptViewRangeController(IScriptViewRangeRepository scriptViewRangeRepository)
        {
            this.scriptViewRangeRepository = scriptViewRangeRepository;
        }

        [HttpGet]
        [Route("get/{id:int}", Name = "ScriptViewRange.record")]
        //[authorizeapi(pagename: "scriptviewrange", pageaccess: pageaccessvalues.view)]

        public async Task<Response> GetForRange(int id)
        {
            Response response;
            try
            {
                response = new Response(await scriptViewRangeRepository.SelectForRange(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

    }
}
