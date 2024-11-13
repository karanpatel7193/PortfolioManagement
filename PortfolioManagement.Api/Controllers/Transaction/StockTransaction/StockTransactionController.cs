    using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Account;
using PortfolioManagement.Business.Transaction.StockTransaction;
using PortfolioManagement.Entity.Account;
using PortfolioManagement.Entity.Transaction;
using PortfolioManagement.Entity.Transaction.StockTransaction;

namespace PortfolioManagement.Api.Controllers.Transaction.StockTransaction
{
    [Route("stockTransaction/stockTransaction")]
    [ApiController]
    public class StockTransactionController : ControllerBase
    {
        [HttpGet]
        [Route("getRecord/{id:int}", Name = "stockTransaction.stockTransaction.record")]
        [AuthorizeAPI(pageName: "Stock", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForRecord(int id)
        {
            Response response;
            try
            {
                StockTransactionBusiness stockTransactionBusiness = new StockTransactionBusiness(Startup.Configuration);
                response = new Response(await stockTransactionBusiness.SelectForRecord(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpPost]
        [Route("getLovValue", Name = "stockTransaction.stockTransaction.lovValue")]
        [AuthorizeAPI(pageName: "Stock", pageAccess: PageAccessValues.IgnoreAuthorization)]
        public async Task<Response> GetForLOV()
        {
            Response response;
            try
            {
                StockTransactionBusiness stockTransactionBusiness = new StockTransactionBusiness(Startup.Configuration);
                response = new Response(await stockTransactionBusiness.SelectForLOV());
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }


        [HttpPost]
        [Route("getGridData", Name = "stockTransaction.stockTransaction.gridData")]
        [AuthorizeAPI(pageName: "Stock", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForGrid(StockTransactionParameterEntity stockTransactionParameterEntity)
        {
            Response response;
            try
            {
                stockTransactionParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                StockTransactionBusiness stockTransactionBusiness = new StockTransactionBusiness(Startup.Configuration);
                response = new Response(await stockTransactionBusiness.SelectForGrid(stockTransactionParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpPost]
        [Route("insert", Name = "stockTransaction.stockTransaction.insert")]
        [AuthorizeAPI(pageName: "Stock", pageAccess: PageAccessValues.Insert)]
        public async Task<Response> Insert(StockTransactionEntity stockTransactionEntity)
        {
            Response response;
            try
            {
                stockTransactionEntity.PmsId = AuthenticateCliam.PmsId(Request);
                StockTransactionBusiness stockTransactionBusiness = new StockTransactionBusiness(Startup.Configuration);
                response = new Response(await stockTransactionBusiness.Insert(stockTransactionEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpPost]
        [Route("update", Name = "stockTransaction.stockTransaction.update")]
        [AuthorizeAPI(pageName: "Stock", pageAccess: PageAccessValues.Update)]
        public async Task<Response> Update(StockTransactionEntity stockTransactionEntity)
        {
            Response response;
            try
            {
                StockTransactionBusiness stockTransactionBusiness = new StockTransactionBusiness(Startup.Configuration);
                response = new Response(await stockTransactionBusiness.Update(stockTransactionEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }


        [HttpPost]
        [Route("delete/{id:int}", Name = "stockTransaction.stockTransaction.delete")]
        [AuthorizeAPI(pageName: "Stock", pageAccess: PageAccessValues.Delete)]
        public async Task<Response> Delete(int id)
        {
            Response response;
            try
            {
                StockTransactionBusiness stockTransactionBusiness = new StockTransactionBusiness(Startup.Configuration);
                await stockTransactionBusiness.Delete(id);
                response = new Response();
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
       
        [HttpPost]
        [Route("getForList", Name = "stockTransaction.stockTransaction.getforlist")]
        [AuthorizeAPI(pageName: "Stock", pageAccess: PageAccessValues.View)]
        public async Task<Response> getForList(StockTransactionParameterEntity stockTransactionParameterEntity)
        {
            Response response;
            try
            {
                stockTransactionParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                StockTransactionBusiness stockTransactionBusiness = new StockTransactionBusiness(Startup.Configuration);
                response = new Response(await stockTransactionBusiness.
                    SelectForList(stockTransactionParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpPost]
        [Route("getReportData", Name = "stockTransaction.stockTransaction.reportData")]
        [AuthorizeAPI(pageName: "Stock Transaction Report", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForReport(StockTransactionParameterEntity stockTransactionParameterEntity)
        {
            Response response;
            try
            {
                stockTransactionParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                StockTransactionBusiness stockTransactionBusiness = new StockTransactionBusiness(Startup.Configuration);
                response = new Response(await stockTransactionBusiness.SelectForReport(stockTransactionParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
        
        [HttpPost]
        [Route("getSummaryData", Name = "stockTransaction.stockTransaction.summaryData")]
        [AuthorizeAPI(pageName: "Stock", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForSummary(StockTransactionParameterEntity transactionParameterEntity)
        {
            Response response;
            try
            {
                transactionParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                StockTransactionBusiness stockTransactionBusiness = new StockTransactionBusiness(Startup.Configuration);
                response = new Response(await stockTransactionBusiness.SelectForSummary(transactionParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }




    }
}
