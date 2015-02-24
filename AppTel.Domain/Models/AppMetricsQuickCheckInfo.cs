using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTel.Domain.Models
{
    public class AppMetricsQuickCheckInfo
    {
        public string Name { get; set; }
        public int TotalPingsLastHour { get; set; }
        public int FailedPingsLastHour { get; set; }
        public int TotalPulsesLastHour { get; set; }
        public int FailedPulsesLastHour { get; set; }
    }
}
