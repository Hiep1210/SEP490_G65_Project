namespace verbum_service_application.Service
{
    public interface MailService
    {
        Task<string> SendEmailAsync(string email, string subject, string body);
    }
}
