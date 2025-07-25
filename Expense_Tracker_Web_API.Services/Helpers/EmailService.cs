using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Expense_Tracker_Web_API.Services.Helpers;

public class EmailService(IConfiguration configuration)
{
    #region Configuration Settings
    private readonly IConfiguration _configuration = configuration;
    #endregion

    #region Send Email Async Method
    public async Task SendEmailAsync(string toEmail, string subject, string body, bool isBodyHtml = false)
    {

        string email_ = _configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");


        string password = _configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");


        string host = _configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");


        string senderName = _configuration.GetValue<string>("EMAIL_CONFIGURATION:SENDERNAME");


        int port = _configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

        SmtpClient smtpClient = new(host, port)
        {
            Credentials = new NetworkCredential(email_, password),

            EnableSsl = true,
            UseDefaultCredentials = true,
        };


        MailAddress fromAddress = new(email_!, senderName);

        MailMessage message = new()
        {
            From = fromAddress,
            Subject = subject,
            Body = body,
            IsBodyHtml = isBodyHtml
        };



        message.To.Add(toEmail);

        await smtpClient.SendMailAsync(message);
    }

    #endregion

}
