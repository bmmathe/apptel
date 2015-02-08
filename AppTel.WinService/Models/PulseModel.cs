using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTel.WinService.Models
{
    public class PulseModel
    {
        public string ApplicationName { get; set; }
        public string EndPoint { get; set; }
        public int? ElapsedTime { get; set; }
    }
}
