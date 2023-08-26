using System.Net;

namespace WebApp.Models
{
    public class ResponseModel
    {
        public string Status { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
    public enum StatusType { 
    success,
    error,
    info
    }

}
