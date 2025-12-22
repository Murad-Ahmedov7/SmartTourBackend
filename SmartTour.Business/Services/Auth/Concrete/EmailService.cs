//using System.Net;
//using System.Net.Mail;
//using Microsoft.Extensions.Configuration;
//using SmartTour.Business.Services.Auth.Abstract;

//public class EmailService : IEmailService
//{
//    private readonly IConfiguration _configuration;

//    public EmailService(IConfiguration configuration)
//    {
//        _configuration = configuration;
//    }

//    public async Task SendAsync(string to, string subject, string body)
//    {
//        Console.WriteLine("EMAIL SERVICE CALLED");
//        var emailSection = _configuration.GetSection("Email");

//        var smtpClient = new SmtpClient
//        {
//            Host = emailSection["Host"]!,
//            Port = int.Parse(emailSection["Port"]!),
//            EnableSsl = true,
//            Credentials = new NetworkCredential(
//                emailSection["Username"],
//                emailSection["Password"]
//            )
//        };

//        var mailMessage = new MailMessage   
//        {
//            From = new MailAddress(emailSection["From"]!),
//            Subject = subject,
//            Body = body,
//            IsBodyHtml = true
//        };

//        mailMessage.To.Add(to);

//        await smtpClient.SendMailAsync(mailMessage);
//    }
//}


using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
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
        Console.WriteLine("EMAIL SERVICE CALLED");

        var smtp = _configuration.GetSection("SmtpSettings");

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(
            _configuration["Email:From"] ?? "mrdhmdv04@gmail.com"
        ));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = body
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            smtp["SmtpServer"],
            int.Parse(smtp["SmtpPort"]!),
            SecureSocketOptions.StartTls
        );

        await client.AuthenticateAsync(
            smtp["SmtpUsername"],   // apikey
            smtp["SmtpPassword"]    // SMTP KEY
        );

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
