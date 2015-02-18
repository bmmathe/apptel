using System;

namespace AppTel.WinService.Models
{
    public class PingModel
    {
        public DateTime CreatedDate { get; set; }
        public string Endpoint { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }    
    }
}
