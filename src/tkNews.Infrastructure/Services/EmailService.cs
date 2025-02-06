using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using tkNews.Application.Common.Interfaces;

namespace tkNews.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly SmtpClient _smtpClient;
    private readonly string _fromEmail;
    
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        
        _fromEmail = _configuration["Email:From"] ?? throw new ArgumentNullException("Email:From configuration is missing");
        var host = _configuration["Email:SmtpServer"] ?? throw new ArgumentNullException("Email:SmtpServer configuration is missing");
        var portStr = _configuration["Email:Port"] ?? throw new ArgumentNullException("Email:Port configuration is missing");
        var username = _configuration["Email:Username"] ?? throw new ArgumentNullException("Email:Username configuration is missing");
        var password = _configuration["Email:Password"] ?? throw new ArgumentNullException("Email:Password configuration is missing");
        
        if (!int.TryParse(portStr, out var port))
        {
            throw new ArgumentException("Invalid port number in configuration");
        }
        
        _smtpClient = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };
    }
    
    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = isHtml
        };
        
        mailMessage.To.Add(to);
        
        await _smtpClient.SendMailAsync(mailMessage);
    }
    
    public async Task SendPasswordResetEmailAsync(string to, string resetLink)
    {
        var subject = "Şifre Sıfırlama";
        var body = $@"
            <h2>Şifre Sıfırlama İsteği</h2>
            <p>Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayın:</p>
            <p><a href='{resetLink}'>Şifremi Sıfırla</a></p>
            <p>Bu bağlantı 30 dakika süreyle geçerlidir.</p>
            <p>Eğer bu isteği siz yapmadıysanız, bu e-postayı görmezden gelebilirsiniz.</p>";
            
        await SendEmailAsync(to, subject, body, true);
    }
    
    public async Task SendEmailConfirmationAsync(string to, string confirmationLink)
    {
        var subject = "E-posta Doğrulama";
        var body = $@"
            <h2>E-posta Adresinizi Doğrulayın</h2>
            <p>E-posta adresinizi doğrulamak için aşağıdaki bağlantıya tıklayın:</p>
            <p><a href='{confirmationLink}'>E-posta Adresimi Doğrula</a></p>
            <p>Bu bağlantı 24 saat süreyle geçerlidir.</p>";
            
        await SendEmailAsync(to, subject, body, true);
    }
} 