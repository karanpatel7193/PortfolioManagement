using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Transaction.StockTransaction;
using PortfolioManagement.Business.Watchlist;
using PortfolioManagement.Entity.Master;
using PortfolioManagement.Entity.Transaction.StockTransaction;
using PortfolioManagement.Entity.Watchlist;
using System;
using System.Threading.Tasks;

namespace PortfolioManagement.Api.Controllers.Watchlist
{
    [Route("watchlist")]
    [ApiController]
    public class WatchlistController : ControllerBase
    {
        #region Interface public methods

        /// GetForTab
        [HttpPost]
        [Route("getTabValue", Name = "watchlist.tabValue")]
        [AuthorizeAPI(pageName: "Watchlist", pageAccess: PageAccessValues.IgnoreAuthentication)]
        public async Task<Response> GetForTab()  
        {
            Response response;
            try
            {
                var PmsId = AuthenticateCliam.PmsId(Request);
                WatchlistBusiness watchlistBusiness = new WatchlistBusiness(Startup.Configuration);
                response = new Response(await watchlistBusiness.SelectForTab(PmsId));  
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }


        //GetForTabScript

        [HttpPost]
        [Route("getTabScriptData", Name = "watchlist.tabScriptData")]
        [AuthorizeAPI(pageName: "Watchlist", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForTabScript(WatchlistParameterEntity watchlistParameterEntity)
        {
            Response response;
            try
            {
                watchlistParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                WatchlistBusiness watchlistBusiness = new WatchlistBusiness(Startup.Configuration);
                response = new Response(await watchlistBusiness.SelectForTabScript(watchlistParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
        /// Insert a new watchlist record.
        [HttpPost]
        [Route("insert", Name = "watchlist.insert")]
        [AuthorizeAPI(pageName: "Watchlist", pageAccess: PageAccessValues.Insert)]
        public async Task<Response> Insert(WatchlistEntity watchlistEntity)
        {
            Response response;
            try
            {
                watchlistEntity.PmsId = AuthenticateCliam.PmsId(Request);
                WatchlistBusiness watchlistBusiness = new WatchlistBusiness(Startup.Configuration);
                response = new Response(await watchlistBusiness.Insert(watchlistEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// Insert a new watchlistscript record.
        [HttpPost]
        [Route("insertscript", Name = "watchlist.InsertScript")]
        //[AuthorizeAPI(pageName: "Watchlist", pageAccess: PageAccessValues.Insert)]
        public async Task<Response> InsertScript(WatchlistParameterEntity watchlistParameterEntity)
        {
            Response response;
            try
            {
                watchlistParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);

                WatchlistBusiness watchlistBusiness = new WatchlistBusiness(Startup.Configuration);
                response = new Response(await watchlistBusiness.InsertScript(watchlistParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// Update an existing watchlist record.
        [HttpPost]
        [Route("update", Name = "watchlist.update")]
        [AuthorizeAPI(pageName: "Watchlist", pageAccess: PageAccessValues.Update)]
        public async Task<Response> Update(WatchlistEntity watchlistEntity)
        {
            Response response;
            try
            {
                WatchlistBusiness watchlistBusiness = new WatchlistBusiness(Startup.Configuration);
                response = new Response(await watchlistBusiness.Update(watchlistEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// Delete a watchlist record by ID.
        [HttpPost]
        [Route("delete/{id}", Name = "watchlist.delete")]
        [AuthorizeAPI(pageName: "Watchlist", pageAccess: PageAccessValues.Delete)]
        public async Task<Response> Delete(int id)
        {
            Response response;
            try
            {
                WatchlistBusiness watchlistBusiness = new WatchlistBusiness(Startup.Configuration);
                await watchlistBusiness.Delete(id);
                response = new Response();
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// Delete a watchlistScirpt record by ID.
        [HttpPost]
        [Route("deleteScript/{id}", Name = "watchlist.deleteScript")]
        [AuthorizeAPI(pageName: "Watchlist", pageAccess: PageAccessValues.Delete)]
        public async Task<Response> DeleteScript(int id)
        {
            Response response;
            try
            {
                WatchlistBusiness watchlistBusiness = new WatchlistBusiness(Startup.Configuration);
                await watchlistBusiness.DeleteScript(id);
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
