using CommonLibrary;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Master;
using PortfolioManagement.Entity.Master;
using PortfolioManagement.Repository.Master;

namespace PortfolioManagement.Api.Controllers.Master
{
    [Route("master/splitBonus")]
    [ApiController]
    public class SplitBonusController : ControllerBase
    {
        ISplitBonusRepository splitBonusRepository;
        public SplitBonusController(ISplitBonusRepository splitBonusRepository)
        {
            this.splitBonusRepository = splitBonusRepository;
        }

        #region Interface public methods
        /// <summary>
        /// Get all columns information for particular SplitBonus record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getRecord/{id:int}", Name = "master.SplitBonus.record")]
        [AuthorizeAPI(pageName: "Split or Bonus", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForRecord(int id)
        {
            Response response;
            try
            {
                response = new Response(await splitBonusRepository.SelectForRecord(id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

       
        /// <summary>
        /// Get SplitBonus list for bind grid.
        /// </summary>
        /// <param name="SplitBonusParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getGridData", Name = "master.SplitBonus.gridData")]
        [AuthorizeAPI(pageName: "Split or Bonus", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForGrid()
        {
            Response response;
            try
            {
                response = new Response(await splitBonusRepository.SelectForGrid());
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Insert record in SplitBonus table.
        /// </summary>
        /// <param name="SplitBonusEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("insert", Name = "master.SplitBonus.insert")]
        [AuthorizeAPI(pageName: "Split or Bonus", pageAccess: PageAccessValues.Insert)]
        public async Task<Response> Insert(SplitBonusEntity splitBonusEntity)
        {
            Response response;
            try
            {
                response = new Response(await splitBonusRepository.Insert(splitBonusEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Update record in SplitBonus table.
        /// </summary>
        /// <param name="SplitBonusEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update", Name = "master.SplitBonus.update")]
        [AuthorizeAPI(pageName: "Split or Bonus", pageAccess: PageAccessValues.Update)]
        public async Task<Response> Update(SplitBonusEntity splitBonusEntity)
        {
            Response response;
            try
            {
                response = new Response(await splitBonusRepository.Update(splitBonusEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Delete record from SplitBonus table.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete/{id:int}", Name = "master.SplitBonus.delete")]
        [AuthorizeAPI(pageName: "Split or Bonus", pageAccess: PageAccessValues.Delete)]
        public async Task<Response> Delete(int id)
        {
            Response response;
            try
            {
                await splitBonusRepository.Delete(id);
                response = new Response();
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpPost]
        [Route("apply", Name = "master.SplitBonus.apply")]
        [AuthorizeAPI(pageName: "Split or Bonus", pageAccess: PageAccessValues.Update)]
        public async Task<Response> Apply(int id, bool IsApply)
        {
            Response response;
            try
            {
                response = new Response(await splitBonusRepository.SplitBonusApply(id, IsApply));
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
