using PortfolioManagement.DataProcessor.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioManagement.DataProcessor
{
    public class AppSettings
    {
        #region "AppSettings"
        public static string ApplicationName
        {
            get
            {
                return MyConvert.ToString(Program.Configuration["AppSettings:ApplicationName"]);
            }
        }

        public static string IstTimeZoneName
        {
            get
            {
                return MyConvert.ToString(Program.Configuration["AppSettings:IstTimeZoneName"]);
            }
        }

        #region Scrap
        public static List<int> Minutes
        {
            get
            {
                string[] vals = MyConvert.ToString(Program.Configuration["AppSettings:Scrap:Minutes"]).Split(",");
                List<int> result = new List<int>();
                foreach (string val in vals)
                    result.Add(MyConvert.ToInt(val));
                return result;
            }
        }
        
        public static int RandomMin
        {
            get
            {
                return MyConvert.ToInt(Program.Configuration["AppSettings:Scrap:RandomMin"]);
            }
        }
        
        public static int RandomMax
        {
            get
            {
                return MyConvert.ToInt(Program.Configuration["AppSettings:Scrap:RandomMax"]);
            }
        }
        #endregion Scrap

        #region "Path"
        public static string PathApi
        {
            get
            {
                return MyConvert.ToString(Program.Configuration["AppSettings:Path:Api"]);
            }
        }

        #endregion

        #region "Log"
        public static bool LogEnableWriteInfo
        {
            get
            {
                return MyConvert.ToBoolean(Program.Configuration["AppSettings:Log:EnableWriteInfo"]);
            }
        }

        public static bool LogEnableWriteError
        {
            get
            {
                return MyConvert.ToBoolean(Program.Configuration["AppSettings:Log:EnableWriteError"]);
            }
        }
        #endregion

        #region "Email"

        public static string EmailHost
        {
            get
            {
                return MyConvert.ToString(Program.Configuration["AppSettings:Email:Host"]);
            }
        }

        public static int EmailPort
        {
            get
            {
                return MyConvert.ToInt(Program.Configuration["AppSettings:Email:Port"]);
            }
        }

        public static string EmailUsername
        {
            get
            {
                return MyConvert.ToString(Program.Configuration["AppSettings:Email:Username"]);
            }
        }

        public static string EmailPassword
        {
            get
            {
                return MyConvert.ToString(Program.Configuration["AppSettings:Email:Password"]);
            }
        }

        public static bool EmailEnableSSL
        {
            get
            {
                return MyConvert.ToBoolean(Program.Configuration["AppSettings:Email:EnableSSL"]);
            }
        }

        public static string EmailReceiver
        {
            get
            {
                return MyConvert.ToString(Program.Configuration["AppSettings:Email:Receiver"]);
            }
        }

        public static string EmailSender
        {
            get
            {
                return MyConvert.ToString(Program.Configuration["AppSettings:Email:Sender"]);
            }
        }

        public static string EmailSenderName
        {
            get
            {
                return MyConvert.ToString(Program.Configuration["AppSettings:Email:SenderName"]);
            }
        }

        public static string EmailCC
        {
            get
            {
                return MyConvert.ToString(Program.Configuration["AppSettings:Email:CC"]);
            }
        }

        public static string EmailBCC
        {
            get
            {
                return MyConvert.ToString(Program.Configuration["AppSettings:Email:BCC"]);
            }
        }

        public static bool EmailIsAttachment
        {
            get
            {
                return MyConvert.ToBoolean(Program.Configuration["AppSettings:Email:IsAttachment"]);
            }
        }

        public static string EmailAttachmentFilePath
        {
            get
            {
                return MyConvert.ToString(Program.Configuration["AppSettings:Email:AttachmentFilePath"]);
            }
        }

        public static string EmailProcessFailureSubject
        {
            get
            {
                return MyConvert.ToString(Program.Configuration["AppSettings:Email:ProcessFailureSubject"]);
            }
        }

        public static string EmailProcessFailureBody
        {
            get
            {
                return MyConvert.ToString(Program.Configuration["AppSettings:Email:ProcessFailureBody"]);
            }
        }

        public static bool EmailEnableEventFailureNotification
        {
            get
            {
                return MyConvert.ToBoolean(Program.Configuration["AppSettings:Email:EnableEventFailureNotification"]);
            }
        }
        #endregion

        #endregion
    }
}
