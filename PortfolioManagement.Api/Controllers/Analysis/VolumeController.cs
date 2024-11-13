using CommonLibrary;
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
                VolumeParameterEntity volumeParameterEntity = new VolumeParameterEntity();
                volumeParameterEntity.DateTime = getCurrentTimeAfterAdjustment();
                VolumeBusiness volumeBusiness = new VolumeBusiness(Startup.Configuration);
                response = new Response(await volumeBusiness.SelectForVolume(volumeParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        private DateTime getCurrentTimeAfterAdjustment()
        {
            DateTime current = DateTime.UtcNow;
            int randomMin = AppSettings.RandomMin;
            int randomMax = AppSettings.RandomMax;
            foreach (var minute in AppSettings.MinuteRanges)
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
