using log4net;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

namespace PortfolioManagement.DataProcessor.Common
{
    public static class Log
    {
        private static readonly string LOG_CONFIG_FILE = @"log4net.config";
        private readonly static string LogSeparator = "";
        private static log4net.ILog _logger = null;

        #region Properties
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
                Console.WriteLine(message);
                if (AppSettings.LogEnableWriteInfo)
                    Logger.Info(message + LogSeparator);
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
        public static string WriteLogFile(this Exception exception)
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine("---------------------------------");
            Console.WriteLine(exception.StackTrace);
            string ErrorId = string.Empty;
            try
            {
                if (ErrorId == string.Empty)
                    ErrorId = Guid.NewGuid().ToString();

                if (AppSettings.LogEnableWriteError)
                    LoggerInstance.Error(ErrorId + Environment.NewLine + exception + LogSeparator);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return ErrorId;
        }
        #endregion
    }
}
