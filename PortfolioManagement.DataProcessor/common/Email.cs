using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;

namespace PortfolioManagement.DataProcessor.Common
{
    public class Email
    {
        public static void SendMailProcessFailure(Exception exParent)
        {
            if (AppSettings.EmailEnableEventFailureNotification)
            {
                Log.WriteLogFile("Error mail sending started " + MyConvert.GetCurrentIstDateTime().ToString("HH:mm:ss fff"));
                try
                {
                    string EmailBody = CreateProcessFailureEmailBody(exParent);
                    string EmailSubject = CreateProcessFailureEmailSubject();
                    string sentMailIds = SendMail(AppSettings.EmailReceiver, AppSettings.EmailSender, AppSettings.EmailSenderName,
                             EmailSubject, EmailBody, AppSettings.EmailCC, AppSettings.EmailBCC, AppSettings.EmailIsAttachment,
                             AppSettings.EmailAttachmentFilePath, MailPriority.High);
                    Log.Write("Error mail sent to --> " + sentMailIds + " " + MyConvert.GetCurrentIstDateTime().ToString("HH:mm:ss fff"));
                }
                catch (Exception ex)
                {
                    Log.WriteLogFile("Error occured while sending error mail " + MyConvert.GetCurrentIstDateTime().ToString("HH:mm:ss fff"));
                    ex.WriteLogFile();
                }
            }
            else
            {
                Log.WriteLogFile("Mail Sending not enabled" + MyConvert.GetCurrentIstDateTime().ToString("HH:mm:ss fff"));
            }
        }

        public static string CreateProcessFailureEmailSubject()
        {
            string EmailSubject = AppSettings.EmailProcessFailureSubject;
            StringBuilder stringBuilder = new StringBuilder();
            if (!String.IsNullOrEmpty(EmailSubject))
            {
                stringBuilder.Append(EmailSubject);
                stringBuilder.Replace("[SERVERNAME]", GetMachineName());
                stringBuilder.Replace("[IPADDRESS]", GetLocalIPAddress());
            }
            return stringBuilder.ToString();
        }

        public static string CreateProcessFailureEmailBody(Exception exParent)
        {
            string EmailBody = ReadTemplate(AppSettings.EmailProcessFailureBody);
            StringBuilder stringBuilder = new StringBuilder();
            if (!String.IsNullOrEmpty(EmailBody))
            {
                stringBuilder.Append(EmailBody);
                stringBuilder.Replace("[SERVERNAME]", GetMachineName());
                stringBuilder.Replace("[IPADDRESS]", GetLocalIPAddress());
                stringBuilder.Replace("[Timestamp]", DateTime.UtcNow.ToString("MMM dd yyyy HH:mm:ss fff"));
                stringBuilder.Replace("[ErrorMessage]", exParent.Message);
                stringBuilder.Replace("[ErrorStack]", MyConvert.ToString(exParent.StackTrace));
            }
            return stringBuilder.ToString();
        }

        private static string GetMachineName()
        {
            Log.Write("Getting Machine Name: " + Dns.GetHostName().ToString() + MyConvert.GetCurrentIstDateTime().ToString("HH:mm:ss fff"));
            return Dns.GetHostName().ToString();
        }

        private static string GetLocalIP()
        {
            Log.Write("Getting IP Address" + MyConvert.GetCurrentIstDateTime().ToString("HH:mm:ss fff"));
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            // Get the IP  
            string IPAddress = Dns.GetHostEntry(hostName).AddressList[1].ToString();
            Log.Write("IP Address: " + IPAddress + " " + MyConvert.GetCurrentIstDateTime().ToString("HH:mm:ss fff"));
            return IPAddress;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        /// <summary>
        /// Function to read file email templete 
        /// </summary>
        /// <param name="FilePath">FilePath to read it</param>
        /// <returns></returns>
        public static string ReadTemplate(string FilePath)
        {
            string result = "";
            if (String.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
                return result;
            result = File.ReadAllText(FilePath);
            return result;
        }

        /// <summary>
        /// Function to send an Email through smtp 
        /// </summary>
        /// <param name="EmailReceiverId">Email Receiver ids comma separated</param>
        /// <param name="EmailSenderId">Email Sender ids comma separated</param>
        /// <param name="EmailSenderNameToDisplay">Email sender name to display</param>
        /// <param name="EmailSubject">Email subject</param>
        /// <param name="EmailBody">Email body</param>
        /// <param name="CCEmailIds">Email ids to put in cc</param>
        /// <param name="BCCEmailIds">Email ids to put in bcc</param>
        /// <param name="IsAttachementAdd">IF an attachment want to Add in Email or not</param>
        /// <param name="AttachmentFilepath">FilePath of an Attachment</param>
        /// <returns></returns>
        public static string SendMail(string EmailReceiverId, string EmailSenderId, string EmailSenderNameToDisplay, string EmailSubject, string EmailBody, string CCEmailIds = "", string BCCEmailIds = "", bool IsAttachementAdd = false, string AttachmentFilepath = "", MailPriority Priority = MailPriority.Normal)
        {
            MailMessage mm = new MailMessage();
            NetworkCredential nc = new NetworkCredential(AppSettings.EmailUsername, AppSettings.EmailPassword);
            SmtpClient client = new SmtpClient(AppSettings.EmailHost, AppSettings.EmailPort);

            mm.BodyEncoding = Encoding.UTF8;
            mm.SubjectEncoding = Encoding.UTF8;
            mm.From = new MailAddress(EmailSenderId, EmailSenderNameToDisplay);
            if (EmailReceiverId != string.Empty)
                mm.To.Add(EmailReceiverId);
            mm.Subject = EmailSubject;
            mm.Body = EmailBody;
            mm.IsBodyHtml = true;
            mm.Priority = Priority;

            // create an object of SmtpClient class
            if (!CCEmailIds.Equals(string.Empty))
                mm.CC.Add(CCEmailIds);

            if (!BCCEmailIds.Equals(string.Empty))
                mm.Bcc.Add(BCCEmailIds);

            //if attachment
            if (IsAttachementAdd)
            {
                Attachment attachment = new Attachment(AttachmentFilepath);
                mm.Attachments.Add(attachment);
            }

            client.Credentials = nc;
            client.EnableSsl = AppSettings.EmailEnableSSL;
            client.Timeout = 20000;
            client.Send(mm);
            return EmailReceiverId;
        }

    }
}
