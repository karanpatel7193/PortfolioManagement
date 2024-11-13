using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Transaction;
using PortfolioManagement.Business.Transaction.StockTransaction;
using PortfolioManagement.Entity.Transaction;
using PortfolioManagement.Entity.Transaction.StockTransaction;

namespace PortfolioManagement.Api.Controllers.Portfolio
{
    [Route("portfolio/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {

        [HttpPost]
        [Route("getPortfolioReport", Name = "portfolio.getPortfolioReport")]
        [AuthorizeAPI(pageName: "Portfolio Report", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetPortfolioReport(StockTransactionParameterEntity transactionParameterEntity)
        {
            Response response;
            try
            {
                transactionParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                ProtfolioBusiness protfolioBusiness = new ProtfolioBusiness(Startup.Configuration);
                response = new Response(await protfolioBusiness.SelectPortfolioReport(transactionParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
