﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppTel.Web.Models
{
    public class JobModel
    {
        [DisplayName("Application Name")]
        [Required]
        public string ApplicationName { get; set; }
        [DisplayName("Endpoint")]
        [Required]
        public string Endpoint { get; set; }
        [DisplayName("Job Name")]
        public string JobName { get; set; }
        public string TriggerName { get; set; }
        [DisplayName("Interval (in seconds)")]
        [Required]            
        public int RepeatIntervalInSeconds { get; set; }
        public string NextFireTime { get; set; }
        public string PreviousFireTime { get; set; }        
        [DisplayName("Trigger State")]
        public string TriggerState { get; set; }
    }
}