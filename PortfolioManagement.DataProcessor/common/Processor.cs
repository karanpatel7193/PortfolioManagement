using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PortfolioManagement.DataProcessor.Common
{
    public class Processor
    {
        public async Task Start()
        {
            Log.Write("Start api call.");
            try
            {
                using (HttpClient httpclient = new HttpClient())
                {
                    httpclient.Timeout = TimeSpan.FromMinutes(15);
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, AppSettings.PathApi);
                    using (HttpResponseMessage response = httpclient.SendAsync(requestMessage).Result)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            Log.Write("Finish api call> Result :" + response.Content.ReadAsStringAsync().Result);
                        }
                        else
                        {
                            Log.Write("Finish api call> Status :" + response.StatusCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write("Error while api call..." + MyConvert.GetCurrentIstDateTime().ToString("HH:mm:ss fff"));
                Log.Write(ex.Message.ToString());
                Log.WriteLogFile(ex);
                Email.SendMailProcessFailure(ex);
            }

            Log.Write("End api call.");
        }
    }
}
