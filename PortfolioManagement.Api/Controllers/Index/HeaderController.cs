using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Business.Index;

namespace PortfolioManagement.Api.Controllers.Index
{
    [Route("index/header")]
    [ApiController]
    public class HeaderController : ControllerBase
    {
        [HttpPost]
        [Route("getForMarquee", Name = "HeaderNifty50.record")]
        // [AuthorizeAPI(pageName: "Header", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForGrid()
        {
            Response response;
            try
            {
                HeaderBusiness headerBusiness = new HeaderBusiness(Startup.Configuration);
                response = new Response(await headerBusiness.SelectForGrid());
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpPost]
        [Route("getForIndex", Name = "HeaderNifty50.index")]
        // [AuthorizeAPI(pageName: "Header", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForIndex()
        {
            Response response;
            try
            {
                HeaderBusiness headerBusiness = new HeaderBusiness(Startup.Configuration);
                response = new Response(await headerBusiness.SelectForIndex());
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
