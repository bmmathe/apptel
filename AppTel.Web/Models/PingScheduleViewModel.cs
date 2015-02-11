using System.Collections.Generic;

namespace AppTel.Web.Models
{
    public class PingScheduleViewModel
    {
        public PingScheduleViewModel()
        {
            Jobs = new List<JobModel>();
        }
        public List<JobModel> Jobs { get; set; }
        public string AppTelServiceBaseURL { get; set; }
    }
}