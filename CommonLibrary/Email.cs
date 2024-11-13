using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class Email
    {
        public IConfiguration Configuration { get; private set; }

        private readonly MailMessage mailMessage = null;
        private readonly SmtpClient smtpClient = null;
        private readonly NetworkCredential networkCredential = null;

        #region Constructor
        public Email(IConfiguration configuration)
        {
            Configuration = configuration;
            mailMessage = new MailMessage();
            smtpClient = new SmtpClient();
            networkCredential = new NetworkCredential();
        }
        #endregion

        #region Properties
        public string SmtpHost
        {
            get
            {
                if (MyConvert.ToString(smtpClient.Host) != string.Empty)
                    return smtpClient.Host;
                else if (MyConvert.ToString(Configuration["AppSettings:Email:SmtpHost"]) != string.Empty)
                {
                    smtpClient.Host = MyConvert.ToString(Configuration["AppSettings:SmtpHost"]);
                    return smtpClient.Host;
                }
                else
                    throw new Exception("SMTP host is not set.");
            }
            set
            {
                smtpClient.Host = value;
            }
        }

        public int SmtpPort
        {
            get
            {
                if (smtpClient.Port != 25)
                    return smtpClient.Port;
                else if (MyConvert.ToString(Configuration["AppSettings:Email:SmtpPort"]) != string.Empty)
                {
                    smtpClient.Port = MyConvert.ToInt(Configuration["AppSettings:Email:SmtpPort"]);
                    return smtpClient.Port;
                }
                else
                    return smtpClient.Port;
            }
            set
            {
                smtpClient.Port = value;
            }
        }

        public bool EnableSsl
        {
            get
            {
                return MyConvert.ToBoolean(Configuration["AppSettings:Email:EnableSsl"]);
            }
        }

        public string SmtpUsername
        {
            get
            {
                if (MyConvert.ToString(networkCredential.UserName) != string.Empty)
                    return networkCredential.UserName;
                else if (MyConvert.ToString(Configuration["AppSettings:Email:SmtpUsername"]) != string.Empty)
                {
                    networkCredential.UserName = MyConvert.ToString(Configuration["AppSettings:Email:SmtpUsername"]);
                    return networkCredential.UserName;
                }
                else
                    throw new Exception("SMTP user name is not set.");
            }
            set
            {
                networkCredential.UserName = value;
            }
        }

        public string SmtpPassword
        {
            get
            {
                if (MyConvert.ToString(networkCredential.Password) != string.Empty)
                    return networkCredential.Password;
                else if (MyConvert.ToString(Configuration["AppSettings:Email:SmtpPassword"]) != string.Empty)
                {
                    networkCredential.Password = MyConvert.ToString(Configuration["AppSettings:Email:SmtpPassword"]);
                    return networkCredential.Password;
                }
                else
                    throw new Exception("SMTP password is not set.");
            }
            set
            {
                networkCredential.Password = value;
            }
        }

        public MailAddress From
        {
            get
            {
                if (mailMessage.From != null)
                    return mailMessage.From;
                else if (MyConvert.ToString(Configuration["AppSettings:Email:From"]) != string.Empty)
                {
                    mailMessage.From = new MailAddress(MyConvert.ToString(Configuration["AppSettings:Email:From"]));
                    return mailMessage.From;
                }
                else
                    throw new Exception("From email is not set.");
            }
            set
            {
                mailMessage.From = value;
            }
        }

        public MailAddressCollection To
        {
            get
            {
                return mailMessage.To;
            }
        }

        public MailAddressCollection CC
        {
            get
            {
                return mailMessage.CC;
            }
        }

        public MailAddressCollection BCC
        {
            get
            {
                return mailMessage.Bcc;
            }
        }

        public MailPriority Priority
        {
            get
            {
                return mailMessage.Priority;
            }
            set
            {
                mailMessage.Priority = value;
            }
        }

        public string Subject
        {
            get
            {
                return mailMessage.Subject;
            }
            set
            {
                mailMessage.Subject = value;
            }
        }

        public string Body
        {
            get
            {
                return mailMessage.Body;
            }
            set
            {
                mailMessage.Body = value;
            }
        }

        #endregion

        /// <summary>
        /// Function to read file email template 
        /// </summary>
        /// <param name="FilePath">FilePath to read it</param>
        /// <returns></returns>
        public string ReadTemplate(string FilePath)
        {
            string result = "";
            if (String.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
                return result;
            result = File.ReadAllText(FilePath);
            return result;
        }

        public async Task<string> ReadTemplateAsync(string FilePath)
        {
            string result = "";
            if (String.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
                return result;
            result = await File.ReadAllTextAsync(FilePath);
            return result;
        }

        public void Send()
        {
            if (To.Count == 0)
                throw new Exception("Email to is not added.");
            if (From == null)
                throw new Exception("Email from is not added.");
            if (SmtpHost == string.Empty)
                throw new Exception("SMTP host is not set.");
            if (SmtpPort == 0)
                throw new Exception("SMTP port is not set..");
            if (SmtpUsername == string.Empty)
                throw new Exception("SMTP user name is not set..");
            if (SmtpPassword == string.Empty)
                throw new Exception("SMTP password is not set..");

            smtpClient.Credentials = networkCredential;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            smtpClient.EnableSsl = EnableSsl;
            smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtpClient.Timeout = 50000;
            mailMessage.IsBodyHtml = true;
            smtpClient.Send(mailMessage);
        }

        public async Task SendAsync()
        {
            if (To.Count == 0)
                throw new Exception("Email to is not added.");
            if (From == null)
                throw new Exception("Email from is not added.");
            if (SmtpHost == string.Empty)
                throw new Exception("SMTP host is not set.");
            if (SmtpPort == 0)
                throw new Exception("SMTP port is not set..");
            if (SmtpUsername == string.Empty)
                throw new Exception("SMTP user name is not set..");
            if (SmtpPassword == string.Empty)
                throw new Exception("SMTP password is not set..");

            smtpClient.Credentials = networkCredential;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            smtpClient.EnableSsl = EnableSsl;
            smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtpClient.Timeout = 50000;
            mailMessage.IsBodyHtml = true;
            await smtpClient.SendMailAsync(mailMessage);
            //smtpClient.SendCompleted += (s, e) =>
            //{
            //    smtpClient.Dispose();
            //    mailMessage.Dispose();
            //};

        }
    }
}
