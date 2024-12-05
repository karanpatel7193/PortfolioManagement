using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Portfolio;
using PortfolioManagement.Business.Transaction.StockTransaction;
using PortfolioManagement.Entity.Transaction;
using PortfolioManagement.Entity.Transaction.StockTransaction;
using PortfolioManagement.Repository.Portfolio;

namespace PortfolioManagement.Api.Controllers.Portfolio
{
    [Route("portfolio/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        IPortfolioRepository portfolioRepository;
        public PortfolioController(IPortfolioRepository portfolioRepository)
        {
            this.portfolioRepository = portfolioRepository;
        }

        [HttpPost]
        [Route("getPortfolioReport", Name = "portfolio.getPortfolioReport")]
        [AuthorizeAPI(pageName: "Portfolio Report", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetPortfolioReport(StockTransactionParameterEntity transactionParameterEntity)
        {
            Response response;
            try
            {
                transactionParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                response = new Response(await portfolioRepository.SelectPortfolioReport(transactionParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
