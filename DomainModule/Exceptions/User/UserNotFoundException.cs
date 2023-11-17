using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.Exceptions
{
    public class UserNotFoundException:Exception
    {
        public UserNotFoundException(string message ="User Not Found Exception"):base(message)
        {
            
        }
    }
}
