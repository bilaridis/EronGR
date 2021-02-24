using System.Collections.Generic;

namespace EronNew.Helpers
{
    public class EmailInfo
    {
        public EmailInfo()
        {
            To = new List<string>();
        }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public string From { get;  set; }
        public string Subject { get;  set; }
        public string Body { get;  set; }
        public bool IsBodyHtml { get;  set; }
        public IList<string> To { get;  set; }
    }
}