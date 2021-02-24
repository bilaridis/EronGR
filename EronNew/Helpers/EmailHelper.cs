using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace EronNew.Helpers
{

    public class EmailHelper
    {
        private static string SmtpServer { get; set; } = "linux50.papaki.gr";
        private static int SmtpPort { get; set; } = 25;
        private static string SmtpUser { get; set; } = "admin@eron.gr"; //Organization email
        private static string SmtpPassword { get; set; } = "2Wp_Ee!47@Sbg"; //password
        private MailMessage message;

        public EmailResult Result { get; set; }
        bool? mailSent { get; set; }
        string mailSentMessage = "";
        public void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                mailSent = false;
                mailSentMessage = "[" + token + "] Send canceled.";
                message.Dispose();
            }
            if (e.Error != null)
            {
                mailSent = false;
                mailSentMessage = token + " - Error - " + e.Error.ToString();
                message.Dispose();
            }
            else
            {
                mailSent = true;
                mailSentMessage = "Message sent.";
                message.Dispose();
            }

        }

        /// <summary>
        /// SendEmail
        /// </summary>
        /// <param name="emailInfo"></param>
        public void SendEmailAsync(EmailInfo emailInfo)
        {
            try
            {
                emailInfo.SmtpServer = SmtpServer;
                emailInfo.SmtpPort = SmtpPort;
                emailInfo.SmtpUser = SmtpUser;
                emailInfo.SmtpPassword = SmtpPassword;
                SmtpClient client = new SmtpClient(emailInfo.SmtpServer, emailInfo.SmtpPort);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(emailInfo.SmtpUser, emailInfo.SmtpPassword);
                client.EnableSsl = false;
                MailAddress from = new MailAddress(emailInfo.From);
                MailAddress to = new MailAddress(emailInfo.To.First());
                message = new MailMessage(from, to);
                message.Body = emailInfo.Body;
                message.IsBodyHtml = emailInfo.IsBodyHtml;
                message.Body += Environment.NewLine;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = emailInfo.Subject;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                string userState = "Message is compossed successfully.";
                client.SendAsync(message, userState);

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
