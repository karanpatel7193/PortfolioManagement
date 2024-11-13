using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.ScriptView;

namespace PortfolioManagement.Api.Controllers.ScriptView
{
    [Route("scriptView/peers")]
    [ApiController]
    public class ScriptViewPeersController : ControllerBase
    {

        [HttpGet]
        [Route("get/{id:int}", Name = "scriptViewPeers.record")]
        [AuthorizeAPI(pageName:"ScriptView", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForPeer(int id)
        {
            Response response;
            try
            {
                ScriptViewPeersBusiness scriptViewPeersBusiness = new ScriptViewPeersBusiness(Startup.Configuration);
                response = new Response(await scriptViewPeersBusiness.SelectForPeers(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
