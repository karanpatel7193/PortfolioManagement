using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Business.IndexView;
using PortfolioManagement.Business.ScriptView;
using PortfolioManagement.Entity.IndexView;
using PortfolioManagement.Entity.ScriptView;

namespace PortfolioManagement.Api.Controllers.IndexView
{
    [Route("IndexView/indexChart")]
    [ApiController]
    public class IndexViewChartController : ControllerBase
    {
        [HttpPost]
        [Route("getForIndexChart", Name = "indexView.indexChartData")]
        //[AuthorizeAPI(pageName: "ScriptView", pageAccess: PageAccessValues.IgnoreAuthentication)]
        public async Task<Response> GetForIndexChart(IndexViewParameterEntity indexViewParameterEntity)
        {
            Response response;
            try
            {
                IndexViewChartBusiness indexViewChartBusiness = new IndexViewChartBusiness(Startup.Configuration);
                response = new Response(await indexViewChartBusiness.SelectForIndexChart(indexViewParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
