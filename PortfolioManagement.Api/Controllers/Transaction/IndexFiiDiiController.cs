using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Master;
using PortfolioManagement.Business.Transaction;
using PortfolioManagement.Entity.Master;
using PortfolioManagement.Entity.Transaction;
using PortfolioManagement.Repository.Transaction;

namespace PortfolioManagement.Api.Controllers.Transaction
{
    [Route("transaction/[controller]")]
    [ApiController]
    public class IndexFiiDiiController : ControllerBase
    {
        IIndexFiiDiiRepository indexFiiDiiRepository;
        public IndexFiiDiiController(IIndexFiiDiiRepository indexFiiDiiRepository)
        {
            this.indexFiiDiiRepository = indexFiiDiiRepository;
        }

        [HttpPost]
        [Route("getChartData", Name = "transaction.indexfiidii.getChartData")]
        public async Task<Response> GetForChart(IndexFiiDiiParameterEntity indexFiiDiiParameterEntity)
        {
            Response response;
            try
            {
                response = new Response(await indexFiiDiiRepository.SelectForChart(indexFiiDiiParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
