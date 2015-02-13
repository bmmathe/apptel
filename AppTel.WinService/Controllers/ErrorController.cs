using System.Web.Http;
using AppTel.Domain.Services;
using AppTel.WinService.Models;

namespace AppTel.WinService.Controllers
{
    public class ErrorController : ApiController
    {
        private IErrorService _errorService;

        public ErrorController(IErrorService errorService)
        {
            _errorService = errorService;
        }

        [HttpPost]
        public void Create([FromBody] AppError error)
        {
            _errorService.LogError(error.ApplicationName, error.Exception);
        }
    }
}
