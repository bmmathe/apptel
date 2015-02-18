using System;

namespace AppTel.WinService.Models
{
    public class AppErrorModel
    {
        public string ApplicationName { get; set; }
        public Exception Exception { get; set; }
    }
}
