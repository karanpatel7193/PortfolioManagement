using CommonLibrary.SqlDB;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CommonLibrary
{
    public static class Log
    {
        private static readonly string LOG_CONFIG_FILE = @"log4net.config";
        private readonly static string LogSeparator = "";
        public static IConfiguration Configuration;
        private static log4net.ILog _logger = null;

        #region Properties
        private static bool EnableWriteInfo
        {
            get
            {
                return MyConvert.ToBoolean(Configuration.GetValue<string>("AppSettings:Log:EnableWriteInfo"));
            }
        }

        private static bool EnableWriteError
        {
            get
            {
                return MyConvert.ToBoolean(Configuration.GetValue<string>("AppSettings:Log:EnableWriteError"));
            }
        }

        private static bool EnableSendEmailError
        {
            get
            {
                return MyConvert.ToBoolean(Configuration.GetValue<string>("AppSettings:Log:EnableSendEmailError"));
            }
        }
        private static string ErrorEmailSubject
        {
            get
            {
                return MyConvert.ToString(Configuration.GetValue<string>("AppSettings:Log:ErrorEmailSubject"));
            }
        }
        private static string ErrorEmailBody
        {
            get
            {
                return MyConvert.ToString(Configuration.GetValue<string>("AppSettings:Log:ErrorEmailBody"));
            }
        }
        private static string ErrorEmailTo
        {
            get
            {
                return MyConvert.ToString(Configuration.GetValue<string>("AppSettings:Log:ErrorEmailTo"));
            }
        }

        private static bool EnableInsertError
        {
            get
            {
                return MyConvert.ToBoolean(Configuration.GetValue<string>("AppSettings:Log:EnableInsertError"));
            }
        }

        private static string ErrorConnectionStringKey
        {
            get
            {
                return MyConvert.ToString(Configuration.GetValue<string>("AppSettings:Log:ErrorConnectionStringKey"));
            }
        }

        private static string ApplicationName
        {
            get
            {
                return MyConvert.ToString(Configuration.GetValue<string>("AppSettings:Application:Name"));
            }
        }

        private static ILog LoggerInstance
        {
            get
            {
                if (_logger == null)
                {
                    _logger = Logger;
                }
                return _logger;
            }
        }
        #endregion


        private static log4net.ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = GetLogger(typeof(Log));
                    SetLog4NetConfiguration();
                }
                return _logger;
            }
        }

        public static ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }

        private static void SetIConfiguration()
        {
            if (Configuration == null)
            {
                IConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
                Configuration = builder.Build();
            }
        }

        private static void SetLog4NetConfiguration()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(File.OpenRead(LOG_CONFIG_FILE));

            var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, xmlDocument["log4net"]);
        }

        #region Write Error Logs
        /// <summary>
        /// Write an error in log file i.e. specified in config file.
        /// </summary>
        /// <param name="message">The Message that needs to log</param>
        public static void Write(string message)
        {
            try
            {
                SetIConfiguration();
                if (EnableWriteInfo)
                    Logger.Info(message + LogSeparator);

                Console.WriteLine(message + LogSeparator);
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// Write an error in log file i.e. specified in config file.
        /// </summary>
        /// <param name="message">The Message that needs to log</param>
        public static void WriteLogFile(this string message)
        {
            Write(message);
        }

        /// <summary>
        /// Write an error in log file i.e. specified in config file.
        /// </summary>
        /// <param name="exception">Instance of an exception</param>
        public static string WriteLogFile(this Exception exception, long userId = 0, bool sentryEnable = true)
        {
            string ErrorId = string.Empty;
            try
            {
                SetIConfiguration();

                if (ErrorId == string.Empty)
                    ErrorId = Guid.NewGuid().ToString();

                if (EnableWriteError)
                    LoggerInstance.Error(userId == 0 ? ErrorId + Environment.NewLine + exception + LogSeparator : "UserId:" + userId + Environment.NewLine + ErrorId + Environment.NewLine + exception + LogSeparator);
                
                if (EnableInsertError)
                    InsertErrorLog(ErrorId, exception, userId);
                
                if (EnableSendEmailError)
                    SendMailProcessFailure(ErrorId, exception, MyConvert.ToString(userId));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return ErrorId;
        }

        private static void InsertErrorLog(string errorId, Exception exception, long userId)
        {
            ISql sql = SetSql(errorId, exception, userId);
            sql.ExecuteNonQuery("ErrorReports_Insert", CommandType.StoredProcedure);
        }

        /// <summary>
        /// Write an error in log file i.e. specified in config file.
        /// </summary>
        /// <param name="exception">Instance of an exception</param>
        public static async Task<string> WriteLogFileAsync(this Exception exception, long userId = 0, bool sentryEnable = true)
        {
            string ErrorId = string.Empty;
            try
            {
                SetIConfiguration();

                if (ErrorId == string.Empty)
                    ErrorId = Guid.NewGuid().ToString();

                if (EnableWriteError)
                    LoggerInstance.Error(userId == 0 ? ErrorId + Environment.NewLine + exception + LogSeparator : "UserId:" + userId + Environment.NewLine + ErrorId + Environment.NewLine + exception + LogSeparator);
                
                if (EnableInsertError)
                    await InsertErrorLogAsync(ErrorId, exception, userId);
                
                if (EnableSendEmailError)
                    await SendMailProcessFailureAsync(ErrorId, exception, MyConvert.ToString(userId));

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return ErrorId;
        }

        public static void SendMailProcessFailure(string ErrorId, Exception ex, string userId)
        {
            if (EnableSendEmailError)
            {
                Log.WriteLogFile("Error mail sending started " + DateTime.Now.ToString("HH:mm:ss fff"));
                try
                {
                    Email email = new Email(Configuration);
                    email.Subject = CreateProcessFailureEmailSubject();
                    email.Body = CreateProcessFailureEmailBody(ErrorId, ex, userId);
                    foreach (string toEmail in ErrorEmailTo.Split(','))
                        email.To.Add(toEmail);
                    email.Priority = MailPriority.High;
                    email.Send();

                    Log.Write("Error mail sent to --> " + ErrorEmailTo + " " + DateTime.Now.ToString("HH:mm:ss fff"));
                }
                catch
                {
                    Log.WriteLogFile("Error occured while sending error mail " + DateTime.Now.ToString("HH:mm:ss fff"));
                }
            }
            else
            {
                Log.WriteLogFile("Mail Sending not enabled" + DateTime.Now.ToString("HH:mm:ss fff"));
            }
        }
        public static async Task SendMailProcessFailureAsync(string ErrorId, Exception ex, string userId)
        {
            if (EnableSendEmailError)
            {
                Log.WriteLogFile("Error mail sending started " + DateTime.Now.ToString("HH:mm:ss fff"));
                try
                {
                    Email email = new Email(Configuration);
                    email.Subject = CreateProcessFailureEmailSubject();
                    email.Body = await CreateProcessFailureEmailBodyAsync(ErrorId, ex, userId);
                    foreach (string toEmail in ErrorEmailTo.Split(','))
                        email.To.Add(toEmail);
                    email.Priority = MailPriority.High;
                    await email.SendAsync();
                    
                    Log.Write("Error mail sent to --> " + ErrorEmailTo + " " + DateTime.Now.ToString("HH:mm:ss fff"));
                }
                catch
                {
                    Log.WriteLogFile("Error occured while sending error mail " + DateTime.Now.ToString("HH:mm:ss fff"));
                }
            }
            else
            {
                Log.WriteLogFile("Mail Sending not enabled" + DateTime.Now.ToString("HH:mm:ss fff"));
            }
        }
        private static string CreateProcessFailureEmailSubject()
        {
            string EmailSubject = ErrorEmailSubject;
            StringBuilder stringBuilder = new StringBuilder();
            if (!String.IsNullOrEmpty(EmailSubject))
            {
                stringBuilder.Append(EmailSubject);
                stringBuilder.Replace("[SERVERNAME]", GetMachineName());
                stringBuilder.Replace("[IPADDRESS]", GetLocalIPAddress());
            }
            return stringBuilder.ToString();
        }
        private static string CreateProcessFailureEmailBody(string ErrorId, Exception ex, string userId)
        {
            string EmailBody = ReadTemplate(ErrorEmailBody);
            StringBuilder stringBuilder = new StringBuilder();
            if (!String.IsNullOrEmpty(EmailBody))
            {
                stringBuilder.Append(EmailBody);
                if (ErrorId != string.Empty)
                    stringBuilder.Replace("[ErrorId]", ErrorId);
                if (userId != string.Empty)
                    stringBuilder.Replace("[UserId]", userId);
                stringBuilder.Replace("[SERVERNAME]", GetMachineName());
                stringBuilder.Replace("[IPADDRESS]", GetLocalIPAddress());
                stringBuilder.Replace("[CurrentDateTime]", DateTime.Now.ToString("MMM dd yyyy HH:mm:ss fff"));
                stringBuilder.Replace("[ErrorMessage]", ex.Message);
                stringBuilder.Replace("[ErrorStack]", MyConvert.ToString(ex.StackTrace != null ? ex.StackTrace : ""));
            }
            return stringBuilder.ToString();
        }
        private static async Task<string> CreateProcessFailureEmailBodyAsync(string ErrorId, Exception ex, string userId)
        {
            string EmailBody = await ReadTemplateAsync(ErrorEmailBody);
            StringBuilder stringBuilder = new StringBuilder();
            if (!String.IsNullOrEmpty(EmailBody))
            {
                stringBuilder.Append(EmailBody);
                if (ErrorId != string.Empty)
                    stringBuilder.Replace("[ErrorId]", ErrorId);
                if (userId != string.Empty)
                    stringBuilder.Replace("[UserId]", userId);
                stringBuilder.Replace("[SERVERNAME]", GetMachineName());
                stringBuilder.Replace("[IPADDRESS]", GetLocalIPAddress());
                stringBuilder.Replace("[CurrentDateTime]", DateTime.Now.ToString("MMM dd yyyy HH:mm:ss fff"));
                stringBuilder.Replace("[ErrorMessage]", ex.Message);
                stringBuilder.Replace("[ErrorStack]", MyConvert.ToString(ex.StackTrace != null ? ex.StackTrace : ""));
            }
            return stringBuilder.ToString();
        }
        private static string GetMachineName()
        {
            return Dns.GetHostName().ToString();
        }
        private static string GetLocalIP()
        {
            string hostName = Dns.GetHostName();
            string IPAddress = Dns.GetHostEntry(hostName).AddressList[1].ToString();
            return IPAddress;
        }
        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }
        private static string ReadTemplate(string FilePath)
        {
            string result = "";
            if (String.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
                return result;
            result = File.ReadAllText(FilePath);
            return result;
        }
        private static async Task<string> ReadTemplateAsync(string FilePath)
        {
            string result = "";
            if (String.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
                return result;
            result = await File.ReadAllTextAsync(FilePath);
            return result;
        }

        private static async Task InsertErrorLogAsync(string errorId, Exception exception, long userId)
        {
            ISql sql = SetSql(errorId, exception, userId);
            await sql.ExecuteNonQueryAsync("ErrorReports_Insert", CommandType.StoredProcedure);
        }

        private static ISql SetSql(string errorId, Exception exception, long userId)
        {
            CreateSql oCreateSQL = new CreateSql(Configuration);
            ISql sql = oCreateSQL.CreateSqlInstance(ErrorConnectionStringKey);

            sql.AddParameter("Id", 0);
            sql.AddParameter("ErrorId", errorId);
            sql.AddParameter("Name", string.Empty);
            sql.AddParameter("Description", string.Empty);
            sql.AddParameter("Email", string.Empty);
            sql.AddParameter("Error", "{Message : " + exception.Message + "Data : " + exception.StackTrace.ToString() + "}");
            sql.AddParameter("UserId", userId);
            sql.AddParameter("IsReportByUser", false);
            sql.AddParameter("CreatedDateTime", System.DateTime.UtcNow);
            sql.AddParameter("ReportedDateTime", System.DateTime.UtcNow);
            sql.AddParameter("ApplicationName", ApplicationName);

            return sql;
        }
        #endregion
    }

    public class ErrorEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public ErrorEntity()
        {
            SetDefaulValue();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Get & Set Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Get & Set ErrorId
        /// </summary>
        public string ErrorId { get; set; }

        /// <summary>
        /// Get & Set Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get & Set Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get & Set Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Get & Set Error
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Get & Set User Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Get & Set Is Report By User
        /// </summary>
        public bool IsReportByUser { get; set; }

        /// <summary>
        /// Get & Set Created Date Time
        /// </summary>
        public System.DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Get & Set Reported Date Time
        /// </summary>
        public System.DateTime ReportDateTime { get; set; }

        /// <summary>
        /// Get & Set ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
        #endregion

        #region Private Methods
        /// <summary>
        /// This function is set properties default value based on its data type in table.
        /// </summary>
        private void SetDefaulValue()
        {
            Id = 0;
            ErrorId = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            Email = string.Empty;
            Error = string.Empty;
            UserId = 0;
            IsReportByUser = false;
            CreateDateTime = DateTime.MinValue;
            ReportDateTime = DateTime.MinValue;
            ApplicationName = string.Empty;
        }
        #endregion
    }

}
