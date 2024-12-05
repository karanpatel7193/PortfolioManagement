using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Business.Index;
using PortfolioManagement.Repository.Index;

namespace PortfolioManagement.Api.Controllers.Index
{
    [Route("index/header")]
    [ApiController]
    public class HeaderController : ControllerBase
    {
        IHeaderRepository headerRepository;
        public HeaderController(IHeaderRepository headerRepository)
        {
            this.headerRepository = headerRepository;
        }
        [HttpPost]
        [Route("getForMarquee", Name = "HeaderNifty50.record")]
        // [AuthorizeAPI(pageName: "Header", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForGrid()
        {
            Response response;
            try
            {
                response = new Response(await headerRepository.SelectForGrid());
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
                response = new Response(await headerRepository.SelectForIndex());
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
