﻿using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PortfolioManagement.Business.Master;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Entity.Master;
namespace PortfolioManagement.Api.Controllers.Master
{
    [Route("master/broker")]
    [ApiController]
    public class BrokerController : ControllerBase
    {
        #region Interface public methods

        /// Get all columns information for a particular broker record.
        [HttpGet]
        [Route("getRecord/{id}", Name = "master.broker.record")]
        [AuthorizeAPI(pageName: "Broker", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForRecord(int id)
        {
            Response response;
            try
            {
                BrokerBusiness brokerBusiness = new BrokerBusiness(Startup.Configuration);
                response = new Response(await brokerBusiness.SelectForRecord(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
        // lovdata

        [HttpPost]
        [Route("getLovValue", Name = "master.broker.lovValue")]
        [AuthorizeAPI(pageName: "Broker", pageAccess: PageAccessValues.IgnoreAuthorization)]
        public async Task<Response> GetForLOV(BrokerParameterEntity brokerParameterEntity)
        {
            Response response;
            try
            {
                brokerParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                BrokerBusiness brokerBusiness = new BrokerBusiness(Startup.Configuration);
                response = new Response(await brokerBusiness.SelectForLOV(brokerParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        //grid data 
        [HttpPost]
        [Route("getGridData", Name = "master.broker.gridData")]
        [AuthorizeAPI(pageName: "Broker", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForGrid(BrokerParameterEntity brokerParameterEntity)
        {
            Response response;
            try
            {
                brokerParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                BrokerBusiness brokerBusiness = new BrokerBusiness(Startup.Configuration);
                response = new Response(await brokerBusiness.SelectForGrid(brokerParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
        /// Insert a record into the broker table.
        [HttpPost]
        [Route("insert", Name = "master.broker.insert")]
        [AuthorizeAPI(pageName: "Broker", pageAccess: PageAccessValues.Insert)]
        public async Task<Response> Insert(BrokerEntity brokerEntity)
        {
            Response response;
            try
            {
                brokerEntity.PmsId = AuthenticateCliam.PmsId(Request);
                BrokerBusiness brokerBusiness = new BrokerBusiness(Startup.Configuration);
                response = new Response(await brokerBusiness.Insert(brokerEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
        /// Update a record in the broker table.
        [HttpPost]
        [Route("update", Name = "master.broker.update")]
        [AuthorizeAPI(pageName: "Broker", pageAccess: PageAccessValues.Update)]
        public async Task<Response> Update(BrokerEntity brokerEntity)
        {
            Response response;
            try
            {
                BrokerBusiness brokerBusiness = new BrokerBusiness(Startup.Configuration);
                response = new Response(await brokerBusiness.Update(brokerEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// Delete a record from the broker table.
        [HttpPost]
        [Route("delete/{id}", Name = "master.broker.delete")]
        [AuthorizeAPI(pageName: "Broker", pageAccess: PageAccessValues.Delete)]
        public async Task<Response> Delete(Byte id)
        {
            Response response;
            try
            {
                BrokerBusiness brokerBusiness = new BrokerBusiness(Startup.Configuration);
                await brokerBusiness.Delete(id);
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