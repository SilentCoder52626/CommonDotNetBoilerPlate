namespace WebApp.ViewModel
{
    public class PermissionViewModel
    {
        public string RoleId { get; set; }
        public IList<ModuleWisePermissionViewModel> Permissions { get; set; } = new List<ModuleWisePermissionViewModel>();
    }
    public class ModuleWisePermissionViewModel
    {
        public string Module { get; set; }
        public bool IsAssignedAll => PermissionData.All(a => a.IsAssigned);
        public IList<PermissionValuesViewModel> PermissionData { get; set; } = new List<PermissionValuesViewModel>();
    }
    public class PermissionValuesViewModel
    {
        public bool IsAssigned { get; set; }
        public string Value { get; set; }

    }
}
