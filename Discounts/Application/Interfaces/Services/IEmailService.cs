namespace Application.Interfaces.Services;

public interface IEmailService
{
    Task SendRegistrationConfirmedAsync(string toEmail, CancellationToken ct = default);

    Task SendNewPasswordAsync(string toEmail, string password, CancellationToken ct = default);

    Task NotifyUserAsync(string toEmail, string subject, string message, CancellationToken ct = default);
}
