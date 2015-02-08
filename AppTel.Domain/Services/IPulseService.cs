using System.Collections.Generic;
using AppTel.Domain.Data;

namespace AppTel.Domain.Services
{
    public interface IPulseService
    {
        void Pulse(string applicationName, string endpoint, int? elapsedTime);
        List<Pulse> GetPulses(string applicationName);
    }
}
