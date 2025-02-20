namespace Docsm.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string body);
        Task SendConfirmEmailAsync(string email);
        Task SendResetPasswordAsync(string email);
    }
}
