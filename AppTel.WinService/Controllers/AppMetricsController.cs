using System.Web.Http;
using AppTel.Domain.Data;
using AppTel.Domain.Services;

namespace AppTel.WinService.Controllers
{
    public class AppMetricsController: ApiController
    {
        private IAppMetricsService _appMetricsService;

        public AppMetricsController(IAppMetricsService appMetricsService)
        {
            _appMetricsService = appMetricsService;
        }

        public AppMetrics Get(string id)
        {
            return _appMetricsService.GetAppMetrics(id);
        }
    }
}
