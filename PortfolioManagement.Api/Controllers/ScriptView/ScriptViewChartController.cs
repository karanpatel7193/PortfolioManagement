using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.ScriptView;
using PortfolioManagement.Business.Transaction.StockTransaction;
using PortfolioManagement.Entity.ScriptView;
using PortfolioManagement.Entity.Transaction.StockTransaction;

namespace PortfolioManagement.Api.Controllers.ScriptView
{
     [Route("scriptView/chart")]
     [ApiController]
    public class ScriptViewChartController
    {
        [HttpPost]
        [Route("getForChart", Name = "scriptView.chartData")]
        //[AuthorizeAPI(pageName: "ScriptView", pageAccess: PageAccessValues.IgnoreAuthentication)]
        public async Task<Response> GetForChart(ScriptViewParameterEntity scriptViewParameterEntity)
        {
            Response response;
            try
            {
                ScriptViewChartBusiness scriptViewChartBusiness = new ScriptViewChartBusiness(Startup.Configuration);
                response = new Response(await scriptViewChartBusiness.SelectForChart(scriptViewParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
