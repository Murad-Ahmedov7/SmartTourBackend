using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using SmartTour.Business.Services.Auth.Abstract;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var emailSection = _configuration.GetSection("Email");

        var smtpClient = new SmtpClient
        {
            Host = emailSection["Host"]!,
            Port = int.Parse(emailSection["Port"]!),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                emailSection["Username"],
                emailSection["Password"]
            )
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailSection["From"]!),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(to);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
