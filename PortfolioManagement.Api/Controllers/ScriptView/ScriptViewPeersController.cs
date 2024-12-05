using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.ScriptView;
using PortfolioManagement.Repository.ScriptView;

namespace PortfolioManagement.Api.Controllers.ScriptView
{
    [Route("scriptView/peers")]
    [ApiController]
    public class ScriptViewPeersController : ControllerBase
    {
        IScriptViewPeersRepository scriptViewPeersRepository;
        public ScriptViewPeersController(IScriptViewPeersRepository scriptViewPeersRepository)
        {
            this.scriptViewPeersRepository = scriptViewPeersRepository;
        }

        [HttpGet]
        [Route("get/{id:int}", Name = "scriptViewPeers.record")]
        [AuthorizeAPI(pageName:"ScriptView", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForPeer(int id)
        {
            Response response;
            try
            {
                response = new Response(await scriptViewPeersRepository.SelectForPeers(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
