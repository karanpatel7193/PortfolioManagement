using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Portfolio;
using PortfolioManagement.Entity.Transaction;
using PortfolioManagement.Entity.Transaction.StockTransaction;
using PortfolioManagement.Repository.Portfolio;

namespace PortfolioManagement.Api.Controllers.Portfolio
{
    [Route("portfolio/portfolioDatewise")]
    [ApiController]
    public class PortfolioDatewiseController : ControllerBase
    {
        IPortfolioDatewiseReository portfolioDatewiseReository;
        public PortfolioDatewiseController(IPortfolioDatewiseReository portfolioDatewiseReository)
        {
            this.portfolioDatewiseReository = portfolioDatewiseReository;
        }
        [HttpPost]
        [Route("getPortfolioDatewiseReport", Name = "portfolioDatewise.getPortfolioDatewiseReport")]
        [AuthorizeAPI(pageName: "Portfolio Report", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetPortfolioDatewiseReport(PortfolioDatewiseParameterEntity portfolioDatewiseParameterEntity)
        {
            Response response;
            try
            {
                portfolioDatewiseParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                response = new Response(await portfolioDatewiseReository.SelectForPortfolioDatewiseData(portfolioDatewiseParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
