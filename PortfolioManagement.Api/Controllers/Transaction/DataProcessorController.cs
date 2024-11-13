using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Transaction;
using System;
using System.Threading.Tasks;

namespace PortfolioManagement.Api.Controllers.Transaction
{
    /// <summary>
    /// This API use for transaction related operation like list, insert, update, delete transaction from database etc.
    /// </summary>
    [Route("transaction/dataprocessor")]
    [ApiController]
    public class DataProcessorController : ControllerBase
    {
        #region Interface public methods
        /// <summary>
        /// Get all columns information for particular transaction record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("start", Name = "transaction.dataprocessor.start")]
        [AuthorizeAPI(pageName: "Transaction", pageAccess: PageAccessValues.IgnoreAuthentication)]
        public async Task<Response> Start()
        {
            Response response;
            try
            {
                DateTime current = getCurrentTimeAfterAdjustment();
                string[] fromTimePart = AppSettings.ApplicationScrapFromTime.Split(':');
                string[] toTimePart = AppSettings.ApplicationScrapToTime.Split(':');
                string[] fiiDiiTimePart = AppSettings.ApplicationScrapFiiDiiTime.Split(':');
                string[] portfolioDateTimePart = AppSettings.ApplicationScrapPortfolioDateTimePart.Split(':');
                DateTime fromTime = new DateTime(current.Year, current.Month, current.Day, MyConvert.ToInt(fromTimePart[0]), MyConvert.ToInt(fromTimePart[1]), MyConvert.ToInt(fromTimePart[2]));
                DateTime toTime = new DateTime(current.Year, current.Month, current.Day, MyConvert.ToInt(toTimePart[0]), MyConvert.ToInt(toTimePart[1]), MyConvert.ToInt(toTimePart[2]));
                DateTime fiiDiiTime = new DateTime(current.Year, current.Month, current.Day, MyConvert.ToInt(fiiDiiTimePart[0]), MyConvert.ToInt(fiiDiiTimePart[1]), MyConvert.ToInt(fiiDiiTimePart[2]));
                DateTime portfolioDateTime = new DateTime(current.Year, current.Month, current.Day, MyConvert.ToInt(portfolioDateTimePart[0]), MyConvert.ToInt(portfolioDateTimePart[1]), MyConvert.ToInt(portfolioDateTimePart[2]));
                
                DataProcessorBusiness dataProcessorBusiness = new DataProcessorBusiness(Startup.Configuration);
                await dataProcessorBusiness.StartScrap(current, fromTime, toTime, fiiDiiTime, AppSettings.ApplicationScrapNifty500_URL, AppSettings.ApplicationScrapNifty500_ApiURL, AppSettings.ApplicationScrapNseScript_URL, AppSettings.ApplicationScrapNifty50_URL, AppSettings.ApplicationScrapSensex_URL, AppSettings.ApplicationScrapFiiDii_URL, AppSettings.ApplicationScrapNseScript_ApiURL, AppSettings.ApplicationScrapNifty50_ApiURL, AppSettings.ApplicationScrapSensex_ApiURL, AppSettings.ApplicationScrapFiiDii_ApiURL);

                PortfolioDatewiseBusiness portfolioDatewiseBusiness = new PortfolioDatewiseBusiness(Startup.Configuration);
                await portfolioDatewiseBusiness.ProcessPortfolioDatewise(current, portfolioDateTime);

                response = new Response("success");
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        #endregion

        private DateTime getCurrentTimeAfterAdjustment()
        {
            DateTime current = DateTime.UtcNow;
            int randomMin = AppSettings.RandomMin;
            int randomMax = AppSettings.RandomMax;
            foreach (var minute in AppSettings.Minutes)
            {
                if (current.Minute >= minute + randomMin && current.Minute <= minute + randomMax)
                {
                    if (minute + randomMax == 60)
                        current = new DateTime(current.Year, current.Month, current.Day, current.Hour + 1, 0, 0);
                    else
                        current = new DateTime(current.Year, current.Month, current.Day, current.Hour, minute + randomMax, 0);
                    break;
                }
            }
            return current;
        }
    }

}
