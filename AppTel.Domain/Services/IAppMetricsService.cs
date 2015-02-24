using System.Collections.Generic;
using AppTel.Domain.Data;
using AppTel.Domain.Models;

namespace AppTel.Domain.Services
{
    public interface IAppMetricsService
    {
        AppMetrics GetAppMetrics(string applicationName);
        IEnumerable<AppMetricsQuickCheckInfo> GetAllAppsQuickCheck();
    }
}
