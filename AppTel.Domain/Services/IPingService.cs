namespace AppTel.Domain.Services
{
    public interface IPingService
    {
        void Ping(string applicationName, string endpoint, string detectText = null);
    }
}