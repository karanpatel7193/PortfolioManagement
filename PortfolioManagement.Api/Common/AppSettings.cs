
using CommonLibrary;

namespace PortfolioManagement.Api.Common
{
    public class AppSettings
    {
        public static string API_URL
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Application:URL:API_URL"]);
            }
        }
        public static string EmailFrom
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Email:From"]);
            }
        }
        public static string WebsiteUrl
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:WebsiteUrl"]);
            }
        }
        public static string ClientWebsiteUrl
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:ClientWebsiteUrl"]);
            }
        }
        public static int LinkExpireDuration
        {
            get
            {
                return MyConvert.ToInt(Startup.Configuration["AppSettings:LinkExpireDuration"]);
            }
        }
        public static int UserImageSize
        {
            get
            {
                return MyConvert.ToInt(Startup.Configuration["AppSettings:Application:UserImageSize"]);
            }
        }
        public static int UserThumbImageSize
        {
            get
            {
                return MyConvert.ToInt(Startup.Configuration["AppSettings:Application:UserThumbImageSize"]);
            }
        }
        public static string VersionNo
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:VersionNo"]);
            }
        }
        public static DateTime UpdateDateTime
        {
            get
            {
                return MyConvert.ToDateTime(Startup.Configuration["AppSettings:UpdateDateTime"]);
            }
        }
        public static Double AppLastVersionNo
        {
            get
            {
                return MyConvert.ToDouble(Startup.Configuration["AppSettings:AppLastVersionNo"]);
            }
        }
        public static bool WebUnderMaintenance
        {
            get
            {
                return MyConvert.ToBoolean(Startup.Configuration["AppSettings:Application:WebUnderMaintenance"]);
            }
        }

        #region Application
        public static string ApplicationSwagger
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Application:Swagger"]);
            }
        }

        #region URL
        #endregion
        #region Path
        public static string PathDocumentUpload
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Application:Path:DocumentUpload"]);
            }
        }
        #endregion
        #region Scrap
        public static List<int> Minutes
        {
            get
            {
                string[] vals = MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:Minutes"]).Split(",");
                List<int> result = new List<int>();
                foreach (string val in vals)
                    result.Add(MyConvert.ToInt(val));
                return result;
            }
        }
        public static int[,] MinuteRanges
        {
            get
            {
                string[] ranges = MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:MinuteRanges"]).Split(";");
                int[,] result = new int[ranges.Length,2];

                for (int i = 0; i < ranges.Length; i++)
                {
                    string[] rangeValues = ranges[i].Split(",");
                    for (int j = 0; j < rangeValues.Length; j++)
                    {
                        result[i, j] = MyConvert.ToInt(rangeValues[j]);
                    }
                }
                return result;
            }
        }
        public static int RandomMin
        {
            get
            {
                return MyConvert.ToInt(Startup.Configuration["AppSettings:Scrap:RandomMin"]);
            }
        }

        public static int RandomMax
        {
            get
            {
                return MyConvert.ToInt(Startup.Configuration["AppSettings:Scrap:RandomMax"]);
            }
        }

        public static string ApplicationScrapFromTime
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:FromTime"]);
            }
        }

        public static string ApplicationScrapToTime
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:ToTime"]);
            }
        }

        public static string ApplicationScrapFiiDiiTime
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:FiiDiiTime"]);
            }
        }

        public static string ApplicationScrapPortfolioDateTimePart
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:PortfolioDateTime"]);
            }
        }

        public static string ApplicationScrapNseScript_URL
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:NseScript_URL"]);
            }
        }

        public static string ApplicationScrapNifty50_URL
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:Nifty50_URL"]);
            }
        }

        public static string ApplicationScrapNifty500_URL
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:Nifty500_URL"]);
            }
        }

        public static string ApplicationScrapSensex_URL
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:Sensex_URL"]);
            }
        }

        public static string ApplicationScrapFiiDii_URL
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:FiiDii_URL"]);
            }
        }

        public static string ApplicationScrapNseScript_ApiURL
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:NseScript_ApiURL"]);
            }
        }

        public static string ApplicationScrapNifty50_ApiURL
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:Nifty50_ApiURL"]);
            }
        }

        public static string ApplicationScrapNifty500_ApiURL
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:Nifty500_ApiURL"]);
            }
        }

        public static string ApplicationScrapSensex_ApiURL
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:Sensex_ApiURL"]);
            }
        }

        public static string ApplicationScrapFiiDii_ApiURL
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:Scrap:FiiDii_ApiURL"]);
            }
        }
        #endregion
        #endregion

        #region Security Token
        public static bool SecurityTokenEnabled
        {
            get
            {
                return MyConvert.ToBoolean(Startup.Configuration["AppSettings:SecurityToken:Enabled"]);
            }
        }

        /// <summary>
        /// Get SecurityToken key from configuration.
        /// </summary>
        public static string SecurityTokenKey
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:SecurityToken:Key"]);
            }
        }

        /// <summary>
        /// Get SecurityToken Issuer from configuration.
        /// </summary>
        public static string SecurityTokenIssuer
        {
            get
            {
                return MyConvert.ToString(Startup.Configuration["AppSettings:SecurityToken:Issuer"]);
            }
        }
        #endregion
    }
}