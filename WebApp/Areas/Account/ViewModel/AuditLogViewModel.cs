namespace WebApp.Areas.Account.ViewModel
{
    public class AuditLogViewModel
    {
        public long Id { get; set; }
        public string? UserName { get; set; }
        public string? ActionOn { get; set; }
        public string? Type { get; set; }
        public string? TableName { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? AffectedColumns { get; set; }
    }
}
