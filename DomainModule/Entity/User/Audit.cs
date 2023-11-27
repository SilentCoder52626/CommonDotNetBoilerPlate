using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.Entity
{
    public class Audit
    {
        public virtual int Id { get; set; }
        public virtual string? IpAddress { get; set; }
        public virtual string? Browser { get; set; }
        public virtual string? UserId { get; set; }
        public virtual string? Type { get; set; }
        public virtual string? TableName { get; set; }
        public virtual DateTime DateTime { get; set; }
        public virtual string? OldValues { get; set; }
        public virtual string? NewValues { get; set; }
        public virtual string? AffectedColumns { get; set; }
        public virtual string? PrimaryKey { get; set; }
    }          
}
