using System.Net;
using System.Net.Mail;
using Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config) => _config = config;

    public Task SendRegistrationConfirmedAsync(string toEmail, CancellationToken ct)
        => SendEmailAsync(toEmail, "Successful Registered", $"Congratulations! You successfully registered for Discounts Website!", ct);

    public Task SendNewPasswordAsync(string toEmail, string password, CancellationToken ct = default)
        => SendEmailAsync(toEmail, "Password Reset", $"Your new password is: {password}\nChange it after login.", ct);

    public async Task NotifyUserAsync(string toEmail, string subject, string message, CancellationToken ct = default)
        => await SendEmailAsync(toEmail, subject, message, ct).ConfigureAwait(false);

    private async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken ct = default)
    {
        using var smtp = new SmtpClient(_config["Email:SmtpServer"])
        {
            Port = int.Parse(_config["Email:Port"]!),
            EnableSsl = bool.Parse(_config["Email:EnableSsl"]!),
            Credentials = new NetworkCredential(
                _config["Email:Username"],
                _config["Email:Password"]
            )
        };

        using var mail = new MailMessage
        {
            From = new MailAddress(_config["Email:From"]!),
            Subject = subject,
            Body = body
        };

        mail.To.Add(toEmail);

        await smtp.SendMailAsync(mail, ct).ConfigureAwait(false);
    }
}
