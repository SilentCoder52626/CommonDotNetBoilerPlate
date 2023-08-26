using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.Exceptions
{
    public class RoleNotFoundException:Exception
    {
        public RoleNotFoundException( string message ="Role Not Found"):base(message)
        {
            
        }
    }
}
