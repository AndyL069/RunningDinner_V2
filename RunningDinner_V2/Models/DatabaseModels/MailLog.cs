using System;

namespace RunningDinner.Models.DatabaseModels
{
    public class MailLog
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string MailTo { get; set; }

        public string Subject { get; set; }

        public string ExceptionMessage { get; set; }

        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }
    }
}
