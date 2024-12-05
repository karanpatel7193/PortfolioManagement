using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Business.Index;
using PortfolioManagement.Entity.Index;
using PortfolioManagement.Repository.Index;

namespace PortfolioManagement.Api.Controllers.Index
{
    [Route("index/chart")]
    [ApiController]
    public class IndexChartController : ControllerBase
    {
        IIndexChartRepository indexChartRepository;
        public IndexChartController(IIndexChartRepository indexChartRepository)
        {
            this.indexChartRepository = indexChartRepository;
        }
        [HttpPost]
        [Route("getIndexChart", Name = "index.chart")]
        // [AuthorizeAPI(pageName: "Header", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForIndexChart(IndexChartParameterEntity indexChartParameterEntity)
        {
            Response response;
            try
            {
                response = new Response(await indexChartRepository.SelectForIndexChart(indexChartParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

    }
}
