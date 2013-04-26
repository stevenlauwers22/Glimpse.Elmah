using System;

namespace Glimpse.Elmah.Plumbing.GetErrors
{
    public class ErrorModel
    {
        public string Id { get; set; }
        public string HostName { get; set; }
        public int StatusCode { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }
        public string DetailsUrl { get; set; }
    }
}