using CommonLibrary;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Analysis;
using PortfolioManagement.Business.Index;
using PortfolioManagement.Entity.Analysis;

namespace PortfolioManagement.Api.Controllers.Analysis
{
    [Route("analysis/volume")]
    [ApiController]
    public class VolumeController : ControllerBase
    {
        [HttpPost]
        [Route("getForVolume", Name = "analysis.getForVolume")]
        // [AuthorizeAPI(pageName: "Analysis", pageAccess: PageAccessValues.View)]
        public async Task<Response> GetForVolume()
        {
            Response response;
            try
            {
                VolumeBusiness volumeBusiness = new VolumeBusiness(Startup.Configuration);
                response = new Response(await volumeBusiness.SelectForVolume());
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        private DateTime getCurrentTimeAfterAdjustment()
        {
            DateTime current = DateTime.UtcNow.AddMinutes(-15);
            int randomMin = AppSettings.RandomMin;
            int randomMax = AppSettings.RandomMax;

            for (int i = 0; i < AppSettings.MinuteRanges.Length / 2; i++)
            {
                if (current.Minute >= AppSettings.MinuteRanges[i, 0] && current.Minute <= AppSettings.MinuteRanges[i, 1])
                {
                    if (AppSettings.MinuteRanges[i, 1] == 59)
                        current = new DateTime(current.Year, current.Month, current.Day, current.Hour + 1, 0, 0);
                    else
                        current = new DateTime(current.Year, current.Month, current.Day, current.Hour, AppSettings.MinuteRanges[i, 1] + 1, 0);
                    break;
                }
            }

            return current;
        }
    }
}
