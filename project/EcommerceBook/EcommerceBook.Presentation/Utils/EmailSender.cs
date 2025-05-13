using System.Net.Mail;
using System.Net;

namespace EcommerceBook.Presentation.Utils
{

    public class GmailEmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("smodektm@gmail.com", "phbkbycralufphep"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("smodektm@gmail.com", "SmodeKTM Bookstore"),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }



}
