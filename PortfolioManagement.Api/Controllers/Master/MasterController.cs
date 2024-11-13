﻿using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PortfolioManagement.Business.Master;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Entity.Master;
using PortfolioManagement.Business.Transaction.StockTransaction;
using PortfolioManagement.Entity.Transaction.StockTransaction;
namespace PortfolioManagement.Api.Controllers.Master
{
    [Route("master/master")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        #region Interface public methods

        /// Insert a record into the master table.
        [HttpPost]
        [Route("insert", Name = "master.master.insert")]
        [AuthorizeAPI(pageName: "Master values", pageAccess: PageAccessValues.Insert)]
        public async Task<Response> Insert(MasterEntity masterEntity )
        {
            Response response;
            try
            {
                MasterBusiness masterBusiness = new MasterBusiness(Startup.Configuration);
                response = new Response(await masterBusiness.Insert(masterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
        /// Update a record in the master table.

        [HttpPost]
        [Route("update", Name = "master.master.update")]
        [AuthorizeAPI(pageName: "Master values", pageAccess: PageAccessValues.Update)]
        public async Task<Response> Update(MasterEntity masterEntity)
        {
            Response response;
            try
            {
                MasterBusiness masterBusiness = new MasterBusiness(Startup.Configuration);
                response = new Response(await masterBusiness.Update(masterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// Delete a record from the master table.
        [HttpPost]
        [Route("delete/{id}", Name = "master.master.delete")]
        [AuthorizeAPI(pageName: "Master values", pageAccess: PageAccessValues.Delete)]
        public async Task<Response> Delete(int id)
        {
            Response response;
            try
            {
                MasterBusiness masterBusiness = new MasterBusiness(Startup.Configuration);
                await masterBusiness.Delete(id);
                response = new Response();
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpPost]
        [Route("getGridData", Name = "master.master.gridData")]
        [AuthorizeAPI(pageName: "Master values", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForGrid()
        {
            Response response;
            try
            {
                MasterBusiness masterBusiness = new MasterBusiness(Startup.Configuration);
                response = new Response(await masterBusiness.SelectForGrid());
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpGet]
        [Route("getRecord/{id:int}", Name = "master.master.record")]
        [AuthorizeAPI(pageName: "Master values", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForRecord(short id)
        {
            Response response;
            try
            {
                MasterBusiness masterBusiness = new MasterBusiness(Startup.Configuration);
                response = new Response(await masterBusiness.SelectForRecord(id));
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
