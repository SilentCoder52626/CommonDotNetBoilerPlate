using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModule.Dto.ActivityLog
{
    public class ActivityLogDto
    {
        public ActivityLogDto()
        {
            ActionOn = DateTime.Now;
        }
        public string? Area { get;  set; }
        public string? Status { get;  set; }
        public string? QueryString { get;  set; }
        public string? Data { get;  set; }
        public string? ControllerName { get;  set; }
        public string? ActionName { get;  set; }
        public string? IpAddress { get;  set; }
        public string? PageAccessed { get;  set; }
        public string? SessionId { get;  set; }
        public string? UrlReferrer { get;  set; }
        public string? UserId { get;  set; }
        public string? UserName { get;  set; }
        public string? Browser { get;  set; }
        public DateTime ActionOn { get; set; }


    }
}
