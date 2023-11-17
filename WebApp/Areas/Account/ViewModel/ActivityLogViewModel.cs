namespace WebApp.Areas.Account.ViewModel
{
    public class ActivityLogViewModel
    {
        public string? Area { get; set; }
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
        public string? IpAddress { get; set; }
        public string? PageAccessed { get; set; }
        public string? SessionId { get; set; }
        public string? UrlReferrer { get; set; }
        public string? UserId { get; set; }
        public string? Status { get; set; }
        public string? ActionOn { get; set; }
        public string? Data { get; set; }
        public string? UserName { get; set; }
        public string? Browser { get; set; }
    }
}
