//using CommonLibrary;
//using Microsoft.AspNetCore.Mvc;
//using PortfolioManagement.Api.Common;
//using PortfolioManagement.Business.Master;
//using PortfolioManagement.Entity.Master;

//namespace PortfolioManagement.Api.Controllers.Master
//{
//    [Route("master/accountbroker")]
//    [ApiController]

//    public class AccountBrokerController : ControllerBase
//    {
//        #region Interface public methods

//        /// Get all columns information for a particular account broker record.
//        [HttpGet]
//        [Route("getRecord/{id}", Name = "master.accountbroker.record")]
//        [AuthorizeAPI(pageName: "AccountBroker", pageAccess: PageAccessValues.View)]
//        public async Task<Response> GetForRecord(int id)
//        {
//            Response response;
//            try
//            {
//                AccountBrokerBusiness accountBrokerBusiness = new AccountBrokerBusiness(Startup.Configuration);
//                response = new Response(await accountBrokerBusiness.SelectForRecord(id));
//            }
//            catch (Exception ex)
//            {
//                response = new Response(await ex.WriteLogFileAsync(), ex);
//            }
//            return response;
//        }

//        ///ListDAtaOfBrokerID

//        [HttpPost]
//        [Route("getBrokerListValue", Name = "master.accountbroker.listValueBroker")]
//        [AuthorizeAPI(pageName: "AccountBroker", pageAccess: PageAccessValues.IgnoreAuthorization)]
//        public async Task<Response> GetForListBroker()
//        {
//            Response response;
//            try
//            {
//                AccountBrokerBusiness accountBrokerBusiness = new AccountBrokerBusiness(Startup.Configuration);
//                response = new Response(await accountBrokerBusiness.SelectForListBroker());
//            }
//            catch (Exception ex)
//            {
//                response = new Response(await ex.WriteLogFileAsync(), ex);
//            }
//            return response;
//        }

//        ///ListDataOfAccountID

//        [HttpPost]
//        [Route("getAccountListValue", Name = "master.accountbroker.listValueAccount")]
//        [AuthorizeAPI(pageName: "AccountBroker", pageAccess: PageAccessValues.IgnoreAuthorization)]
//        public async Task<Response> GetForListAccount()
//            {
//            Response response;
//            try
//            {
//                AccountBrokerBusiness accountBrokerBusiness = new AccountBrokerBusiness(Startup.Configuration);
//                response = new Response(await accountBrokerBusiness.SelectForListAccount());
//            }
//            catch (Exception ex)
//            {
//                response = new Response(await ex.WriteLogFileAsync(), ex);
//            }
//            return response;
//        }

//        /// Get paginated and sorted data for AccountBroker.
//        [HttpPost]
//        [Route("getGridData", Name = "master.accountbroker.gridData")]
//        [AuthorizeAPI(pageName: "AccountBroker", pageAccess: PageAccessValues.View)]
//        public async Task<Response> GetForGrid(AccountBrokerParameterEntity accountBrokerParameterEntity)
//        {
//            Response response;
//            try
//            {
//                AccountBrokerBusiness accountBrokerBusiness = new AccountBrokerBusiness(Startup.Configuration);
//                response = new Response(await accountBrokerBusiness.SelectForGrid(accountBrokerParameterEntity));
//            }
//            catch (Exception ex)
//            {
//                response = new Response(await ex.WriteLogFileAsync(), ex);
//            }
//            return response;
//        }

//        /// Insert a record into the AccountBroker table.
//        [HttpPost]
//        [Route("insert", Name = "master.accountbroker.insert")]
//        [AuthorizeAPI(pageName: "AccountBroker", pageAccess: PageAccessValues.Insert)]
//        public async Task<Response> Insert(AccountBrokerEntity accountBrokerEntity)
//        {
//            Response response;
//            try
//            {
//                AccountBrokerBusiness accountBrokerBusiness = new AccountBrokerBusiness(Startup.Configuration);
//                response = new Response(await accountBrokerBusiness.Insert(accountBrokerEntity));
//            }
//            catch (Exception ex)
//            {
//                response = new Response(await ex.WriteLogFileAsync(), ex);
//            }
//            return response;
//        }

//        /// Update a record in the AccountBroker table.
//        [HttpPost]
//        [Route("update", Name = "master.accountbroker.update")]
//        [AuthorizeAPI(pageName: "AccountBroker", pageAccess: PageAccessValues.Update)]
//        public async Task<Response> Update(AccountBrokerEntity accountBrokerEntity)
//        {
//            Response response;
//            try
//            {
//                AccountBrokerBusiness accountBrokerBusiness = new AccountBrokerBusiness(Startup.Configuration);
//                response = new Response(await accountBrokerBusiness.Update(accountBrokerEntity));
//            }
//            catch (Exception ex)
//            {
//                response = new Response(await ex.WriteLogFileAsync(), ex);
//            }
//            return response;
//        }

//        /// Delete a record from the AccountBroker table.
//        [HttpPost]
//        [Route("delete/{id}", Name = "master.accountbroker.delete")]
//        [AuthorizeAPI(pageName: "AccountBroker", pageAccess: PageAccessValues.Delete)]
//        public async Task<Response> Delete(int id)
//        {
//            Response response;
//            try
//            {
//                AccountBrokerBusiness accountBrokerBusiness = new AccountBrokerBusiness(Startup.Configuration);
//                await accountBrokerBusiness.Delete(id);
//                response = new Response();
//            }
//            catch (Exception ex)
//            {
//                response = new Response(await ex.WriteLogFileAsync(), ex);
//            }
//            return response;
//        }

//        #endregion
//    }
//}

