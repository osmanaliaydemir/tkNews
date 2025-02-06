namespace tkNews.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
    Task SendPasswordResetEmailAsync(string to, string resetLink);
    Task SendEmailConfirmationAsync(string to, string confirmationLink);
} 