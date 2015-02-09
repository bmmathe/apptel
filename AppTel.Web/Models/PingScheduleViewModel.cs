using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTel.Web.Models
{
    public class PingScheduleViewModel
    {
        public PingScheduleViewModel()
        {
            Jobs = new List<JobModel>();
        }
        public List<JobModel> Jobs { get; set; }
    }
}