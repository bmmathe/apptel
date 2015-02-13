using System;

namespace AppTel.WinService.Models
{
    public class AppError
    {
        public string ApplicationName { get; set; }
        public Exception Exception { get; set; }
    }
}
