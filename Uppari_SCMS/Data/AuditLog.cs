using NetTopologySuite.Geometries;

namespace Uppari_SCMS.Data
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public string TableName { get; set; } = string.Empty;
        public string KeyValues { get; set; } = string.Empty;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string ActionType { get; set; } = string.Empty; // Added, Modified, Deleted
        public int UserId { get; set; }
        public Point? Location { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
