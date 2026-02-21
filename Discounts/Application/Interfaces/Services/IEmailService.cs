namespace Application.Interfaces.Services;

public interface IEmailService
{
    Task NotifyUserAsync(string toEmail, string subject, string message, CancellationToken ct = default);

    Task SendNewPasswordAsync(string toEmail, string password, CancellationToken ct = default);

    Task SendRegistrationConfirmedAsync(string toEmail, CancellationToken ct = default);
}
