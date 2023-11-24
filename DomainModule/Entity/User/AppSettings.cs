using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.Entity
{
    public class AppSettings
    {
        public virtual int Id { get; set; }
        public virtual string UserId { get; set; }
        public virtual string Key { get; set; }
        public virtual string Value { get; set; }
    }
}
