using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Business.ScriptView;

namespace PortfolioManagement.Api.Controllers.ScriptView
{
    [Route("scriptView/range")]
    [ApiController]
    public class ScriptViewRangeController : ControllerBase
    {
        [HttpGet]
        [Route("get/{id:int}", Name = "ScriptViewRange.record")]
        //[AuthorizeAPI(pageName: "scriptViewRange", pageAccess: PageAccessValues.View)]

        public async Task<Response> GetForRange(int id)
        {
            Response response;
            try
            {
                ScriptViewRangeBusiness scriptViewRangeBusiness = new ScriptViewRangeBusiness(Startup.Configuration);
                response = new Response(await scriptViewRangeBusiness.SelectForRange(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

    }
}
