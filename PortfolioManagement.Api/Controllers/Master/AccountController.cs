using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PortfolioManagement.Business.Master;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Entity.Master;
using PortfolioManagement.Repository.Master;
namespace PortfolioManagement.Api.Controllers.Master
{
    [Route("master/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IAccounRepository accounRepository;
        public AccountController(IAccounRepository accounRepository)
        {
            this.accounRepository = accounRepository;
        }

        #region Interface public methods

        /// Get all columns information for a particular account record.
        [HttpGet]
        [Route("getRecord/{id}", Name = "master.account.record")]
        [AuthorizeAPI(pageName: "Account", pageAccess: PageAccessValues .View)]
        public async Task<Response> GetForRecord(int id)
        {
            Response response;
            try
            {

                
                response = new Response(await accounRepository.SelectForRecord(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
                                                                                                                                                                                                                                                                                                                                                                                
        /// lov  
        [HttpPost]
        [Route("getLovValue", Name = "master.account.lovValue")]
        [AuthorizeAPI(pageName: "Account", pageAccess: PageAccessValues.IgnoreAuthorization)]
        public async Task<Response> GetForLOV(AccountParameterEntity accountParameterEntity)
        {
            Response response;
            try
            {
                accountParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                response = new Response(await accounRepository.SelectForLOV(accountParameterEntity));

            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
       
        /// Get grid data for accounts with pagination and sorting.

        [HttpPost]
        [Route("getGridData", Name = "master.account.gridData")]
        [AuthorizeAPI(pageName: "Account", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForGrid(AccountParameterEntity accountParameterEntity)
        {
            Response response;
            try
            {
                accountParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                response = new Response(await accounRepository.SelectForGrid(accountParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// Get data needed for adding a new account, such as available brokers.
        [HttpPost]
        [Route("getAddData", Name = "master.account.addData")]
        [AuthorizeAPI(pageName: "Account", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForAdd(AccountParameterEntity accountParameterEntity)
        {
            Response response;
            try
            {
                accountParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                response = new Response(await accounRepository.SelectForAdd(accountParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// Get data for editing an existing account, such as the account details and associated brokers.
        [HttpPost]
        [Route("getEditData", Name = "master.account.editData")]
        [AuthorizeAPI(pageName: "Account", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForEdit(AccountParameterEntity accountParameterEntity)
        {
            Response response;
            try
            {
                accountParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                response = new Response(await accounRepository.SelectForEdit(accountParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// Insert a record into the account table.
        [HttpPost]
        [Route("insert", Name = "master.account.insert")]
        [AuthorizeAPI(pageName: "Account", pageAccess: PageAccessValues.Insert)]
        public async Task<Response> Insert(AccountEntity accountEntity)
        {
            Response response;
            try
            {
                accountEntity.PmsId = AuthenticateCliam.PmsId(Request);
                response = new Response(await accounRepository.Insert(accountEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }



        /// Update a record in the account table.

        [HttpPost]
        [Route("update", Name = "master.account.update")]
        [AuthorizeAPI(pageName: "Account", pageAccess: PageAccessValues.Update)]
        public async Task<Response> Update(AccountEntity accountEntity)
        {
            Response response;
            try
            {
                response = new Response(await accounRepository.Update(accountEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// Delete a record from the account table.
        [HttpPost]
        [Route("delete/{id}", Name = "master.account.delete")]
        [AuthorizeAPI(pageName: "Account", pageAccess: PageAccessValues.IgnoreAuthorization)]
        public async Task<Response> Delete(int id)
        {
            Response response;
            try
            {
                await accounRepository.Delete(id);
                response = new Response();
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        #endregion
    }
}
