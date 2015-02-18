using System;
using System.Collections.Generic;

namespace AppTel.WinService.Models
{
    public class AppMetricsModel
    {
        public AppMetricsModel()
        {
            Pings = new List<PingModel>();
            Pulses = new List<PulseModel>();
            Exceptions = new List<AppErrorModel>();
        }

        public string Name { get; set; }
        public int PingsPerHour { get; set; }
        public int PulsesPerHour { get; set; }
        public List<PingModel> Pings { get; set; }
        public List<PulseModel> Pulses { get; set; }
        public List<AppErrorModel> Exceptions { get; set; }
    }
}
