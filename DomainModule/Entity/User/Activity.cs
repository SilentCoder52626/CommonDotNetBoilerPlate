using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModule.Entity
{
    public class Activity
    {
        public virtual int Id { get; set; }
        public virtual string? Area { get; set; }
        public virtual string? ControllerName { get; set; }
        public virtual string? ActionName { get; set; }
        public virtual string? IpAddress { get; set; }
        public virtual string? PageAccessed { get; set; }
        public virtual string? SessionId { get; set; }
        public virtual string? UrlReferrer { get; set; }
        public virtual string? UserId { get; set; }
        public virtual string? UserName { get; set; }
        public virtual string? Browser { get; set; }
        public virtual string? Data { get; set; }
        public virtual string? QueryString { get; set; }
        public virtual DateTime ActionOn { get; set; }
        public virtual string? Status { get; set; }


    }



}
