using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.Dto
{
    public class PermissionDto
    {
        public string RoleId { get; set; }
        public IList<ModuleWisePermissionDto> Permissions { get; set; } = new List<ModuleWisePermissionDto>();
    }
    public class ModuleWisePermissionDto
    {
        public string Module { get; set; }
        public bool IsAssignedAll => PermissionData.All(a => a.IsAssigned);
        public IList<PermissionValues> PermissionData { get; set; } = new List<PermissionValues>();
    }
    public class PermissionValues
    {
        public bool IsAssigned { get; set; }
        public string Value { get; set; }

    }
}
