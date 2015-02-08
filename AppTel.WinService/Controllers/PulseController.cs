using System.Web.Http;
using AppTel.Domain.Services;
using AppTel.WinService.Models;

namespace AppTel.WinService.Controllers
{
    public class PulseController : ApiController
    {
        private IPulseService _pulseService;

        public PulseController(IPulseService pulseService)
        {
            _pulseService = pulseService;
        }

        // GET api/values/5
        public PulseModel[] Get(string applicationName)
        {
            return new PulseModel[]{};
        }

        // POST api/values
        public void Post([FromBody]PulseModel pulse)
        {
            _pulseService.Pulse(pulse.ApplicationName, pulse.EndPoint, pulse.ElapsedTime);
        }        
    }
}
