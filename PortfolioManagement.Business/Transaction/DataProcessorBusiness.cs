using CommonLibrary;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Transaction;
using ScrapySharp.Network;
using StockMarketBusiness.Transaction.Json;
using PortfolioManagement.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using PortfolioManagement.Business.Master;
using System.Runtime.Caching;

namespace PortfolioManagement.Business.Transaction
{
    /// <summary>
    /// This class having data scraping from nse and bse india site.
    /// Created By :: Rekansh Patel
    /// Created On :: 11/22/2020
    /// </summary>
    public class DataProcessorBusiness
    {
        IConfiguration _config;
        ScrapingBrowser _browser;

        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public DataProcessorBusiness(IConfiguration config)
        {
            _config = config;
            _browser = new ScrapingBrowser();
        }
        #endregion

        #region Public Method
        public async Task StartScrap(DateTime current, DateTime fromTime, DateTime toTime, DateTime fiiDiiTime, string nifty500_URL, string nifty500_ApiURL, string nseScript_URL, string nifty50_URL, string sensex_URL, string fiiDii_URL, string nseScript_ApiURL, string nifty50_ApiURL, string sensex_ApiURL, string fiiDii_ApiURL)
        {
            List<ScriptDaySummaryEntity> scriptDaySummaryEntities = await startIndexProcess(current, fromTime, toTime, nifty50_URL, sensex_URL, nifty50_ApiURL, sensex_ApiURL, false);
            Thread.Sleep(5000);
            scriptDaySummaryEntities = await startIndexProcess(current, fromTime, toTime, nifty500_URL, sensex_URL, nifty500_ApiURL, sensex_ApiURL, true);
            Thread.Sleep(5000);
            await startScriptProcess(current, fromTime, toTime, nseScript_URL, nseScript_ApiURL, scriptDaySummaryEntities);
            Thread.Sleep(5000);
            await startFiiDiiProcess(current, fiiDiiTime, fiiDii_URL, fiiDii_ApiURL);
        }
        #endregion

        #region Private Methods

        private async Task<List<ScriptDaySummaryEntity>> startIndexProcess(DateTime current, DateTime fromTime, DateTime toTime, string nifty50_URL, string sensex_URL, string nifty50_ApiURL, string sensex_ApiURL, bool isProcessScript)
        {
            ScrapNifty50DataEntity scrapNifty50DataEntity = new ScrapNifty50DataEntity();
            if (current >= fromTime && current <= toTime)
            {
                IndexEntity indexEntity = new IndexEntity();
                indexEntity.Date = current;

                if (!isProcessScript)
                    indexEntity = await scrapSensexData(indexEntity, sensex_URL, sensex_ApiURL);
                
                scrapNifty50DataEntity = await scrapNifty50Data(indexEntity, nifty50_URL, nifty50_ApiURL, isProcessScript);

                if(!isProcessScript)
                {
                    IndexBusiness indexBusiness = new IndexBusiness(_config);
                    await indexBusiness.Insert(scrapNifty50DataEntity.IndexEntity);
                }
            }
            return scrapNifty50DataEntity.ScriptDaySummaryEntities;
        }

        private async Task<IndexEntity> scrapSensexData(IndexEntity indexEntity, string sensex_URL, string sensex_ApiURL)
        {
            try
            {
                Log.Write("scrapSensexData process start");
                resetBrowser(false);
                WebPage webPage = await _browser.NavigateToPageAsync(new Uri(sensex_URL));
                string ajaxResponse = await _browser.AjaxDownloadStringAsync(new Uri(sensex_ApiURL));
                Sensex sensex = JsonSerializer.Deserialize<Sensex>(ajaxResponse);


                if (sensex.RealTime.Length > 0)
                {
                    indexEntity.SensexPreviousDay = sensex.RealTime[0].Prev_Close;
                    indexEntity.SensexOpen = sensex.RealTime[0].I_open;
                    indexEntity.SensexHigh = sensex.RealTime[0].High;
                    indexEntity.SensexLow = sensex.RealTime[0].Low;
                    indexEntity.SensexClose = sensex.RealTime[0].Curvalue;
                }
                Log.Write("scrapSensexData process end");
            }
            catch (Exception ex)
            {
                await ex.WriteLogFileAsync();
            }
            return indexEntity;
        }

        private async Task<ScrapNifty50DataEntity> scrapNifty50Data(IndexEntity indexEntity, string nifty50_URL, string nifty50_ApiURL, bool isProcessScript)
        {

            ScrapNifty50DataEntity scrapNifty50DataEntity = new ScrapNifty50DataEntity();

            try
            {
                if (isProcessScript)
                    Log.Write("scrapNifty500Data process start");
                else
                    Log.Write("scrapNifty50Data process start");

                resetBrowser(false);
                WebPage webPage = await _browser.NavigateToPageAsync(new Uri(nifty50_URL)); 

                string ajaxResponse = await _browser.AjaxDownloadStringAsync(new Uri(nifty50_ApiURL));
                Nifty50 nifty50 = JsonSerializer.Deserialize<Nifty50>(ajaxResponse);

                if (nifty50.data.Length > 0)
                {
                    indexEntity.NiftyPreviousDay = nifty50.data[0].previousClose;
                    indexEntity.NiftyOpen = nifty50.data[0].open;
                    indexEntity.NiftyHigh = nifty50.data[0].dayHigh;
                    indexEntity.NiftyLow = nifty50.data[0].dayLow;
                    indexEntity.NiftyClose = nifty50.data[0].lastPrice;
                }
                scrapNifty50DataEntity.IndexEntity = indexEntity;

                if (isProcessScript)
                    for (int i = 1; i < nifty50.data.Length; i++)
                    {
                        ScriptDaySummaryEntity scriptDaySummaryEntity = new ScriptDaySummaryEntity();
                        scriptDaySummaryEntity.ScriptName = MyConvert.ToString(nifty50.data[i].symbol);
                        scriptDaySummaryEntity.PreviousDay = nifty50.data[i].previousClose;
                        scriptDaySummaryEntity.Open = nifty50.data[i].open;
                        scriptDaySummaryEntity.High = nifty50.data[i].dayHigh;
                        scriptDaySummaryEntity.Low = nifty50.data[i].dayLow;
                        scriptDaySummaryEntity.Close = nifty50.data[i].lastPrice;
                        scriptDaySummaryEntity.Price = nifty50.data[i].lastPrice;
                        scriptDaySummaryEntity.Volume = nifty50.data[i].totalTradedVolume;
                        scriptDaySummaryEntity.Value = nifty50.data[i].totalTradedValue;
                        scriptDaySummaryEntity.High52Week = nifty50.data[i].yearHigh;
                        scriptDaySummaryEntity.Low52Week = nifty50.data[i].yearLow;
                        scrapNifty50DataEntity.ScriptDaySummaryEntities.Add(scriptDaySummaryEntity);
                    }
                if (isProcessScript)
                    Log.Write("scrapNifty500Data process end");
                else
                    Log.Write("scrapNifty50Data process end");
            }
            catch (Exception ex)
            {
                await ex.WriteLogFileAsync();
            }

            return scrapNifty50DataEntity;
        }
        private async Task startScriptProcess(DateTime current, DateTime fromTime, DateTime toTime, string nseScript_URL, string nseScript_ApiURL, List<ScriptDaySummaryEntity> scriptDaySummaryEntities)
        {
            if (current >= fromTime && current <= toTime)
            {
                ScriptBusiness scriptBusiness = new ScriptBusiness(_config);
                List<ScriptEntity> scriptEntities = await scriptBusiness.SelectForScrap();

                //Parallel.ForEach(scriptEntities, async (scriptEntity) =>
                //{
                //    await startSingleScriptProcess(current, nseScript_URL, nseScript_ApiURL, scriptEntity, scriptDaySummaryEntities);
                //});
                foreach (var scriptEntity in scriptEntities)
                {
                    await startSingleScriptProcess(current, nseScript_URL, nseScript_ApiURL, scriptEntity, scriptDaySummaryEntities);
                }
            }
        }

        private async Task startSingleScriptProcess(DateTime current, string nseScript_URL, string nseScript_ApiURL, ScriptEntity scriptEntity, List<ScriptDaySummaryEntity> scriptDaySummaryEntities)
        {
            try
            {
                ScriptDaySummaryEntity scriptDaySummaryEntity = await scrapScriptData(current, nseScript_URL, nseScript_ApiURL, scriptEntity, scriptDaySummaryEntities);
            
                ScriptDaySummaryBusiness scriptDaySummaryBusiness = new ScriptDaySummaryBusiness(_config);
                await scriptDaySummaryBusiness.Insert(scriptDaySummaryEntity);
            }
            catch (Exception ex) 
            {
                await ex.WriteLogFileAsync();
            }
        }

        private async Task<ScriptDaySummaryEntity> scrapScriptData(DateTime current, string nseScript_URL, string nseScript_ApiURL, ScriptEntity scriptEntity, List<ScriptDaySummaryEntity> scriptDaySummaryEntities)
        {
            ScriptDaySummaryEntity scriptDaySummaryEntity = scriptDaySummaryEntities.Where(x => x.ScriptName == scriptEntity.NseCode).FirstOrDefault();
            
            if (scriptDaySummaryEntity == null || scriptDaySummaryEntity.ScriptName.Length == 0)
            {
                Log.Write("Script Scrap process start: " + scriptEntity.NseCode);

                ScrapingBrowser browserScript = new ScrapingBrowser();
                browserScript.AllowAutoRedirect = true;
                browserScript.AllowMetaRedirect = true;
                browserScript.KeepAlive = true;
                browserScript.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                browserScript.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                browserScript.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36");
                browserScript.Headers.Add("Accept-Charset", "ISO-8859-1");
                browserScript.Headers.Add("X-Requested-With", "XMLHttpRequest");
                WebPage webPage = await browserScript.NavigateToPageAsync(new Uri(nseScript_URL + scriptEntity.NseCode));

                browserScript.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                browserScript.Headers.Add("Accept-Encoding", "");
                browserScript.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36");
                browserScript.Headers.Add("Accept-Charset", "ISO-8859-1");
                browserScript.Headers.Add("X-Requested-With", "XMLHttpRequest");
                string ajaxResponse = await browserScript.AjaxDownloadStringAsync(new Uri(nseScript_ApiURL + scriptEntity.NseCode));
                Quote quote = JsonSerializer.Deserialize<Quote>(ajaxResponse);

                browserScript.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                browserScript.Headers.Add("Accept-Encoding", "");
                browserScript.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36");
                browserScript.Headers.Add("Accept-Charset", "ISO-8859-1");
                browserScript.Headers.Add("X-Requested-With", "XMLHttpRequest");
                ajaxResponse = await browserScript.AjaxDownloadStringAsync(new Uri(nseScript_ApiURL + scriptEntity.NseCode + "&section=trade_info"));
                QuoteExt quoteExt = JsonSerializer.Deserialize<QuoteExt>(ajaxResponse);

                scriptDaySummaryEntity = new ScriptDaySummaryEntity();
                scriptDaySummaryEntity.Price = quote.priceInfo.lastPrice;
                scriptDaySummaryEntity.High = quote.priceInfo.intraDayHighLow.max;
                scriptDaySummaryEntity.Low = quote.priceInfo.intraDayHighLow.min;
                scriptDaySummaryEntity.PreviousDay = quote.priceInfo.previousClose;
                scriptDaySummaryEntity.Open = quote.priceInfo.open;
                scriptDaySummaryEntity.Close = quote.priceInfo.close;
                scriptDaySummaryEntity.Volume = MyConvert.ToLong(MyConvert.ToFloat(quoteExt.marketDeptOrderBook.tradeInfo.totalTradedVolume) * 100000);
                scriptDaySummaryEntity.Value = quoteExt.marketDeptOrderBook.tradeInfo.totalTradedValue * 10000000;
                scriptDaySummaryEntity.High52Week = quote.priceInfo.weekHighLow.max;
                scriptDaySummaryEntity.Low52Week = quote.priceInfo.weekHighLow.min;
            
                Log.Write("Script Scrap process end: " + scriptEntity.NseCode);
            }

            scriptDaySummaryEntity.ScriptId = scriptEntity.Id;
            scriptDaySummaryEntity.Date = current.Date;
            scriptDaySummaryEntity.DateTime = current;

            return scriptDaySummaryEntity;
        }

        private async Task startFiiDiiProcess(DateTime current, DateTime fiiDiiTime, string fiiDii_URL, string fiiDii_ApiURL)
        {
            if (current >= fiiDiiTime.AddMinutes(-2) && current <= fiiDiiTime.AddMinutes(2))
            {
                IndexBusiness indexBusiness = new IndexBusiness(_config);
                await indexBusiness.UpdateFiiDii(await scrapFiiDiiData(current, fiiDii_URL, fiiDii_ApiURL));
            }
        }

        private async Task<IndexEntity> scrapFiiDiiData(DateTime current, string fiiDii_URL, string fiiDii_ApiURL)
        {
            IndexEntity indexEntity = new IndexEntity();
            indexEntity.Date = current.Date;

            try
            {
                Log.Write("Fii Dii scrap process start");
                resetBrowser();
                WebPage webPage = await _browser.NavigateToPageAsync(new Uri(fiiDii_URL));

                resetBrowser(false);
                string ajaxResponse = await _browser.AjaxDownloadStringAsync(new Uri(fiiDii_ApiURL));
                //string ajaxResponse = System.IO.File.ReadAllText("D:\\fiiDii_ApiURL.json");


                FiiDii fiiDii = JsonSerializer.Deserialize<FiiDii>("{\"fiiDiiRecords\":" + ajaxResponse + "}");

                if (fiiDii.fiiDiiRecords.Length == 2)
                {
                    indexEntity.DII = MyConvert.ToDouble(fiiDii.fiiDiiRecords[0].netValue);
                    indexEntity.FII = MyConvert.ToDouble(fiiDii.fiiDiiRecords[1].netValue);
                }

                Log.Write("Fii Dii scrap process end");

            }
            catch(Exception ex)
            {
                await ex.WriteLogFileAsync();
            }

            return indexEntity;
        }

        private void resetBrowser(bool isCompress = true)
        {
            _browser.UserAgent = new FakeUserAgent("Chrome", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/129.0.0.0 Safari/537.36\r\n");
            _browser.AllowAutoRedirect = true;
            _browser.AllowMetaRedirect = true;
            _browser.KeepAlive = true;
            _browser.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            if (isCompress)
                _browser.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            else
                _browser.Headers.Add("Accept-Encoding", "");
            _browser.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36");
            _browser.Headers.Add("Accept-Charset", "ISO-8859-1");
            _browser.Headers.Add("X-Requested-With", "XMLHttpRequest");
        }

        #endregion

    }
}
