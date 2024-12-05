using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.ScriptView;
using PortfolioManagement.Repository.ScriptView;

namespace PortfolioManagement.Api.Controllers.ScriptView
{

    [Route("scriptView/CorporateAction")]
    [ApiController]
    public class ScriptViewCorporateActionController : ControllerBase
    {
        IScriptViewCorporateActionRepository scriptViewCorporateActionRepository;
        public ScriptViewCorporateActionController(IScriptViewCorporateActionRepository scriptViewCorporateActionRepository)
        {
            this.scriptViewCorporateActionRepository = scriptViewCorporateActionRepository;
        }

        [HttpGet]
        [Route("bonus/{id:int}", Name = "ScriptView.bonus")] 
        [AuthorizeAPI(pageName: "ScriptView", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForBonus(int id)
        {
            Response response;
            try
            {
                response = new Response(await scriptViewCorporateActionRepository.SelectForBonus(id));
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
                response = new Response(await scriptViewCorporateActionRepository.SelectForSplit(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
