using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.ScriptView;
using PortfolioManagement.Business.Transaction.StockTransaction;
using PortfolioManagement.Entity.ScriptView;
using PortfolioManagement.Entity.Transaction.StockTransaction;
using PortfolioManagement.Repository.ScriptView;

namespace PortfolioManagement.Api.Controllers.ScriptView
{
     [Route("scriptView/chart")]
     [ApiController]
    public class ScriptViewChartController
    {
        IScriptViewChartRepository scriptViewChartRepository;
        public ScriptViewChartController(IScriptViewChartRepository scriptViewChartRepository)
        {

            this.scriptViewChartRepository = scriptViewChartRepository;

        }
        [HttpPost]
        [Route("getForChart", Name = "scriptView.chartData")]
        //[AuthorizeAPI(pageName: "ScriptView", pageAccess: PageAccessValues.IgnoreAuthentication)]
        public async Task<Response> GetForChart(ScriptViewParameterEntity scriptViewParameterEntity)
        {
            Response response;
            try
            {
                response = new Response(await scriptViewChartRepository.SelectForChart(scriptViewParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
