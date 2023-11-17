using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModule.Dto
{
    public class RoleDto
    {
        public string RoleName { get; set; }
    }

    public class RoleEditDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
