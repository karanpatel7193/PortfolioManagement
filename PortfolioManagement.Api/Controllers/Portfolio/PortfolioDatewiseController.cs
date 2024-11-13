using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Transaction;
using PortfolioManagement.Entity.Transaction;
using PortfolioManagement.Entity.Transaction.StockTransaction;

namespace PortfolioManagement.Api.Controllers.Portfolio
{
    [Route("portfolio/portfolioDatewise")]
    [ApiController]
    public class PortfolioDatewiseController : ControllerBase
    {
        [HttpPost]
        [Route("getPortfolioDatewiseReport", Name = "portfolioDatewise.getPortfolioDatewiseReport")]
        [AuthorizeAPI(pageName: "Portfolio Report", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetPortfolioDatewiseReport(PortfolioDatewiseParameterEntity portfolioDatewiseParameterEntity)
        {
            Response response;
            try
            {
                portfolioDatewiseParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                PortfolioDatewiseBusiness portfolioDatewiseBusiness = new PortfolioDatewiseBusiness(Startup.Configuration);
                response = new Response(await portfolioDatewiseBusiness.SelectForPortfolioDatewiseData(portfolioDatewiseParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
