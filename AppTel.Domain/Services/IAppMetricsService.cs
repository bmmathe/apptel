using AppTel.Domain.Data;

namespace AppTel.Domain.Services
{
    public interface IAppMetricsService
    {
        AppMetrics GetAppMetrics(string applicationName);
    }
}
