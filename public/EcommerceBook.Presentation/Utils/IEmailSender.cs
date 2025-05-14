﻿namespace EcommerceBook.Presentation.Utils
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlBody);
    }

}
