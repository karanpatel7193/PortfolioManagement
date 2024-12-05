using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Account;
using PortfolioManagement.Entity.Account;
using PortfolioManagement.Repository.Account;

namespace PortfolioManagement.Api.Controllers.Account
{
    [Route("account/[controller]")]
    [ApiController]
    public class PmsController : ControllerBase
    {
        IPmsRepository pmsRepository;

        public PmsController(IPmsRepository pmsRepository)
        {
            this.pmsRepository = pmsRepository;
        }
        [HttpGet]
        [Route("getRecord/{Id:int}", Name = "account.pms.record")]
        [AuthorizeAPI(pageName: "PMS", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForRecord(int Id)
        {
            Response response;
            try
            {
                response = new Response(await pmsRepository.SelectForRecord(Id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpPost]
        [Route("getGridData", Name = "account.pms.gridData")]
        [AuthorizeAPI(pageName: "PMS", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForGrid(PmsParameterEntity pmsParameterEntity)
        {
            Response response;
            try
            {
                response = new Response(await pmsRepository.SelectForGrid(pmsParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpPost]
        [Route("insert", Name = "account.pms.insert")]
        [AuthorizeAPI(pageName: "PMS", pageAccess: PageAccessValues.Insert)]
        public async Task<Response> Insert(PmsEntity pmsEntity)
        {
            Response response;
            try
            {
                response = new Response(await pmsRepository.Insert(pmsEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpPost]
        [Route("update", Name = "account.pms.update")]
        [AuthorizeAPI(pageName: "PMS", pageAccess: PageAccessValues.Update)]
        public async Task<Response> Update(PmsEntity pmsEntity)
        {
            Response response;
            try
            {
                response = new Response(await pmsRepository.Update(pmsEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpPost]
        [Route("delete/{Id:int}", Name = "account.pms.delete")]
        [AuthorizeAPI(pageName: "PMS", pageAccess: PageAccessValues.Delete)]
        public async Task<Response> Delete(int Id)
        {
            Response response;
            try
            {
                await pmsRepository.Delete(Id);
                response = new Response();
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
    }
}
