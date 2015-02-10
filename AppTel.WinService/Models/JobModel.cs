namespace AppTel.WinService.Models
{
    public class JobModel
    {
        public string ApplicationName { get; set; }
        public string Endpoint { get; set; }
        public string JobName { get; set; }
        public string TriggerName { get; set; }
        public int RepeatIntervalInSeconds { get; set; }
        public string NextFireTime { get; set; }
        public string PreviousFireTime { get; set; }
        public string TriggerState { get; set; }
    }
}
