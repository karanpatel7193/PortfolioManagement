using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Business.Index;
using PortfolioManagement.Entity.Index;

namespace PortfolioManagement.Api.Controllers.Index
{
    [Route("index/chart")]
    [ApiController]
    public class IndexChartController : ControllerBase
    {
        [HttpPost]
        [Route("getIndexChart", Name = "index.chart")]
        // [AuthorizeAPI(pageName: "Header", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForIndexChart(IndexChartParameterEntity indexChartParameterEntity)
        {
            Response response;
            try
            {
                IndexChartBusiness indexChartBusiness = new IndexChartBusiness(Startup.Configuration);
                response = new Response(await indexChartBusiness.SelectForIndexChart(indexChartParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

    }
}
