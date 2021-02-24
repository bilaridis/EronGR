using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace EronNew.Helpers
{
    public class EmailService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await Task.Run(() => {
                EmailInfo emailObj = new EmailInfo()
                {
                    From = "admin@eron.gr",
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                };
                emailObj.To.Add(email);

                var emailH = new EmailHelper();
                emailH.SendEmailAsync(emailObj);
            });

        }
    }
}
